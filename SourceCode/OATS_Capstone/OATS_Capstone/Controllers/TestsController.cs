using OATS_Capstone.Models;
using System;
using System.Collections;
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
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, int questionid)
        {
            var message = String.Empty;
            foreach (HttpPostedFileBase file in files)
            {
                string filePath = Path.Combine(Server.MapPath("~/Resource/Images"), file.FileName);
                System.IO.File.WriteAllBytes(filePath, this.ReadData(file.InputStream));
                //write path to db
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    question.ImageUrl = "~/Resource/Images/" + file.FileName;
                    if (db.SaveChanges() > 0)
                    {
                        message = "All files have been successfully stored.";
                    }
                }
                else
                {
                    message = "Problem in storing images.";
                }
            }
            return this.Json(message);
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

        public JsonResult TestsAssignStudentSearch(int userid)
        {
             
            var db = SingletonDb.Instance();
            var user = db.Users.FirstOrDefault(p=>p.UserID==userid);
            var testsInInivation=user.Invitations.Select(i=>i.Test);

            var tests = db.Tests.ToList();
            var listTestsSearch = new List<SearchingTests>();
            tests.ForEach(delegate(Test test)
            {
                if(!testsInInivation.Contains(test)){
                var testTemplate = new SearchingTests();
                testTemplate.Id = test.TestID;
                testTemplate.TestTitle = test.TestTitle;
                testTemplate.StartDate = test.StartDateTime;
                testTemplate.EndDate = test.EndDateTime;
                listTestsSearch.Add(testTemplate);
                }
            });
            return Json(listTestsSearch, JsonRequestBehavior.DenyGet);
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

        public JsonResult AddUserToInivtationTest(int testid, List<int> userids)
        {
            //find test by id

            //find users by id

            //get invitation list of test

            //add users to invition list

            return Json("done");
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
        public JsonResult NewTest_ContentTab(int testid)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
            return Json(new { tab = this.RenderPartialViewToString("P_ContentTab", test) });
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
            var invitations = db.Invitations.ToList();
            var users = db.Users.ToList();
            var master = new InvitationMasterModel() { InvitationList = invitations, UserList = users };
            return Json(new { tab = this.RenderPartialViewToString("P_InvitationTab", master) });
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
            var message = Constants.DefaultProblemMessage;
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
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { success,message, questionHtml });
        }
        public JsonResult AddListQuestion(int testid, List<QuestionItemTemplate> listquestion)
        {
            var db = SingletonDb.Instance();
            var success = false;
            var arraylist = new ArrayList();
            var message = Constants.DefaultProblemMessage;
            try
            {
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null) {
                    listquestion.ForEach(delegate(QuestionItemTemplate item) {
                        var type = item.QuestionItem.QuestionType.Type;
                        var realType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                        if (realType != null) {
                            item.QuestionItem.QuestionType = realType;
                        }
                        test.Questions.Add(item.QuestionItem);
                    });

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = "Import questions complete.";
                        listquestion.ForEach(delegate(QuestionItemTemplate item) {
                            arraylist.Add(new { ClientID = item.ClientID, QuestionHtml = this.RenderPartialViewToString("P_Question_Instance", item.QuestionItem) });
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message, arraylist });
        }
        public JsonResult AddAnswer(int questionid)
        {
            var db = SingletonDb.Instance();
            var questionHtml = String.Empty;
            var success = false;
            var ans = new Answer();
            ans.AnswerContent = String.Empty;
            var message = Constants.DefaultProblemMessage;
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
        public JsonResult UpdateAnswer(int answerid, string answerContent, bool isright, string type)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var message = Constants.DefaultProblemMessage;
            try
            {
                var ans = db.Answers.FirstOrDefault(i => i.AnswerID == answerid);
                if (ans != null)
                {
                    ans.AnswerContent = answerContent;
                    if (type == "Radio")
                    {
                        var questionid = ans.QuestionID;
                        var ansInQues = db.Answers.Where(k => k.QuestionID == questionid).ToList();
                        if (isright)
                        { 
                        ansInQues.ForEach(h => h.IsRight = false);
                        }
                    }
                    ans.IsRight = isright;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message });
        }
        public JsonResult ResortQuestions(int count,List<Question> questions)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var questionsDb = db.Questions;
            var message = Constants.DefaultProblemMessage;
            try
            {
                if (count == 0)
                {
                    success = true;
                }
                else
                {
                    questions.ForEach(delegate(Question question)
                    {
                        var ques = questionsDb.FirstOrDefault(i => i.QuestionID == question.QuestionID);
                        ques.LabelOrder = question.LabelOrder;
                        ques.SerialOrder = question.SerialOrder;
                    });
                    if (db.SaveChanges() >=0)
                    {
                        success = true;
                    }
                }
                
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { success,message });
        }
        public JsonResult DeleteQuestion(int questionid)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var message = Constants.DefaultProblemMessage;
            try
            {
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    if (question.UserInTestDetails.Count > 0)
                    {
                        success = false;
                        message = "This Question Already In Use.";
                    }
                    else
                    {
                        question.Answers.ToList().ForEach(k => db.Answers.Remove(k));
                        var tags = question.Tags.ToList();
                        tags.ForEach(k => question.Tags.Remove(k));
                        db.Questions.Remove(question);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            message = Constants.DefaultSuccessMessage;
                        }
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message });
        }
        public JsonResult DeleteAnswer(int answerid)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var message = Constants.DefaultProblemMessage;
            try
            {
                var ans = db.Answers.FirstOrDefault(i => i.AnswerID == answerid);
                if (ans != null)
                {
                    db.Answers.Remove(ans);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message });
        }
        public JsonResult UpdateQuestionTitle(int questionid, string newtext)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    question.QuestionTitle = newtext;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success ,message});
        }
        public JsonResult UpdateQuestionTextDescription(int questionid, string text)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    question.TextDescription = text;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message});
        }
        public JsonResult UpdateTestTitle(int testid, string text)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    test.TestTitle = text;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success,message });
        }
    }
}
