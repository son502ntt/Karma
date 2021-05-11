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
            List<Cart> listCart = GetCart();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(listCart);
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
                    cart = new Cart(MaSP, dataProduct.Gia, soluong, dataProduct.TenSanPham, dataProduct.AnhSanPham);
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
                ct.TongTien = TongTien();
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
                ct.TongTien = TongTien();
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

        [HttpPost]
        public JsonResult UpdateQty(string id, int sl)
        {
            List<Cart> listCart = GetCart();
            //bug get id
            Cart cart = listCart.SingleOrDefault(m => m.MaSanPham == id);
            cart.SoLuong = sl;

            cart.ThanhTien = cart.SoLuong * cart.DonGia;
            cart.TongTien = TongTien();

            var result = cart;
            return Json(new
            {
                status = result
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "User");
            else
            {
                List<Cart> listCart = GetCart();
                if(listCart.Count == 0)
                {
                    Response.Write("<script language='Javascript'>alert('Giỏ hàng hiện đang rỗng !!!'); location.href='/Shop/ShopCategory';</script>");
                }
                else
                {
                    ViewBag.TongTien = TongTien();
                    return View(listCart);
                }
                return View();
            }
        }
        [HttpPost]
        public JsonResult Checkout(string id)
        {
            List<Cart> listCart = GetCart();
            Customer kh = (Customer)Session["User"];
            Bill bill = new Bill();
            bill.MaDH = Guid.NewGuid().ToString();
            bill.IdKH = kh.ID;
            bill.NgayGD = DateTime.Now.Date;
            bill.TongSoLuong = TongSoLuong();
            bill.TongTien = TongTien();

            products = new FireSharp.FirebaseClient(config);
            PushResponse billResponse = products.Push("Bill/", bill);
            foreach(var item in listCart)
            {
                BillDetail billDetail = new BillDetail();
                billDetail.MaDH = bill.MaDH;
                billDetail.MaSanPham = item.MaSanPham;
                billDetail.TenSanPham = item.TenSanPham;
                billDetail.SoLuong = item.SoLuong;
                billDetail.ThanhTien = item.ThanhTien;

                PushResponse detailResponse = products.Push("BillDetail/", billDetail);

                FirebaseResponse productresponse = products.Get("Products/" + item.MaSanPham);
                Products data = JsonConvert.DeserializeObject<Products>(productresponse.Body);
                data.SoLuong = data.SoLuong - billDetail.SoLuong;
                SetResponse response = products.Set("Products/" + item.MaSanPham, data);
            }
            Session["Cart"] = null;
            return Json(new
            {

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirmation()
        {
            return View();
        }
    }
}