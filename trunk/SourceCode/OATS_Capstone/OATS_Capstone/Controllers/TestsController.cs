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

        public ActionResult MakeTest()
        {
            var db = SingletonDb.Instance();
            var test = new Test();
            test.TestTitle = String.Empty;
            test.CreatedUserID = 1;//must be fix, this is for test purpose
            test.CreatedDateTime = DateTime.Now;
            test.StartDateTime = DateTime.Now;
            db.Tests.Add(test);
            db.SaveChanges();
            var generatedId = test.TestID;
            return RedirectToAction("NewTest", new { id = generatedId });
        }
        public ActionResult NewTest(int id)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == id);
            return View(test);
        }
        public JsonResult QuestionTypes()
        {
            var obj = new
            {
                radio = this.RenderPartialViewToString("P_Type_Radio_Template"),
                multiple = this.RenderPartialViewToString("P_Type_Multiple_Template"),
                essay = this.RenderPartialViewToString("P_Type_Essay_Template"),
                shortanswer = this.RenderPartialViewToString("P_Type_ShortAnswer_Template"),
                text = this.RenderPartialViewToString("P_Type_Text_Template"),
                image = this.RenderPartialViewToString("P_Type_Image_Template"),
                imagepreview = this.RenderPartialViewToString("P_RenderPreviewImage"),
                empty = this.RenderPartialViewToString("P_Type_Empty_Template")
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

        public JsonResult AddNewQuestion(int testid, string type, string questiontitle, List<Answer> answers, int serialorder, string labelorder)
        {
            var db = SingletonDb.Instance();
            var success = false;
            var questionHtml = String.Empty;
            try
            {
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var qType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                    var question = new Question();
                    question.QuestionTitle = questiontitle;
                    question.SerialOrder = serialorder;
                    question.LabelOrder = labelorder;
                    question.QuestionType = qType;
                    question.Answers = answers;
                    test.Questions.Add(question);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        questionHtml = this.RenderPartialViewToString("P_Question_Instance", question);
                    }
                }
            }
            catch (Exception)
            {

                success = false;
            }

            return Json(new { success, questionHtml });
        }
        public JsonResult AddAnswer(int questionid) {
            var db = SingletonDb.Instance();
            var questionHtml=String.Empty;
            var success=false;
            var ans = new Answer();
            ans.AnswerContent = String.Empty;
            try
            {
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    question.Answers.Add(ans);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        questionHtml = this.RenderPartialViewToString("P_Question_Instance", question);
                    }
                }
            }
            catch (Exception ex)
            {

                success = false;
            }
            
            return Json(new { success, questionHtml });
        }
        public JsonResult ResortQuestions(List<Question> questions)
        {
            var success = false;
            var db=SingletonDb.Instance();
            var questionsDb=db.Questions;
            try
            {
                questions.ForEach(delegate(Question question) {
                    var ques = questionsDb.FirstOrDefault(i => i.QuestionID == question.QuestionID);
                    ques.LabelOrder = question.LabelOrder;
                    ques.SerialOrder = question.SerialOrder;
                });
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }
            }
            catch (Exception)
            {

               success = false;
            }

            return Json(new { success });
        }
    }
}
