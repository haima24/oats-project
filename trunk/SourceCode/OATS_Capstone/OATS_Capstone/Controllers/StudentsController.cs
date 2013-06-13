using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OATS_Capstone.Models;
using TugberkUg.MVC.Helpers;
namespace OATS_Capstone.Controllers
{
    public class StudentsController : Controller
    {
        //
        // GET: /Students/
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MakeStudent()
        {

            var db = SingletonDb.Instance();
            var user = new User();            
            user.UserMail = string.Empty;
            
            db.Users.Add(user);
            if (db.SaveChanges() > 0) {
                var roleMapping = new UserRoleMapping();
                roleMapping.ClientUserID = user.UserID;
                roleMapping.RoleID = 2;
                var owner = AuthenticationSessionModel.Instance().OwnerUser;
                owner.OwnerUser_UserRoleMappings.Add(roleMapping);
                db.SaveChanges();
            }
            var generateId = user.UserID;
            return RedirectToAction("NewStudent", new { id = generateId});
        }
        public ActionResult NewStudent(int id)
        {
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i=>i.UserID == id);
            return View(user);
        }
        public JsonResult AssignTestToStudent(int userId, int testId)
        {
            var success = false;
            var db = SingletonDb.Instance();
            //var invitation = new Invitation();
            //invitation.UserID = userId;
            //invitation.TestID = testId;

            //var generatedHtml = string.Empty;
            

            //if (userId !=0 && testId != 0)
            //{
            //    db.Invitations.Add(invitation);
            //    db.SaveChanges();
            //    success = true;
            //    generatedHtml =  this.RenderPartialViewToString("P_Assign_Test_To_Student",invitation.User );
            //}
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
                    generatedHtml = this.RenderPartialViewToString("P_Assign_Test_To_Student", user);
                }
            }
            return Json(new {success=success,generatedHtml=generatedHtml});

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
                    generatedHtml = this.RenderPartialViewToString("P_Assign_Test_To_Student", user);
                }
            }
            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, generatedHtml});
        }
    }
}
