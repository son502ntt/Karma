using Firebase.Auth;
using Karma.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karma.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private static string ApiKey = "AIzaSyBIumAxnSssgty3e16QnkzmBVB3GqFvqqM";
        private static string Bucket = "karma-ddc59-default-rtdb.firebaseio.com";
        // GET: Account
        // GET: Admin/Account
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(SignUp model)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));

                var a = await auth.CreateUserWithEmailAndPasswordAsync(model.Email, model.Password, model.Name, true);
                ModelState.AddModelError(string.Empty, "Please Verify your email then login Plz.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        //[AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            HttpCookie cookie = Request.Cookies["RememberUser"];
            if (cookie != null)
            {
                ViewBag.Email = cookie["Email"].ToString();
                ViewBag.Password = cookie["Pass"].ToString();

            }
            try
            {
                // Verification.
                if (this.Request.IsAuthenticated)
                {

                    //  return this.RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return this.View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
        public async Task<ActionResult> Login(Login model, string returnUrl)
        {

            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    HttpCookie cookie = new HttpCookie("RememberUser");
                    if(model.RememberMe == true)
                    {
                        cookie["Email"] = model.Email;
                        cookie["Pass"] = model.Password;
                        cookie.HttpOnly = true;
                        cookie.Expires.AddYears(1);
                        HttpContext.Response.Cookies.Add(cookie);

                    }
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var ab = await auth.SignInWithEmailAndPasswordAsync(model.Email, model.Password);
                    string token = ab.FirebaseToken;
                    var user = ab.User;
                    if (token != "")
                    {

                        this.SignInUser(user.Email, token, false);
                        Session["User"] = user.Email;
                        return RedirectToAction("IndexAdmin", "Index");

                    }
                    else
                    {
                        // Setting.
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }
        private void SignInUser(string email, string token, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Authentication, token));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                //var ctx = Request.GetOwinContext();
                //var authenticationManager = ctx.Authentication;
                // Sign In.
                //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                Console.WriteLine("Loi: " + ex.Message);
                throw ex;
            }
        }
        private void ClaimIdentities(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();
            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }
        public ActionResult LogOut()
        {
            Session["User"] = null; // remove session
            if (Request.Cookies["user"] != null)
            {
                var user = new HttpCookie("user")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Value = null
                };
                Response.SetCookie(user);
            } // Xoa cookie 
            return RedirectToAction("Login", "Account"); //Chuyen huong sang trang dang nhap
        }
    }
}