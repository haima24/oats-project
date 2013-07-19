using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugberkUg.MVC.Helpers;

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
        public ActionResult DetailRegister(string id,string forward="")
        {
            ActionResult action = RedirectToActionPermanent("Index", "Account");
            try
            {
                var common = new CommonService();
                var user = common.DetailRegister(id);
                if (user != null)
                {
                    ViewBag.Forward = forward;
                    action = View(user);
                }
            }
            catch (Exception)
            {
                action = RedirectToActionPermanent("Index", "Account");
            }
            return action;
        }
        public JsonResult Login(string email, string password, bool remembered)
        {
            var common = new CommonService();
            common.Login(email, password, remembered);
            return Json(new { common.success, common.message });
        }
        public JsonResult SignUp(User user)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_SignUp_Container");
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.SignUp(user);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public ActionResult LogOut()
        {
            AuthenticationSessionModel.TerminateAuthentication();
            return View("Index");
        }
        public JsonResult ForgotPassword(string email,string connectionid)
        {
            var common = new CommonService();
            common.ForgotPassword(email, connectionid);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateUserPasswordOnRegisterDetail(int userid, string password) {
            var common = new CommonService();
            common.UpdateUserPasswordOnRegisterDetail(userid, password);
            return Json(new { common.message, common.success });
        }
    }
}
