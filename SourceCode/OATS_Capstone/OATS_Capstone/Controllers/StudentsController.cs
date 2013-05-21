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

        public ActionResult NewStudent()
        {
            return View();
        }


        public JsonResult NewStudentByEmail(string email)
        {
            var error = string.Empty;
            var iserror = false;
            var db = SingletonDb.Instance();
            var emailsInDb = db.Users.Select(i => i.UserMail);
            if (!emailsInDb.Contains(email.Trim())) //not exist in db
            {
                var newUser = new User();
                newUser.UserMail = email;
                
                //She or he is a Student 
                newUser.RoleID = 2;
                db.Users.Add(newUser);
                var affectedRow = db.SaveChanges();
                if (affectedRow <= 0)
                {
                    //error
                    error = "There was an unhandle problem in server.";
                    iserror = true;
                }
            }
            var result = new { 
                iserror,
                error
            };
            return Json(result);
        }

    }
}
