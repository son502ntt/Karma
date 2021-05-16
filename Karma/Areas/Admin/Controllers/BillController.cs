using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Karma.Models;

namespace Karma.Areas.Admin.Controllers
{
    public class BillController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "LKoB1wtpiSNxhWkXcEtIZfhCt9DnPqy9N0JDp9tp",
            BasePath = "https://karma-ddc59-default-rtdb.firebaseio.com/"

        };
        private static string ApiKey = "AIzaSyBIumAxnSssgty3e16QnkzmBVB3GqFvqqM";
        private static string AuthEmail = "sontrinh502tb@gmail.com";
        private static string AuthPassword = "123456";
        private static string Bucket = "karma-ddc59.appspot.com";
        IFirebaseClient client;
        // GET: Admin/Product
        public ActionResult Index()
        {
            if (Session["User"] == null)
                return RedirectToAction("Login", "Account");
            else
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("Bill");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                if (data != null)
                {
                    var list = new List<Bill>();
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Bill>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("BillDetail");
            dynamic dataProducts = JsonConvert.DeserializeObject<dynamic>(response.Body);
            if (dataProducts != null)
            {
                var dataBill = new List<BillDetail>();
                foreach (var item in dataProducts)
                {
                    var data = JsonConvert.DeserializeObject<BillDetail>(((JProperty)item).Value.ToString());
                    if (data.MaDH == id)
                    {
                        dataBill.Add(data);
                    }
                    else
                    {
                        continue;
                    }
                }
                return View(dataBill);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("BillDetail/" + id);
            BillDetail data = JsonConvert.DeserializeObject<BillDetail>(response.Body);
            FirebaseResponse responseBill = client.Get("Bill/" + data.MaDH);
            Bill dataBill = JsonConvert.DeserializeObject<Bill>(responseBill.Body);
            dataBill.MaDH = dataBill.MaDH;
            dataBill.IdKH = dataBill.IdKH;
            dataBill.NgayGD = dataBill.NgayGD;
            dataBill.TongTien = dataBill.TongTien - data.ThanhTien;
            dataBill.TongSoLuong = dataBill.TongSoLuong - data.SoLuong;
            FirebaseResponse delete = client.Delete("BillDetail/" + id);
            SetResponse setResponse = client.Set("Bill/" + dataBill.MaDH, dataBill); 
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteBill(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse responseBill = client.Delete("Bill/" + id);
            FirebaseResponse response = client.Get("BillDetail");
            dynamic dataProducts = JsonConvert.DeserializeObject<dynamic>(response.Body);
            if (dataProducts != null)
            {
                var dataBill = new List<string>();
                foreach (var item in dataProducts)
                {
                    var data = JsonConvert.DeserializeObject<BillDetail>(((JProperty)item).Value.ToString());
                    if (data.MaDH == id)
                    {
                        dataBill.Add(data.MaCT);
                    }
                    else
                    {
                        continue;
                    }
                }
                foreach (var item in dataBill)
                {
                    FirebaseResponse delete = client.Delete("BillDetail/" + item);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}