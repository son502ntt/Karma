using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karma.Controllers
{
    public class ShopController : Controller
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
        // GET: Shop
        public ActionResult ShopCategory(int? page)
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
                
                return View(dataShop.OrderBy(m => m.Gia).ToList().ToPagedList(pageNum, 6));
            }
            else
            {
                return View();
            }
        }
        public ActionResult ProductDetails(string id)
        {
            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseProducts = products.Get("Products/" + id);
            dynamic dataProduct = JsonConvert.DeserializeObject<Products>(responseProducts.Body);
            FirebaseResponse responseCategory = products.Get("Categorys/" + dataProduct.MaLoai);
            dynamic dataCategory = JsonConvert.DeserializeObject<Categories>(responseCategory.Body);
            dataProduct.TenLoai = dataCategory.TenLoai;
            return View(dataProduct);
        }
        public ActionResult ProductsOfCategory(string id, int? page)
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
                    if (data.MaLoai == id)
                    {
                        dataShop.Add(data);
                    }
                    else
                    {
                        continue;
                    }
                }
                return View(dataShop.OrderBy(m => m.Gia).ToList().ToPagedList(pageNum, 6));
            }
            else
            {
                return View();
            }
        }
        public ActionResult ProductCheckout()
        {
            return View();
        }
        public ActionResult CategoriesPart()
        {
            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseCategories = products.Get("Categorys");
            dynamic dataCategories = JsonConvert.DeserializeObject<dynamic>(responseCategories.Body);
            if (dataCategories != null)
            {
                var dataShop = new List<Categories>();

                foreach (var item in dataCategories)
                {
                    dataShop.Add(JsonConvert.DeserializeObject<Categories>(((JProperty)item).Value.ToString()));
                }
                return PartialView(dataShop);
            }
            else
            {
                return PartialView();
            }
        }

        public ActionResult ShoppingCart()
        {
            return View();
        }
        public ActionResult Confirmation()
        {
            return View();
        }
    }
}