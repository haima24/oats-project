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
        public JsonResult TeachersSearch()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listTeachersSearch = new List<SearchingTeachers>();

            try
            {
                var db = SingletonDb.Instance();
                var teachers = AccessDomainSessionModel.Instance().TeachersInThisDomain;
                teachers.ForEach(delegate(User teacher)
                {
                    var teacherTemplate = new SearchingTeachers();
                    teacherTemplate.UserID = teacher.UserID;
                    teacherTemplate.RoleID = teacher.RoleID;
                    teacherTemplate.LastName = teacher.LastName;
                    teacherTemplate.FirstName = teacher.FirstName;
                    listTeachersSearch.Add(teacherTemplate);
                });
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { listTeachersSearch,success,message});
        }
        public ActionResult Index(string subdomain)
        {
            AccessDomainSessionModel.Instance().CurrentSubdomain = subdomain;
            return View();
        }
        public ActionResult MakeTeacher()
        {

            var db = SingletonDb.Instance();
            var user = new User();
            user.UserMail = string.Empty;
            user.RoleID = 3;
            db.Users.Add(user);
            db.SaveChanges();
            var generateId = user.UserID;
            return RedirectToAction("NewTeacher", new { id = generateId, subdomain = AccessDomainSessionModel.Instance().CurrentSubdomain });
        }

        public ActionResult NewTeacher(int id,string subdomain)
        {
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(i => i.UserID == id);
            AccessDomainSessionModel.Instance().CurrentSubdomain = subdomain;
            return View(user);
        }

        public JsonResult NewTeacherByEmail(string email)
        {
            var error = string.Empty;
            var iserror = false;
            var db = SingletonDb.Instance();
            var emailsInDb = db.Users.Select(i => i.UserMail);
            if (!emailsInDb.Contains(email.Trim())) //not exist in db
            {
                var newUser = new User();
                newUser.UserMail = email;

                //She or he is a Teacher
                newUser.RoleID = 3;
                db.Users.Add(newUser);
                var affectedRow = db.SaveChanges();
                if (affectedRow <= 0)
                {
                    //error
                    error = "There was an unhandle problem in server.";
                    iserror = true;
                }
            }
            var result = new
            {
                iserror,
                error
            };
            return Json(result);
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

        public JsonResult AssignTestToTeacher(int userId, int testId)
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
