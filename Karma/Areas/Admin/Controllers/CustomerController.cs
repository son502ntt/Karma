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
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "LKoB1wtpiSNxhWkXcEtIZfhCt9DnPqy9N0JDp9tp",
            BasePath = "https://karma-ddc59-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: Admin/Customer
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customers");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                //var path = "";
                //if (file.ContentLength > 0)
                //{
                //    if ((Path.GetExtension(file.FileName).ToLower() == ".jpg") || (Path.GetExtension(file.FileName).ToLower() == ".png"))
                //    {
                //        path = Path.Combine(Server.MapPath("~/Content/img/mobiles/"), file.FileName);

                //    }

                //}
                AddCustomerToFirebase(customer);
                ModelState.AddModelError(string.Empty, "Added Successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index", "Customer");
        }
        private void AddCustomerToFirebase(Customer customer)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = customer;
            PushResponse response = client.Push("Customers/", data);
            data.ID = response.Result.name;
            SetResponse setResponse = client.Set("Customers/" + data.ID, data);
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