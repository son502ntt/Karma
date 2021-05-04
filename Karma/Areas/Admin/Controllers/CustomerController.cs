using Firebase.Auth;
using Firebase.Storage;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Areas.Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "LKoB1wtpiSNxhWkXcEtIZfhCt9DnPqy9N0JDp9tp",
            BasePath = "https://karma-ddc59-default-rtdb.firebaseio.com/"

        };
        private static string ApiKey = "AIzaSyBIumAxnSssgty3e16QnkzmBVB3GqFvqqM";
        private static string AuthEmail = "sontrinh502tb@gmail.com";
        private static string AuthPassword = "123456789";
        private static string Bucket = "karma-ddc59.appspot.com";
        IFirebaseClient client;
        // GET: Admin/Customer
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("Customers");// get json
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);// boc tach json
                if (data != null)
                {
                    var list = new List<Customer>();
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
                else
                {
                    return View();
                }
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Customer customer, HttpPostedFileBase file)
        {
            FileStream stream;
            if (file.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Assets/User/img/"), file.FileName);
                file.SaveAs(path);
                stream = new FileStream(Path.Combine(path), FileMode.Open);
                var link = await Upload(stream, file.FileName);
                customer.Avatar = link;
                var result = AddCustomerToFirebase(customer);
                if (result == true)
                {
                    ViewBag.MessSuccess = "Đăng ký thành công !!!";
                }
                else
                {
                    ViewBag.MessFailed = "Email đã được đăng ký";
                }

                return View(customer);
            }
            return View(customer);
        }
        public async Task<string> Upload(FileStream stream, string fileName)
        {

            var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
            string link = "";
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("images")
                .Child(fileName)
                .PutAsync(stream, cancellation.Token);
            try
            {
                link = await task;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }
            return link;
        }
        private bool AddCustomerToFirebase(Customer customer)
        {
            customer.ID = Guid.NewGuid().ToString();
            client = new FireSharp.FirebaseClient(config);// lấy quyến truy data
            FirebaseResponse responseUser = client.Get("Customers");// lấy dât từ bảng "" => json
            dynamic dataUser = JsonConvert.DeserializeObject<dynamic>(responseUser.Body);// bóc tách dữ liệu từ json
            if (dataUser == null)
            {
                var data1 = customer;

                SetResponse response1 = client.Set("Customers/" + customer.ID, data1);
                return true;
            }
            foreach (var item in dataUser)
            {
                var list = JsonConvert.DeserializeObject<Customer>(((JProperty)item).Value.ToString());
                if (list.Email == customer.Email)
                {
                    return false;
                }
            }

            var data = customer;

            SetResponse response = client.Set("Customers/" + customer.ID, data);       
            return true;
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customers/" + id);
            Customer data = JsonConvert.DeserializeObject<Customer>(response.Body);
            return View(data);
            
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customers/" + id);
            Customer data = JsonConvert.DeserializeObject<Customer>(response.Body);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Customers/" + customer.ID, customer);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Customers/" + id);
            return RedirectToAction("Index");
        }
    }
}   