using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karma.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "LKoB1wtpiSNxhWkXcEtIZfhCt9DnPqy9N0JDp9tp",
            BasePath = "https://karma-ddc59-default-rtdb.firebaseio.com/"

        };
        private static string ApiKey = "AIzaSyBIumAxnSssgty3e16QnkzmBVB3GqFvqqM";
        private static string AuthEmail = "sontrinh502tb@gmail.com";
        private static string AuthPassword = "123456789";
        private static string Bucket = "karma-ddc59.appspot.com";
        IFirebaseClient user;
        // GET: Account
        // GET: Admin/Account
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SignUp(SignUp userSignUp)
        {
            //if(userSignUp.Password != userSignUp.ConfirmPassword)
            //{
            //    ViewBag.NotMatch = "Mat khau khong trung khop";
            //    return View(userSignUp);
            //}    
            var result =  AddCustomerToFirebase(userSignUp);
            if(result == true)
            {
                ViewBag.MessSuccess = "Đăng ký thành công !!!";
            }
            else
            {
                ViewBag.MessFailed = "Email đã được đăng ký";
            }
            
            return View(userSignUp);
        }
        private void IfExistEmail(SignUp userSignUp)
        {

        }
        private bool AddCustomerToFirebase(SignUp userSignUp)
        {
            userSignUp.ID = Guid.NewGuid().ToString();
            user = new FireSharp.FirebaseClient(config);// lấy quyến truy data
            FirebaseResponse responseUser = user.Get("Customers");// lấy dât từ bảng "" => json
            dynamic dataUser = JsonConvert.DeserializeObject<dynamic>(responseUser.Body);// bóc tách dữ liệu từ json
            if(dataUser == null)
            {
                var data1 = userSignUp;

                SetResponse response1 = user.Set("Customers/" + userSignUp.ID, data1);
                

                SendMail(userSignUp);
                return true;
            }
            foreach (var item in dataUser)
            {
                var list = JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString());
                if (list.Email == userSignUp.Email)
                {
                    return false;
                }
            }
            
            var data = userSignUp;

            SetResponse response = user.Set("Customers/" + userSignUp.ID, data);

            SendMail(userSignUp);
            
            return true;


        }
        [NonAction]
        public void SendMail(SignUp userSignUp)
        {
            var url = "/User/VerifyEmail/" + userSignUp.CodeActive;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("qunag7190@gmail.com", "Xác thực email");
            var toEmail = new MailAddress(userSignUp.Email);
            var fromPass = "270134275";

            var subject = "";
            var body = "";
            subject = "Xác thực email của bạn";
            body = "Chúng tôi nhận được yêu cầu đăng ký tài khoản từ email này. <br> Vui lòng nhấp vào link bên dưới để kích hoạt tài khoản: <br> <a href="+ link +"> Kích hoạt tài khoản </a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromPass)
            };

            using (var mess = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(mess);
            Session["Check"] = userSignUp.CodeActive;
        }
        public ActionResult VerifyEmail()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult VerifyEmail(SignUp userSignUp)
        {
            var result = Session["Check"].ToString();
            user = new FireSharp.FirebaseClient(config);// lấy quyến truy data
            FirebaseResponse responseUser = user.Get("Customers");// lấy dât từ bảng "" => json
            dynamic dataUser = JsonConvert.DeserializeObject<dynamic>(responseUser.Body);// bóc tách dữ liệu từ json
            foreach (var item in dataUser)
            {
                var list = JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString());
                if (list.CodeActive == result)
                {
                    list.CodeActive = null;
                    //FirebaseResponse responseId = user.Get("Customers/" + list.ID);
                    SetResponse response = user.Set("Customers/" + list.ID, list);
                    ViewBag.Success = "Kích hoạt tài khoản thành công";
                     return View(userSignUp);
                }
            }
            ViewBag.Failed = "Lỗi không xác định";
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["Remember"];
            if (cookie != null)
            {
                ViewBag.Email = cookie["Email"].ToString();
                ViewBag.Password = cookie["Pass"].ToString();

            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            user = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseUser = user.Get("Customers");
            dynamic dataUser = JsonConvert.DeserializeObject<dynamic>(responseUser.Body);
            foreach (var item in dataUser)
            {
                var list = JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString());
                if (list.Email == login.Email && list.Password == login.Password)
                {
                    if(list.CodeActive == null)
                    {
                        if (login.RememberMe == true)
                        {
                            Session["User"] = list;
                            HttpCookie cookie = new HttpCookie("Remember");
                            cookie["Email"] = login.Email;
                            cookie["Pass"] = login.Password;
                            cookie.HttpOnly = true;
                            cookie.Expires.AddYears(1);
                            HttpContext.Response.Cookies.Add(cookie);
                        }
                        Session["User"] = list; //them sesion de k can remember me
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.MessVRF = "Email chưa đươc kích hoạt";
                    return View(login);
                }
            }
            ViewBag.Mess = "Sai tài khoản hoặc mật khẩu";
            return View(login);
            
        }
        
    }
}