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

    }
}
