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
            var db = SingletonDb.Instance();
            var students = db.Users.Where(i => i.Role.RoleID == 2).ToList();
            var listStudentsSearch = new List<SearchingStudents>();
            students.ForEach(delegate(User student)
            {
                var studentTemplate = new SearchingStudents();
                studentTemplate.UserID = student.UserID;
                studentTemplate.RoleID = student.RoleID;
                studentTemplate.LastName = student.LastName;
                studentTemplate.FirstName = student.FirstName;
                listStudentsSearch.Add(studentTemplate);
            });
            return Json(listStudentsSearch, JsonRequestBehavior.DenyGet);
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
            var invitation = new Invitation();
            invitation.UserID = userId;
            invitation.TestID = testId;
            if (userId !=0 && testId != 0)
            {
                db.Invitations.Add(invitation);
                db.SaveChanges();
                success = true;
            }
            return Json(success);

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

     

    }
}
