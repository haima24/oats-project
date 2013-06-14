using OATS_Capstone.Models;
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
                var authen=AuthenticationSessionModel.Instance();
                generatedHtml = this.RenderPartialViewToString("P_Profile_Popup", authen);
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message,generatedHtml });
        }
        public JsonResult UsersSearch()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listuser = new List<SearchingUsers>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var ownerId = authen.OwnerUserId;
                var users = db.Users.ToList();
                users.ForEach(delegate(User user)
                {
                    var userTemplate = new SearchingUsers();
                    userTemplate.UserID = user.UserID;
                    userTemplate.LastName = user.LastName;
                    userTemplate.FirstName = user.FirstName;
                    var roleMap = db.UserRoleMappings.FirstOrDefault(i => i.ClientUserID == user.UserID && i.OwnerDomainUserID == ownerId);
                    if (roleMap != null)
                    {
                        userTemplate.RoleName = roleMap.Role.RoleDescription;
                    }
                    listuser.Add(userTemplate);
                });
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { listuser, message, success });
        }
        public JsonResult UpdateUserEmail(int userId, string userEmail)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i => i.UserID == userId);//may be null
            if (user != null)
            {
                user.UserMail = userEmail;
                if (db.SaveChanges() > 0) //SaveChanges return affected rows
                {
                    success = true;
                }
            }

            return Json(new { success });

        }
        public JsonResult UpdateUserName(int userId, string userName)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i => i.UserID == userId);
            if (user != null)
            {
                user.FirstName = userName;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }
            }
            return Json(new { success });
        }
    }
}
