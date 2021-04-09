using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Areas.Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "LKoB1wtpiSNxhWkXcEtIZfhCt9DnPqy9N0JDp9tp",
            BasePath = "https://karma-ddc59-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: Admin/Product
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Products");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            if (data != null)
            {
                var list = new List<Product>();
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
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
        public ActionResult Create(Product product)
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
                AddCustomerToFirebase(product);
                ModelState.AddModelError(string.Empty, "Added Successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Index", "Product");
        }
        private void AddCustomerToFirebase(Product product)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = product;
            PushResponse response = client.Push("Products/", data);
            data.MaSanPham = response.Result.name;
            SetResponse setResponse = client.Set("Products/" + data.MaSanPham, data);
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Products/" + id);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);
            return View(data);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Products/" + id);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Products/" + product.MaSanPham, product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Products/" + id);
            return RedirectToAction("Index");
        }
    }
}