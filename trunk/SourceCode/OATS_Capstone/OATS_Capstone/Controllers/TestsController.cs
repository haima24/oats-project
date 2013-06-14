﻿using Microsoft.AspNet.SignalR;
using OATS_Capstone.Hubs;
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

        public JsonResult RemoveUser(int testid, int userid)
        {
            var success = false;
            var meassage = Constants.DefaultProblemMessage;
            var generatedHTML = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                //var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                var inv = test.Invitations.FirstOrDefault(k => k.UserID == userid);
                test.Invitations.Remove(inv);
                db.Invitations.Remove(inv);
                var authen = AuthenticationSessionModel.Instance();
                var ownerid = authen.OwnerUserId;

                var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                if (user != null)
                {
                    if (user.Invitations.Count == 0)
                    {
                        var roleMap = db.UserRoleMappings.FirstOrDefault(i => i.OwnerDomainUserID == ownerid && i.ClientUserID == userid);
                        if (roleMap != null) { db.UserRoleMappings.Remove(roleMap); }
                    }
                }
                if (db.SaveChanges() >= 0)
                {
                    generatedHTML = this.RenderPartialViewToString("P_InvitationTab", test.Invitations);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                meassage = Constants.DefaultExceptionMessage;
            }
            return Json(new { generatedHTML, success, meassage });
        }

        public JsonResult ModalPopupUser(int testid, string role)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var popupHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var invitedUsers = test.Invitations.Select(i => i.User);
                    users = db.Users.ToList().Where(k => !invitedUsers.Contains(k)&&k.UserID!=authen.OwnerUserId).ToList();
                    
                    if (authen.OwnerUser != null)
                    {
                        users = users.Where(u =>
                        {
                            var roleMap = db.UserRoleMappings.FirstOrDefault(i => i.OwnerDomainUserID == authen.OwnerUserId && i.ClientUserID == u.UserID);
                            if (roleMap != null)
                            {
                                return roleMap.Role.RoleDescription == role;
                            }
                            else {
                                return true;
                            }
                        }).ToList();
                    }
                }
                ViewBag.Role = role;
                popupHtml = this.RenderPartialViewToString("P_Modal_Assign_Tests_For_Users", users);
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, popupHtml });
        }

        public JsonResult ModalRemovePopupUser(int testid, string role)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var popupHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var roleMapping = new List<UserRoleMapping>();
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                if (test != null)
                {
                    switch (role)
                    {
                        case "Student":
                            users = test.Invitations.ToStudents();
                            break;
                        case "Teacher":
                            users = test.Invitations.ToTeachers();
                            break;
                        default:
                            break;
                    }
                }
                ViewBag.Role = role;
                popupHtml = this.RenderPartialViewToString("P_Modal_Remove_Tests_For_User", users);
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, popupHtml });
        }

        public JsonResult ModalReinvitePopupUser(int testid, string role)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var popupHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var roleMapping = new List<UserRoleMapping>();
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                if (test != null)
                {
                    switch (role)
                    {
                        case "Student":
                            users = test.Invitations.ToStudents();
                            break;
                        case "Teacher":
                            users = test.Invitations.ToTeachers();
                            break;
                        default:
                            break;
                    }
                }
                ViewBag.Role = role;
                popupHtml = this.RenderPartialViewToString("P_Modal_Reinvite_Tests_For_Users", users);
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, popupHtml });
        }

        public JsonResult ReuseQuestionTemplate(int questionid)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var questionHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                questionHtml = this.RenderPartialViewToString("P_Question_Instance", question);
                success = true;
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, questionHtml });
        }
        public JsonResult ReuseSearchQuestionTemplate(int maxrows, string term)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var renderedHtmlList = new List<String>();
            var termLower = term.ToLower();
            try
            {
                var db = SingletonDb.Instance();
                var questions = db.Questions.ToList();
                var matchQuestions = questions.Where(delegate(Question question)
                {
                    return question.QuestionTitle.ToLower().Contains(termLower)
                        || question.Answers.Any(k => k.AnswerContent.ToLower().Contains(termLower));
                });
                var matches = matchQuestions.Take(maxrows).ToList();
                matches.ForEach(delegate(Question question)
                {
                    var html = this.RenderPartialViewToString("P_Reuse_Template_Question_Instance", question);
                    renderedHtmlList.Add(html);
                });
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return Json(new { success, message, renderedHtmlList });
        }
        public JsonResult TestsAssignUserSearch(int userid, string letter)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listTests = new List<SearchingTests>();
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                var testsInInvitation = user.Invitations.Select(i => i.Test);
                var tests = db.Tests.ToList();
                var filteredTest = tests.Where(i => !testsInInvitation.Contains(i));
                var finalResult = filteredTest.Where(i => i.TestTitle.ToLower().Contains(letter.ToLower()));
                foreach (var item in finalResult)
                {
                    var search = new SearchingTests();
                    search.TestTitle = item.TestTitle;
                    search.Id = item.TestID;
                    search.StartDate = item.StartDateTime;
                    search.EndDate = item.EndDateTime;
                    listTests.Add(search);
                }
                success = true;
                message = string.Empty;

            }

            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;


            }
            return Json(new { listTests, success, message });
        }
        public JsonResult TestsSearch()
        {

            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listTestsSearch = new List<SearchingTests>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var tests = authen.TestsInThisOwner;
                tests.ForEach(delegate(Test test)
                {
                    var testTemplate = new SearchingTests();
                    testTemplate.Id = test.TestID;
                    testTemplate.TestTitle = test.TestTitle;
                    testTemplate.StartDate = test.StartDateTime;
                    testTemplate.EndDate = test.EndDateTime;
                    listTestsSearch.Add(testTemplate);
                });
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { listTestsSearch, success, message });
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
            test.SettingConfigID = 1;
            test.IsActive = true;
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
        public JsonResult AddUserToInvitationTest(int testid, int count, List<int> userids, string role)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var ownerUser = AuthenticationSessionModel.Instance().OwnerUser;
                var userRole = db.Roles.FirstOrDefault(k => k.RoleDescription == role);
                if (count > 0)
                {
                    userids.ForEach(delegate(int id)
                    {
                        var user = db.Users.FirstOrDefault(k => k.UserID == id);
                        if (user != null)
                        {
                            var invitation = new Invitation();
                            invitation.User = user;
                            test.Invitations.Add(invitation);
                            if (userRole != null)
                            {
                                var roleMap = db.UserRoleMappings.FirstOrDefault(i => i.OwnerDomainUserID == ownerUser.UserID && i.ClientUserID == user.UserID);
                                if (roleMap == null)
                                {
                                    roleMap = new UserRoleMapping();
                                    roleMap.Role = userRole;
                                    roleMap.ClientUser = user;
                                    roleMap.OwnerDomainUserID = ownerUser.UserID;
                                    db.UserRoleMappings.Add(roleMap);
                                }
                            }


                        }
                    });
                }
                if (db.SaveChanges() >= 0)
                {
                    generatedHtml = this.RenderPartialViewToString("P_InvitationTab", test.Invitations);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { generatedHtml, success, message });
        }

        public JsonResult RemoveUserToInvitationTest(int testid, int count, List<int> userids)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var authen = AuthenticationSessionModel.Instance();
                if (test != null)
                {
                    if (count > 0)
                    {
                        userids.ForEach(delegate(int id)
                        {
                            var user = db.Users.FirstOrDefault(k => k.UserID == id);
                            if (user != null)
                            {
                                //var invitation = new Invitation();
                                //invitation.User = user;
                                //test.Invitations.Remove(invitation);

                                var inv = test.Invitations.FirstOrDefault(k => k.UserID == id);
                                test.Invitations.Remove(inv);
                                db.Invitations.Remove(inv);
                                var ownerid = authen.OwnerUserId;

                                if (user.Invitations.Count == 0) {
                                    var roleMap = db.UserRoleMappings.FirstOrDefault(i => i.OwnerDomainUserID == ownerid && i.ClientUserID == id);
                                    if (roleMap != null) { db.UserRoleMappings.Remove(roleMap); }
                                }
                            }
                        });
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        generatedHtml = this.RenderPartialViewToString("P_InvitationTab", test.Invitations);
                        success = true;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { generatedHtml, success, message });
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
        public ActionResult DoTest(int id)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == id);
            return View(test);
        }
        public JsonResult TestCalendarObjectResult()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var listTestCalendar = new List<TestCalendarObject>();
            try
            {
                var db = SingletonDb.Instance();
                var tests = db.Tests.ToList();


                tests.ForEach(delegate(Test test)
                {
                    var template = new TestCalendarObject();
                    template.id = test.TestID;
                    template.testTitle = test.TestTitle;
                    template.startDateTime = test.StartDateTime;
                    template.endDateTime = test.EndDateTime;
                    listTestCalendar.Add(template);
                });
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return Json(new { listTestCalendar, success, message });
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
        public JsonResult NewTest_ResponseTab(int testid)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
            return Json(new { tab = this.RenderPartialViewToString("P_ResponseTab",test) });
        }
        public JsonResult NewTest_ScoreTab()
        {
            return Json(new { tab = this.RenderPartialViewToString("P_ScoreTab") });
        }
        public JsonResult NewTest_SettingTab(int testid)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
            return Json(new { tab = this.RenderPartialViewToString("P_SettingTab", test) });
        }
        public JsonResult NewTest_InvitationTab(int testid)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
            var invitations = test.Invitations.ToList();
            return Json(new { tab = this.RenderPartialViewToString("P_InvitationTab", invitations) });
        }

        public JsonResult AddNewQuestion(int testid, string type, string questiontitle, List<Answer> answers, int serialorder, string labelorder, string textdescription)
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
                    if (textdescription != null) { question.TextDescription = textdescription; }
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

            return Json(new { success, message, questionHtml });
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
                if (test != null)
                {
                    listquestion.ForEach(delegate(QuestionItemTemplate item)
                    {
                        var type = item.QuestionItem.QuestionType.Type;
                        var realType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                        if (realType != null)
                        {
                            item.QuestionItem.QuestionType = realType;
                        }
                        test.Questions.Add(item.QuestionItem);
                    });

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = "Import questions complete.";
                        listquestion.ForEach(delegate(QuestionItemTemplate item)
                        {
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
            return Json(new { success, message, arraylist });
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
                    var maxSerial = question.Answers.Max(k => k.SerialOrder);
                    if (maxSerial.HasValue) { ans.SerialOrder = maxSerial.Value + 1; }
                    else { ans.SerialOrder = 0; }
                    question.Answers.Add(ans);
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

            return Json(new { success, questionHtml, message });
        }

        public JsonResult ResortQuestions(int count, List<Question> questions)
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
                    if (db.SaveChanges() >= 0)
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

            return Json(new { success, message });
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
            return Json(new { success, message });
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
            return Json(new { success, message });
        }
        public JsonResult UpdateAnswer(List<Answer> answers)
        {
            var success = false;
            var db = SingletonDb.Instance();
            var message = Constants.DefaultProblemMessage;
            try
            {
                answers.ForEach(delegate(Answer ans)
                {
                    var ansDb = db.Answers.FirstOrDefault(i => i.AnswerID == ans.AnswerID);
                    if (ansDb != null)
                    {
                        ansDb.AnswerContent = ans.AnswerContent;
                        ansDb.IsRight = ans.IsRight;
                        ansDb.Score = ans.Score;
                        ansDb.SerialOrder = ans.SerialOrder;
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                    }
                });
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message });
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
                    if (db.SaveChanges() >= 0)
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
            return Json(new { success, message });
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
                    if (db.SaveChanges() >= 0)
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
            return Json(new { success, message });
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
                    if (db.SaveChanges() >= 0)
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
            return Json(new { success, message });
        }
        public JsonResult UpdateQuestionType(int questionid, string type)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (test != null)
                {
                    var questionType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                    if (questionType != null)
                    {
                        test.QuestionType = questionType;
                        if (db.SaveChanges() >= 0)
                        {
                            success = true;
                        }
                    }
                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message });
        }
        public JsonResult UpdateStartEnd(int testid, DateTime start, DateTime end)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    test.StartDateTime = start;
                    test.EndDateTime = end;
                    if (db.SaveChanges() >= 0)
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
            return Json(new { success, message });
        }
        public JsonResult UpdateSettings(int testid, String settingKey, bool isactive)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var settingConfig = test.SettingConfig;
                if (settingConfig.SettingConfigID == 1)//1 is default
                {
                    var settings = settingConfig.SettingConfigDetails.ToList();
                    //clone default setting
                    var newSettingConfig = new SettingConfig();
                    settings.ForEach(delegate(SettingConfigDetail settingDetail)
                    {
                        var newSettingDetail = new SettingConfigDetail();
                        newSettingDetail.IsActive = settingDetail.IsActive;
                        newSettingDetail.NumberValue = settingDetail.NumberValue;
                        newSettingDetail.SettingType = settingDetail.SettingType;
                        newSettingDetail.TextValue = settingDetail.TextValue;
                        newSettingConfig.SettingConfigDetails.Add(newSettingDetail);
                    });
                    newSettingConfig.Description = "SettingForTest_" + test.TestID;
                    test.SettingConfig = newSettingConfig;
                }
                var currentSetting = test.SettingConfig;
                var sets = currentSetting.SettingConfigDetails.ToList();
                sets.ForEach(delegate(SettingConfigDetail detail)
                {
                    if (detail.SettingType.SettingTypeKey == settingKey)
                    {
                        detail.IsActive = isactive;
                    }
                });
                if (db.SaveChanges() >= 0)
                {
                    success = true;
                    message = String.Empty;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message });
        }
        public JsonResult DeActiveTest(int testid)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(k => k.TestID == testid);
                if (test != null)
                {
                    test.IsActive = false;
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = "Successful de-active this test";
                        var owner = AuthenticationSessionModel.Instance().OwnerUser;
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_deactivetest(test.TestID, owner.UserMail);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message });
        }
        public JsonResult EnableTest(int testid)
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(k => k.TestID == testid);
                if (test != null)
                {
                    test.IsActive = true;
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = "Successful active this test";
                        generatedHtml = this.RenderPartialViewToString("P_Inner_Container", test);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return Json(new { success, message, generatedHtml });
        }
    }
}
