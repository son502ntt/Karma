using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult ShopCategory()
        {
            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseProducts = products.Get("Products");
            FirebaseResponse responseCategories = products.Get("Categorys");
            dynamic dataProducts = JsonConvert.DeserializeObject<dynamic>(responseProducts.Body);
            dynamic dataCategories = JsonConvert.DeserializeObject<dynamic>(responseCategories.Body);
            Shop dataShop = new Shop();
            if (dataProducts != null && dataCategories != null)
            {
                foreach (var item in dataProducts)
                {
                    var data = JsonConvert.DeserializeObject<Products>(((JProperty)item).Value.ToString());// convert tostring r bo vao product
                    dataShop.Products.Add(data);
                }
                foreach (var item in dataCategories)
                {
                    dataShop.Categories.Add(JsonConvert.DeserializeObject<Categories>(((JProperty)item).Value.ToString()));
                }
                return View(dataShop);
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
            return View(dataProduct);// dung model
        }
        public ActionResult ProductsOfCategory(string id)
        {

            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseProducts = products.Get("Products");
            FirebaseResponse responseCategories = products.Get("Categorys");
            dynamic dataProducts = JsonConvert.DeserializeObject<dynamic>(responseProducts.Body);
            dynamic dataCategories = JsonConvert.DeserializeObject<dynamic>(responseCategories.Body);
            Shop dataShop = new Shop();
            if (dataProducts != null && dataCategories != null)
            {
                foreach (var item in dataProducts)
                {
                    var data = JsonConvert.DeserializeObject<Products>(((JProperty)item).Value.ToString());
                    if (data.MaLoai == id)
                    {
                        dataShop.Products.Add(data);
                    }
                    else
                    {
                        continue;
                    }
                }
                foreach (var item in dataCategories)
                {
                    dataShop.Categories.Add(JsonConvert.DeserializeObject<Categories>(((JProperty)item).Value.ToString()));
                }
                return View(dataShop);
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