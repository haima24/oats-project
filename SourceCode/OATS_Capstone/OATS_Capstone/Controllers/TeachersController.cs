using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OATS_Capstone.Models;
using TugberkUg.MVC.Helpers;


namespace OATS_Capstone.Controllers
{
    public class TeachersController : Controller
    {
        //
        // GET: /Teachers/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MakeTeacher()
        {

            var db = SingletonDb.Instance();
            var user = new User();
            user.UserMail = string.Empty;
            db.Users.Add(user);
            if (db.SaveChanges() > 0)
            {
                var roleMapping = new UserRoleMapping();
                roleMapping.ClientUserID = user.UserID;
                roleMapping.RoleID = 3;
                var owner = AuthenticationSessionModel.Instance().OwnerUser;
                owner.OwnerUser_UserRoleMappings.Add(roleMapping);
                db.SaveChanges();
            }
            var generateId = user.UserID;
            return RedirectToAction("NewTeacher", new { id = generateId});
        }
        public ActionResult NewTeacher(int id)
        {
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i => i.UserID == id);
            return View(user);
        }
        public JsonResult AssignTestToTeacher(int userId, int testId)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var generatedHtml = string.Empty;
            var user = db.Users.FirstOrDefault(i => i.UserID == userId);//find user in db
            var test = db.Tests.FirstOrDefault(k => k.TestID == testId);//find test in db
            if (user != null && test != null)
            {
                var invitation = new Invitation();
                invitation.Test = test;
                user.Invitations.Add(invitation);
                if (db.SaveChanges() > 0)
                {
                    success = true;
                    generatedHtml = this.RenderPartialViewToString("P_Assign_Test_To_Teacher", user);
                }
            }
            return Json(new { success = success, generatedHtml = generatedHtml });

        }
        public JsonResult UnassignTest(int userId, int testId)
        {
            var success = false;
            var generatedHtml = string.Empty;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);
                var unassignedInvitation = user.Invitations.FirstOrDefault(i => i.TestID == testId);
                user.Invitations.Remove(unassignedInvitation);

                //var test = db.Tests.FirstOrDefault(i => i.TestID == testId);
                //test.Invitations.Remove(unassignedInvitation);
                db.Invitations.Remove(unassignedInvitation);
                if (db.SaveChanges() > 0)
                {
                    success = true;
                    generatedHtml = this.RenderPartialViewToString("P_Assign_Test_To_Teacher", user);
                }
            }
            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, generatedHtml });
        }

    }
}
