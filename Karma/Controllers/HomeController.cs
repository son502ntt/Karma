using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;

namespace Karma.Controllers
{
    public class HomeController : Controller
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
        IFirebaseClient products;
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseProducts = products.Get("Products");
            dynamic dataProducts = JsonConvert.DeserializeObject<dynamic>(responseProducts.Body);
            if (dataProducts != null)
            {
                var dataShop = new List<Products>();
                foreach (var item in dataProducts)
                {
                    var data = JsonConvert.DeserializeObject<Products>(((JProperty)item).Value.ToString());
                    dataShop.Add(data);
                }

                return View(dataShop.OrderBy(m => m.GiaGoc).ToList().ToPagedList(pageNum, 8));
            }
            else
            {
                return View();
            }
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult BlogDetails()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}