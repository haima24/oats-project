using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TugberkUg.MVC.Helpers;

namespace OATS_Capstone.Controllers
{
    public class TestsController : Controller
    {
        //
        // GET: /Tests/

        public JsonResult TestsSearch()
        {
            var db = SingletonDb.Instance();
            var tests = db.Tests.ToList();
            var listTestsSearch = new List<SearchingTests>();
            tests.ForEach(delegate(Test test) {
                var testTemplate = new SearchingTests();
                testTemplate.Id = test.TestID;
                testTemplate.TestTitle = test.TestTitle; 
                testTemplate.StartDate = test.StartDateTime;
                testTemplate.EndDate = test.EndDateTime;
                listTestsSearch.Add(testTemplate);
            });
            return Json(listTestsSearch, JsonRequestBehavior.DenyGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewTest()
        {
            return View();
        }
        public JsonResult QuestionTypes()
        {
            var obj = new { 
                radio = this.RenderPartialViewToString("P_Type_Radio"),
                multiple=this.RenderPartialViewToString("P_Type_Multiple"),
                essay=this.RenderPartialViewToString("P_Type_Essay"),
                shortanswer=this.RenderPartialViewToString("P_Type_ShortAnswer"),
                text=this.RenderPartialViewToString("P_Type_Text"),
                image=this.RenderPartialViewToString("P_Type_Image")
            };
            return Json(obj);
        }
        public JsonResult NewTest_ContentTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_ContentTab") });
        }
        public JsonResult Index_CalendarTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_CalendarTab") });
        }
        public JsonResult Index_TestListTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_TestListTab") });
        }
        public JsonResult NewTest_ResponseTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_ResponseTab") });
        }
        public JsonResult NewTest_ScoreTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_ScoreTab") });
        }
        public JsonResult NewTest_SettingTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_SettingTab") });
        }
        public JsonResult NewTest_InvitationTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_InvitationTab") });
        }
        public ActionResult TakeTest()
        {
            return View();
        }

    }
}
