using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OATS_Capstone.Models;

namespace OATS_Capstone.Controllers
{
    public class TeachersController : Controller
    {
        //
        // GET: /Teachers/

        public JsonResult TeachersSearch()
        {
            var db = SingletonDb.Instance();
            var teachers = db.Users.Where(i => i.Role.RoleID == 3).ToList();
            var listTeachersSearch = new List<SearchingTeachers>();
            teachers.ForEach(delegate(User teacher)
            {
                var teacherTemplate = new SearchingTeachers();
                teacherTemplate.UserID = teacher.UserID;
                teacherTemplate.RoleID = teacher.RoleID;
                teacherTemplate.LastName = teacher.LastName;
                teacherTemplate.FirstName = teacher.FirstName;
                listTeachersSearch.Add(teacherTemplate);
            });
            return Json(listTeachersSearch, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTeacher()
        {
            return View();
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


    }
}
