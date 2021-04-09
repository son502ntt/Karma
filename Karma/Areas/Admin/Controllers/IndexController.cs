using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {
        // GET: Admin/Index
        public ActionResult IndexAdmin()
        {

            if (Session["User"] == null)
                return RedirectToAction("Index", "Login");
            else
                return View();
        }
    }
}