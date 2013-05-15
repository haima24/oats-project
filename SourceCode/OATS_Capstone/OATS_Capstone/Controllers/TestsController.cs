using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OATS_Capstone.Controllers
{
    public class TestsController : Controller
    {
        //
        // GET: /Tests/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTest()
        {
            return View();
        }
        public ActionResult NewTest_ResponseTab()
        {
            return View();
        }
        public ActionResult NewTest_ScoreTab()
        {
            return View();
        }
        public ActionResult NewTest_SettingTab()
        {
            return View();
        }
        public ActionResult NewTest_InvitationTab()
        {
            return View();
        }
        public ActionResult TakeTest()
        {
            return View();
        }

    }
}
