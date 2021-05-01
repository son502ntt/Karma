using FireSharp.Interfaces;
using FireSharp.Response;
using Karma.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karma.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
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

        public List<Cart> GetCart()
        {
            List<Cart> listCart = Session["Cart"] as List<Cart>;

            if (listCart == null)
            {
                listCart = new List<Cart>();
                Session["Cart"] = listCart;
            }
            return listCart;
        }
        public ActionResult ShoppingCart()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddToCart(string MaSP, int soluong)
        {
            //get SP
            
            products = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseProducts = products.Get("Products/" + MaSP);
            dynamic dataProduct = JsonConvert.DeserializeObject<Products>(responseProducts.Body);

            List<Cart> listCart = GetCart();

            Cart cart = listCart.Find(m => m.MaSanPham == MaSP);

            bool warning = true;
            
            if(cart == null)
            {
                if(soluong < dataProduct.SoLuong)
                {
                    cart = new Cart(MaSP, dataProduct.Gia, soluong, dataProduct.TenSanPham);
                    warning = false;
                    listCart.Add(cart);

                }
                else
                {
                    var result = cart;
                    return Json(new
                    {
                        warning = true,
                        status =result
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if(cart.SoLuong != dataProduct.SoLuong)
                {
                    cart.SoLuong = cart.SoLuong + soluong;
                    if(cart.SoLuong > dataProduct.SoLuong)
                    {
                        cart.SoLuong = cart.SoLuong - soluong;
                        var result = cart;
                        return Json(new
                        {
                            warning = true,
                            status = result
                        }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        warning = false;
                    }
                }
            }
            if(warning)
            {
                Cart ct = listCart.SingleOrDefault(m => m.MaSanPham == MaSP);
                ct.ThanhTien = TongTien();
                ct.TongSoLuong = TongSoLuong();
                var result = cart;
                return Json(new
                {
                    warning = true,
                    status = result
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Cart ct = listCart.SingleOrDefault(m => m.MaSanPham == MaSP);
                ct.ThanhTien = TongTien();
                ct.TongSoLuong = TongSoLuong();
                var result = cart;
                return Json(new
                {
                    warning = false,
                    status = result
                }, JsonRequestBehavior.AllowGet);

            }
        }

        private double TongTien()
        {
            double? tongTien = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if( listCart != null)
            {
                tongTien = listCart.Sum(m => m.ThanhTien);
            }
            return (double)tongTien;
        }

        private int TongSoLuong()
        {
            int tongSL = 0;
            List<Cart> listCart = Session["Cart"] as List<Cart>;
            if (listCart != null)
            {
                tongSL = listCart.Sum(m => m.SoLuong);
            }
            return tongSL;
        }

        public ActionResult GioHangPart()
        {
            List<Cart> listCart = GetCart();
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView(listCart);
        }
    }
}