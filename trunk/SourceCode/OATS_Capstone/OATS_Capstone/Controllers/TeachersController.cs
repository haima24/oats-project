using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult NewTeacher()
        {
            return View();
        }

    }
}
