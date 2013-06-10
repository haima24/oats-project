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

        public JsonResult StudentsSearch()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listStudentsSearch = new List<SearchingStudents>();
            try
            {
                var db = SingletonDb.Instance();
                var students = db.Users.Where(i => i.Role.RoleID == 2).ToList();
                students.ForEach(delegate(User student)
                {
                    var studentTemplate = new SearchingStudents();
                    studentTemplate.UserID = student.UserID;
                    studentTemplate.RoleID = student.RoleID;
                    studentTemplate.LastName = student.LastName;
                    studentTemplate.FirstName = student.FirstName;
                    listStudentsSearch.Add(studentTemplate);
                });
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;   
            }

            return Json(new {listStudentsSearch,message,success });
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MakeStudent()
        {

            var db = SingletonDb.Instance();
            var user = new User();            
            user.UserMail = string.Empty;
            user.RoleID = 2;
            db.Users.Add(user);
            db.SaveChanges();
            var generateId = user.UserID;
            return RedirectToAction("NewStudent", new {id = generateId});
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

            return Json(new {success });
           
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
