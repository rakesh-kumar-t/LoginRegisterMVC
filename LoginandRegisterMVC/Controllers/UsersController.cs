using LoginandRegisterMVC.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace LoginandRegisterMVC.Controllers
{
    public class UsersController : Controller
    {
        private UserContext db = new UserContext();
        // GET: Users
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            using (UserContext db = new UserContext())
            {
                //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1);
                var obj = db.Users.Where(u => u.UserId.Equals(user.UserId)).FirstOrDefault();
                if (obj == null)
                {
                    if (ModelState.IsValid)
                    {
                        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "sha1"); ;
                        user.ConfirmPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.ConfirmPassword, "sha1");
                        db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error Occured! Try again!!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User exists ,Please login with your password");
                }
                return View(user);
            }

            }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using(UserContext db=new UserContext())
            {
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "sha1");

                //FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password,FormsAuthPasswordFormat.SHA1 );
                var obj = db.Users.Where(u => u.UserId.Equals(user.UserId) && u.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserId,false);
                    Session["UserId"] = obj.UserId.ToString();
                    Session["Username"] = obj.Username.ToString();
                    Session["Role"] = obj.Role.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Userid or password wrong");
                }
            }
            return View(user);
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
        
     
    }
}