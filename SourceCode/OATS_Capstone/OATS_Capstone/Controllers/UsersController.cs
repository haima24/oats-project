﻿using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugberkUg.MVC.Helpers;

namespace OATS_Capstone.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        public JsonResult ProfilePopup()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var generatedHtml = String.Empty;
            try
            {
                var authen = AuthenticationSessionModel.Instance();
                generatedHtml = this.RenderPartialViewToString("P_Profile_Popup", authen);
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, generatedHtml });
        }
        public JsonResult UsersSearch(string term)
        {
            var common = new CommonService();
            common.UsersSearch(term);
            return Json(new { common.resultlist, common.message, common.success });
        }
        public JsonResult UpdateUserEmail(int userId, string userEmail)
        {
            var common = new CommonService();
            common.UpdateUserEmail(userId, userEmail);
            return Json(new { common.success, common.message });

        }
        public JsonResult UpdateUserName(int userId, string userName)
        {
            var common = new CommonService();
            common.UpdateUserName(userId, userName);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateProfile(User profile)
        {
            var common = new CommonService();
            common.UpdateProfile(profile);
            return Json(new { common.message, common.success });
        }
        public JsonResult IsMatchOldPass(string pass)
        {
            var common = new CommonService();
            var ismatch = common.IsMatchOldPass(pass);
            return Json(new { ismatch, common.message, common.success });
        }
        public JsonResult RemoveNonRegisteredUser(int userid)
        {
            var common = new CommonService();
            common.RemoveNonRegisteredUser(userid);
            return Json(new { common.success, common.message });
        }
        public JsonResult MakeUser(string name,string email)
        {
            var common = new CommonService();
            var generatedId=common.MakeUser(name,email);
            return Json(new { common.success, common.message, generatedId });
        }
        public JsonResult EmailInput(string role)
        {
            ViewBag.Role = role;
            var generatedHtml=this.RenderPartialViewToString("P_Email_Input");
            return Json(new { generatedHtml });
        }
        public JsonResult ImportUsers(List<User> users)
        {
            var common = new CommonService();
            common.ImportUsers(users);
            return Json(new {common.message,common.success,common.resultlist });
        }
    }
}
