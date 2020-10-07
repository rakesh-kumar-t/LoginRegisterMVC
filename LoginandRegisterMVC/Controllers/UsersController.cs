using LoginandRegisterMVC.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;


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
                        user.Password = HashPassword(user.Password); ;
                        user.ConfirmPassword = HashPassword(user.ConfirmPassword);
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
            if (db.Users.Where(u=>u.UserId.Equals("admin@demo.com")).FirstOrDefault()==null)
            {
                User user = new User();
                user.UserId = "admin@demo.com";
                user.Username = "admin";
                user.Password = HashPassword("Admin@123");
                user.ConfirmPassword = HashPassword("Admin@123");
                user.Role = "Admin";
                db.Users.Add(user);
                db.SaveChanges();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using(UserContext db=new UserContext())
            {
                user.Password = HashPassword(user.Password);

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
        public string HashPassword(string password)
        {
            var pwdarray = Encoding.ASCII.GetBytes(password);
            var sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(pwdarray);
            var hashpwd = new StringBuilder(hash.Length);
            foreach (byte b in hash)
            {
                hashpwd.Append(b.ToString());
            }
            return hashpwd.ToString();
        }
        //Dispose the database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}