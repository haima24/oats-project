using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OATS_Capstone.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Login(string email, string password)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(delegate(User u) {
                    var uEmail = String.IsNullOrEmpty(u.UserMail)?String.Empty: u.UserMail.Trim();
                    var uPass = String.IsNullOrEmpty(u.Password) ? String.Empty : u.Password.Trim();
                    var iEmail = email.Trim();
                    var iPass = password.Trim();
                    return uEmail.Equals(iEmail) && uPass.Equals(iPass);
                });
                if (user != null)
                {
                    var authen = AuthenticationSessionModel.Instance();
                    authen.UserId = user.UserID;
                    success = true;
                    message = String.Empty;
                }
                else {
                    success = false;
                    message = "Login failed, invalid email or password, pleasy try again.";
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message });
        }
    }
}
