using Karma.Areas.Admin.CodeSession;
using Karma.Areas.Admin.Models;
using Karma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Karma.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        ShopDbContextDataContext db = new ShopDbContextDataContext();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["User"] != null)
                return RedirectToAction("IndexAdmin","Index");
            else
            {
                HttpCookie cookie = Request.Cookies["userremember"]; // get cookies from browser
                if (cookie != null) // neu ton tai thi thuc thi cau lenh duoi
                {
                    ViewBag.Username = cookie["username"].ToString(); // get cookies username from cookie
                    ViewBag.Password = cookie["password"].ToString();// get cookies pắord from cookie
                }
                return View();
            }
                
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            var check = db.Admins.FirstOrDefault(x => x.UserName == model.UserName && x.PassWord == model.PassWord); // select username = voi username nhap vao password = voi password nhap vao

            if (model.RememberMe == true)// neu remember me == true thi thuc thi cau lenh duoi 
            {
                HttpCookie cookie = new HttpCookie("userremember"); // get cookies
                cookie["username"] = check.UserName;// get cookies for uẻname
                cookie["password"] = check.PassWord;// get cookies for pasword
                cookie.Expires = DateTime.Now.AddMinutes(2);  // set HSD 2'
                HttpContext.Response.SetCookie(cookie); // set cookies
            }
            else
            {

            }

            if (check != null)
            {
                ViewBag.Mess = "Success !!!"; // thong bao
                Session["User"] = check.ID; // set Session
                HttpCookie UserCookie = new HttpCookie("user", check.ID.ToString()); // Get Cookie
                UserCookie.HttpOnly = true; // Chi ap dung vs phuong thuc HTTP
                UserCookie.Expires = DateTime.Now.AddYears(1); // set HSD 1 nam
                HttpContext.Response.SetCookie(UserCookie); // Set Cookie
                return RedirectToAction("IndexAdmin", "Index");// Chuyen huong sang trang index
            }
            else
            {
                ViewBag.Mess = "Failed !!!";// thong bao
            }
            return this.Index();
        }
        public ActionResult Logout()
        {
            Session["User"] = null; // remove session
            if (Request.Cookies["user"] != null)
            {
                var user = new HttpCookie("user")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                Response.SetCookie(user);
            } // Xoa cookie 
            return RedirectToAction("Index", "Login"); //Chuyen huong sang trang dang nhap
        }
    }

}