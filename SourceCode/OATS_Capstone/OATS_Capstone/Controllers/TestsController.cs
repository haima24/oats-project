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

        public JsonResult SaveNewTest(Test test)
        {
            var success = false;
            var db = SingletonDb.Instance();

            try
            {
                var realTest = db.Tests.FirstOrDefault(i => i.TestID == test.TestID);
                if (realTest != null)
                {
                    realTest.TestTitle = test.TestTitle;
                    var questions = test.Questions.ToList();
                    var realQuestions = realTest.Questions;
                    questions.ForEach(delegate(Question question)
                    {
                        if (question.QuestionID != 0)
                        {
                            var ques = realQuestions.FirstOrDefault(i => i.QuestionID == question.QuestionID);
                            ques.QuestionTitle = question.QuestionTitle;
                            var answers = question.Answers.ToList();
                            var realAnswers = ques.Answers;
                            answers.ForEach(delegate(Answer answer)
                            {
                                if (answer.AnswerID != 0)
                                {
                                    var ans = realAnswers.FirstOrDefault(k => k.AnswerID == answer.AnswerID);
                                    ans.AnswerContent = answer.AnswerContent;
                                    ans.IsRight = answer.IsRight;
                                }
                                else
                                {
                                    var ans = new Answer();
                                    ans.AnswerContent = answer.AnswerContent;
                                    ans.IsRight = answer.IsRight;
                                    realAnswers.Add(ans);
                                }
                            });
                        }
                        else
                        {
                            var ques = new Question();
                            ques.QuestionTitle = question.QuestionTitle;
                            var answers = question.Answers.ToList();
                            answers.ForEach(delegate(Answer answer)
                            {
                                var ans = new Answer();
                                ans.AnswerContent = answer.AnswerContent;
                                ans.IsRight = answer.IsRight;
                                ques.Answers.Add(ans);
                            });
                            realQuestions.Add(ques);
                        }
                    });



                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            
            return Json(new { success });
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

    }
}
