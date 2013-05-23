using OATS_Capstone.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (HttpPostedFileBase file in files)
            {
                string filePath = Path.Combine(Server.MapPath("~/Resource/Images"), file.FileName);
                System.IO.File.WriteAllBytes(filePath, this.ReadData(file.InputStream));
            }
            return this.Json("All files have been successfully stored.");
        }

        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        public JsonResult TestsSearch()
        {
            var db = SingletonDb.Instance();
            var tests = db.Tests.ToList();
            var listTestsSearch = new List<SearchingTests>();
            tests.ForEach(delegate(Test test)
            {
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
            var obj = new
            {
                radio = this.RenderPartialViewToString("P_Type_Radio"),
                multiple=this.RenderPartialViewToString("P_Type_Multiple"),
                essay=this.RenderPartialViewToString("P_Type_Essay"),
                shortanswer=this.RenderPartialViewToString("P_Type_ShortAnswer"),
                text=this.RenderPartialViewToString("P_Type_Text"),
                image=this.RenderPartialViewToString("P_Type_Image"),
                imagepreview=this.RenderPartialViewToString("P_RenderPreviewImage")
            };
            return Json(obj);
        }
        public JsonResult NewTest_ContentTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_ContentTab") });
        }
        public JsonResult TestCalendarObjectResult()
        {
            var db = SingletonDb.Instance();
            var tests = db.Tests.ToList();
            var listTestCalendar = new List<TestCalendarObject>();

            tests.ForEach(delegate(Test test)
            {
                var template = new TestCalendarObject();
                template.id = test.TestID;
                template.testTitle = test.TestTitle;
                template.startDateTime = test.StartDateTime;
                template.endDateTime = test.EndDateTime;
                listTestCalendar.Add(template);
            });
            return Json(listTestCalendar);
        }
        public JsonResult Index_CalendarTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_CalendarTab") });
        }
        public JsonResult Index_TestListTab()
        {
            var db = SingletonDb.Instance();
            return Json(new { tab = this.RenderPartialViewToString("P_TestListTab", db.Tests) });
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
            var db = SingletonDb.Instance();
            var invitations = db.Invitations;
            return Json(new { tab = this.RenderPartialViewToString("P_InvitationTab", invitations) });
        }
        public ActionResult TakeTest()
        {
            return View();
        }

    }
}
