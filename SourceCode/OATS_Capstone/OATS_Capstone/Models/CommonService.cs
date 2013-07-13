using Microsoft.AspNet.SignalR;
using OATS_Capstone.Hubs;
using OATS_Capstone.Mailers;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using TugberkUg.MVC.Helpers;

namespace OATS_Capstone.Models
{
    public delegate String OnRenderPartialViewHandler(object model);
    public delegate String OnRenderPartialViewHandlerWithParameter(object model, object parameter);
    public class CommonService
    {
        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        public event OnRenderPartialViewHandler OnRenderPartialViewToString;
        public event OnRenderPartialViewHandlerWithParameter OnRenderPartialViewToStringWithParameter;

        bool _success = false;
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        string _message = Constants.DefaultProblemMessage;
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        string _generatedHtml = string.Empty;
        public string generatedHtml
        {
            get { return _generatedHtml; }
            set { _generatedHtml = value; }
        }

        List<Object> _resultlist = new List<object>();
        public List<Object> resultlist
        {
            get { return _resultlist; }
            set { _resultlist = value; }
        }

        ArrayList _arraylist = new ArrayList();
        public ArrayList arraylist
        {
            get { return _arraylist; }
            set { _arraylist = value; }
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

        public void UploadFiles(IEnumerable<HttpPostedFileBase> files, int questionid)
        {
            message = Constants.DefaultProblemMessage;
            success = false;
            try
            {
                foreach (HttpPostedFileBase file in files)
                {

                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Resource/Images"), file.FileName);
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
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void RemoveUser(int testid, int userid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                //var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                var inv = test.Invitations.FirstOrDefault(k => k.UserID == userid);
                test.Invitations.Remove(inv);
                db.Invitations.Remove(inv);
                if (db.SaveChanges() >= 0)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderPartialViewToString(test.Invitations);
                    }

                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void ModalPopupUser(int testid, string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var invitedUsers = test.Invitations.Select(i => i.User.UserID);
                    users = db.Users.Where(i => !invitedUsers.Contains(i.UserID) && !invitedUsers.Contains(test.CreatedUserID)).Where(k => k.UserID != authen.UserId).ToList();
                }

                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(users);
                }

            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void ModalRemovePopupUser(int testid, string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                if (test != null)
                {
                    switch (role)
                    {
                        case "Student":
                            users = test.Invitations.Where(k => k.Role.RoleDescription == "Student").Select(i => i.User).ToList();
                            break;
                        case "Teacher":
                            users = test.Invitations.Where(k => k.Role.RoleDescription == "Teacher").Select(i => i.User).ToList();
                            break;
                        default:
                            break;
                    }
                }
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(users);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void ModalReinvitePopupUser(int testid, string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                //filter users
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();
                if (test != null)
                {
                    switch (role)
                    {
                        case "Student":
                            users = test.Invitations.Where(k => k.Role.RoleDescription == "Student").Select(i => i.User).ToList();
                            break;
                        case "Teacher":
                            users = test.Invitations.Where(k => k.Role.RoleDescription == "Teacher").Select(i => i.User).ToList();
                            break;
                        default:
                            break;
                    }
                }
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(users);
                }

            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void CloneQuestion(int targetTestID, int questionid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var oldQuestion = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                var test = db.Tests.FirstOrDefault(i => i.TestID == targetTestID);
                if (oldQuestion != null)
                {
                    if (test != null)
                    {
                        var question = new Question();
                        question.ImageUrl = oldQuestion.ImageUrl;
                        question.NoneChoiceScore = oldQuestion.NoneChoiceScore;
                        question.QuestionTitle = oldQuestion.QuestionTitle;
                        question.QuestionType = oldQuestion.QuestionType;
                        var tagsInQuestion = oldQuestion.TagInQuestions.ToList();
                        question.TagInQuestions = new List<TagInQuestion>();
                        tagsInQuestion.ForEach(k =>
                        {
                            var tagInquestion = new TagInQuestion();
                            tagInquestion.Tag = k.Tag;
                            tagInquestion.SerialOrder = k.SerialOrder;
                            question.TagInQuestions.Add(tagInquestion);
                        });
                        question.TextDescription = oldQuestion.TextDescription;
                        var answers = oldQuestion.Answers.ToList();
                        question.Answers = new List<Answer>();
                        answers.ForEach(k =>
                        {
                            var answer = new Answer();
                            answer.AnswerContent = k.AnswerContent;
                            answer.IsRight = k.IsRight;
                            answer.Score = k.Score;
                            answer.SerialOrder = k.SerialOrder;
                            question.Answers.Add(answer);
                        });
                        test.Questions.Add(question);
                        if (db.SaveChanges() > 0)
                        {
                            if (OnRenderPartialViewToString != null)
                            {
                                success = true;
                                generatedHtml = OnRenderPartialViewToString.Invoke(question);
                            }
                        }
                    }
                }
                else
                {
                    success = false;
                    message = "This question is no longer exist.";
                }
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void ReuseSearchQuestionTemplate(string term, List<int> tagids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();

            try
            {
                var db = SingletonDb.Instance();
                var questions = db.Questions.ToList();
                if (tagids != null)
                {
                    questions = questions.Where(i =>
                    {
                        var tags = i.Test.TagInTests.Select(k => k.TagID);
                        return tags.Any(t => tagids.Contains(t));
                    }).ToList();
                }
                var matches = new List<Question>();
                if (!string.IsNullOrEmpty(term))
                {
                    var termLower = term.Trim().ToLower();
                    var matchQuestions = questions.Where(delegate(Question question)
                    {
                        return question.QuestionTitle.ToLower().Contains(termLower)
                            || question.Answers.Any(k => k.AnswerContent.ToLower().Contains(termLower));
                    });
                    matches = matchQuestions.ToList();
                }
                success = true;
                matches.ForEach(delegate(Question question)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        var html = OnRenderPartialViewToString.Invoke(question);
                        resultlist.Add(html);
                    }

                });

            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void TestsAssignUserSearch(int userid, string letter)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
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
                    var dateDes = item.StartDateTime.ToDateDefaultFormat();
                    if (item.EndDateTime.HasValue)
                    {
                        dateDes += " - " + item.EndDateTime.ToDateDefaultFormat();
                    }
                    search.DateDescription = dateDes;
                    resultlist.Add(search);
                }
                success = true;
                message = string.Empty;
            }

            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void TestsSearch(string term, List<int> tagids)
        {

            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var authenUserId = authen.UserId;
                if (!string.IsNullOrEmpty(term))
                {
                    if (tagids == null) { tagids = new List<int>(); }
                    var tags = db.Tags.Where(i => tagids.Contains(i.TagID));
                    var lower = term.ToLower();
                    var tests = db.Tests.ToList();
                    if (tagids.Count > 0)
                    {
                        var rawTests = tags.SelectMany(i => i.TagInTests.Select(k => k.Test));
                        var groupTests = from t in rawTests
                                         group t by t into GroupTests
                                         select GroupTests.Key;
                        tests = groupTests.ToList();
                    }
                    tests.ForEach(delegate(Test test)
                    {
                        if (test.TestTitle != null)
                        {
                            if (test.TestTitle.ToLower().Contains(lower))
                            {
                                var testTemplate = new SearchingTests();
                                testTemplate.Id = test.TestID;
                                testTemplate.TestTitle = test.TestTitle;
                                var dateDes = test.StartDateTime.ToDateDefaultFormat();
                                if (test.EndDateTime.HasValue)
                                {
                                    dateDes += " - " + test.EndDateTime.ToDateDefaultFormat();
                                }
                                testTemplate.DateDescription = dateDes;
                                testTemplate.IsCurrentUserOwnTest = test.CreatedUserID == authenUserId;
                                testTemplate.Introduction = test.Introduction;
                                resultlist.Add(testTemplate);
                            }
                        }
                    });
                }
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void TestsSearchTag(string term)
        {

            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                if (!string.IsNullOrEmpty(term))
                {
                    var lower = term.ToLower();
                    var tags = db.Tags.ToList();
                    tags.ForEach(delegate(Tag tag)
                    {
                        if (tag.TagName != null)
                        {
                            if (tag.TagName.ToLower().Contains(lower))
                            {
                                var result = new { TagID = tag.TagID, TagName = tag.TagName };
                                resultlist.Add(result);
                            }
                        }
                    });
                }
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void UsersSearch(string term)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                if (!string.IsNullOrEmpty(term))
                {
                    var lower = term.ToLower();
                    var users = db.Users.ToList();
                    users.ForEach(delegate(User user)
                    {
                        var first = false;
                        var last = false;
                        var mail = false;
                        if (user.FirstName != null) { if (user.FirstName.ToLower().Contains(lower)) { first = true; } }
                        if (user.LastName != null) { if (user.LastName.ToLower().Contains(lower)) { last = true; } }
                        if (user.UserMail != null) { if (user.UserMail.ToLower().Contains(lower)) { mail = true; } }
                        if (first || last || mail)
                        {
                            var userTemplate = new SearchingUsers();
                            userTemplate.UserID = user.UserID;
                            userTemplate.LastName = user.LastName;
                            userTemplate.FirstName = user.FirstName;
                            resultlist.Add(userTemplate);
                        }
                    });
                }
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }

        public int MakeTest()
        {
            var authen = AuthenticationSessionModel.Instance();
            var generatedId = 0;
            if (authen.UserId != 0)
            {
                var db = SingletonDb.Instance();
                var test = new Test();
                test.TestTitle = String.Empty;
                test.CreatedUserID = authen.UserId;//must be fix, this is for test purpose
                test.CreatedDateTime = DateTime.Now;
                test.StartDateTime = DateTime.Now;
                test.SettingConfigID = 1;
                test.IsActive = true;
                db.Tests.Add(test);
                db.SaveChanges();
                generatedId = test.TestID;
            }
            return generatedId;
        }

        public void AddUserToInvitationTest(int testid, int count, List<int> userids, string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var userRole = db.Roles.FirstOrDefault(k => k.RoleDescription == role);
                if (count > 0)
                {
                    userids.ForEach(delegate(int id)
                    {
                        var user = db.Users.FirstOrDefault(k => k.UserID == id);
                        if (user != null)
                        {
                            var invitation = new Invitation();
                            var key = GenerateInvitationAccessToken(test.TestID, id);
                            invitation.AccessToken = key;
                            invitation.User = user;
                            invitation.Role = userRole;
                            invitation.InvitationDateTime = DateTime.Now;
                            test.Invitations.Add(invitation);
                        }
                    });
                }
                if (db.SaveChanges() >= 0)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderPartialViewToString.Invoke(test.Invitations);
                    }
                    //send mail
                    //var invitations = test.Invitations.ToList();
                    //UserMailer.InviteUsers(invitations);
                }
            }
            catch (SmtpException)
            {
                success = false;
                message = "Invitation has been save, but server could not send invitation emails due to internet connection problem, please use re-invite method later.";
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }


        //reinvite
        public void ReinviteUserToInvitationTest(int testid, int count, List<int> userids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var authen = AuthenticationSessionModel.Instance();
                var invitations = new List<Invitation>();
                if (test != null)
                {
                    if (count > 0)
                    {
                        userids.ForEach(delegate(int id)
                        {
                            var invitation = db.Invitations.FirstOrDefault(i => i.TestID == testid && i.UserID == id);
                            if (invitation != null)
                            {
                                invitations.Add(invitation);
                            }
                        });
                        IUserMailer mailer = new UserMailer();
                        mailer.ReInviteUsers(invitations);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void RemoveUserToInvitationTest(int testid, int count, List<int> userids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
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
                                var inv = test.Invitations.FirstOrDefault(k => k.UserID == id);
                                test.Invitations.Remove(inv);
                                db.Invitations.Remove(inv);
                            }
                        });
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(test.Invitations);
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void NewTest_ContentTab(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(test);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }

        public void TestCalendarObjectResult()
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                //var tests = db.Tests.ToList();
                var authen = AuthenticationSessionModel.Instance();
                var user = authen.User;
                var invitedTests = user.Invitations.Select(i => i.Test).ToList();
                var tests = user.Tests.ToList().Concat(invitedTests).ToList();


                tests.ForEach(delegate(Test test)
                {
                    var template = new TestCalendarObject();
                    template.id = test.TestID;
                    template.testTitle = test.TestTitle;
                    template.startDateTime = test.StartDateTime;
                    template.endDateTime = test.EndDateTime;
                    resultlist.Add(template);
                });
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

        }

        public void NewTest_ResponseTab(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    //var checkIds = new List<int>();
                    //var responseTest = new ResponseTest(test, checkIds);
                    var responseTest = new ResponseTest(test);
                    generatedHtml = OnRenderPartialViewToString.Invoke(responseTest);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }

        public void NewTest_ScoreTab(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    //var checkIds = new List<int>();
                    //var scoreTest = new ScoreTest(test, checkIds);
                    var scoreTest = new ScoreTest(test);
                    generatedHtml = OnRenderPartialViewToString.Invoke(scoreTest);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }


        }

        public void NewTest_FeedBackTab(int testid, string feedbacktab)
        {
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {

                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        message = Constants.DefaultSuccessMessage;
                        AbsFeedBackUser feedbacksLogic = null;
                        switch (feedbacktab)
                        {
                            case "student":
                                feedbacksLogic = new FeedBackStudent(test);
                                break;
                            case "teacher":
                                feedbacksLogic = new FeedBackTeacher(test);
                                break;
                            default:
                                feedbacksLogic = new FeedBackStudent(test);
                                break;
                        }
                        generatedHtml = OnRenderPartialViewToString.Invoke(feedbacksLogic);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
                generatedHtml = String.Empty;
            }
        }

        public void NewTest_SettingTab(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(test);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void NewTest_InvitationTab(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var invitations = test.Invitations.ToList();
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(test.Invitations);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }

        public void AddNewQuestion(int testid, string type)
        {
            success = false;
            generatedHtml = String.Empty;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var dbType = db.QuestionTypes.FirstOrDefault(i => i.Type == type);
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (dbType != null && test != null)
                {
                    var question = new Question();
                    question.QuestionTitle = string.Empty;
                    question.TextDescription = string.Empty;
                    question.QuestionType = dbType;
                    if (dbType.Type == "Radio" || dbType.Type == "Multiple")
                    {
                        var ans1 = new Answer() { AnswerContent = string.Empty };
                        var ans2 = new Answer() { AnswerContent = string.Empty };
                        question.Answers.Add(ans1);
                        question.Answers.Add(ans2);
                    }
                    test.Questions.Add(question);
                    if (db.SaveChanges() > 0)
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(question);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }

        public void AddListQuestion(int testid, List<QuestionItemTemplate> listquestion)
        {
            success = false;
            arraylist = new ArrayList();
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    listquestion.ForEach(delegate(QuestionItemTemplate item)
                    {

                        var qItem = item.QuestionItem;
                        if (qItem != null)
                        {
                            var type = qItem.QuestionType.Type;
                            var realType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                            if (realType != null)
                            {
                                qItem.QuestionType = realType;
                            }
                            qItem.QuestionTitle = qItem.QuestionTitle ?? string.Empty;
                            qItem.TextDescription = qItem.TextDescription ?? string.Empty;
                            test.Questions.Add(qItem);
                        }
                    });

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = "Import questions complete.";
                        listquestion.ForEach(delegate(QuestionItemTemplate item)
                        {
                            var html = String.Empty;
                            if (OnRenderPartialViewToString != null)
                            {
                                html = OnRenderPartialViewToString.Invoke(item.QuestionItem);
                            }

                            arraylist.Add(new { ClientID = item.ClientID, QuestionHtml = html });
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void AddAnswer(int questionid)
        {

            generatedHtml = String.Empty;
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var ans = new Answer();
                ans.AnswerContent = String.Empty;
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var maxSerial = question.Answers.Max(k => k.SerialOrder);
                    if (maxSerial.HasValue) { ans.SerialOrder = maxSerial.Value + 1; }
                    else { ans.SerialOrder = 0; }
                    question.Answers.Add(ans);
                    if (db.SaveChanges() > 0)
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(question);
                        }
                    }
                }
            }
            catch (Exception)
            {

                success = false;
            }

        }

        public void ResortQuestions(int count, List<Question> questions)
        {
            success = false;

            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var questionsDb = db.Questions;
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

        }

        public void DeleteQuestion(int questionid)
        {
            success = false;

            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
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
                        var tags = question.TagInQuestions.ToList();
                        tags.ForEach(k => db.TagInQuestions.Remove(k));
                        db.Questions.Remove(question);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            message = Constants.DefaultSuccessMessage;
                        }
                    }


                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void DeleteAnswer(int answerid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var ans = db.Answers.FirstOrDefault(i => i.AnswerID == answerid);
                if (ans != null)
                {
                    var question = ans.Question;
                    if (question != null)
                    {
                        //if (question.UserInTestDetails.Count > 0)
                        //{
                        //    success = false;
                        //    message = "This Question Already In Use.";
                        //}
                        //else
                        //{
                        db.Answers.Remove(ans);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }
                        //}
                    }

                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateAnswer(List<Answer> answers)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                List<Answer> dbAnswers = new List<Answer>();
                answers.ForEach(delegate(Answer ans)
                {
                    var ansDb = db.Answers.FirstOrDefault(i => i.AnswerID == ans.AnswerID);
                    if (ansDb != null)
                    {
                        ansDb.AnswerContent = String.IsNullOrEmpty(ans.AnswerContent) ? string.Empty : (ans.AnswerContent);
                        ansDb.IsRight = ans.IsRight;
                        ansDb.Score = ans.Score;
                        ansDb.SerialOrder = ans.SerialOrder;
                        dbAnswers.Add(ansDb);
                    }
                });
                RecalculateUserInTestScore(dbAnswers);
                if (db.SaveChanges() >= 0)
                {
                    success = true;
                }
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateQuestionTitle(int questionid, string newtext)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    question.QuestionTitle = string.IsNullOrEmpty(newtext) ? string.Empty : (newtext);
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
        }
        public void UpdateQuestionTextDescription(int questionid, string text)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
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
        }
        public void UpdateTestTitle(int testid, string text)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        var tests = db.Tests.Where(k => k.TestID != testid).ToList();
                        var duplicatedTestByName = tests.FirstOrDefault(i => i.TestTitle.Trim() == text.Trim());
                        if (duplicatedTestByName != null)
                        {
                            success = false;
                            message = "Duplicate Test Name.";
                        }
                        else
                        {
                            test.TestTitle = text.Trim();
                            if (db.SaveChanges() >= 0)
                            {
                                success = true;
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateQuestionType(int questionid, string type)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var questionType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                    if (questionType != null)
                    {
                        question.QuestionType = questionType;
                        if (db.SaveChanges() >= 0)
                        {
                            success = true;
                            if (OnRenderPartialViewToString != null)
                            {
                                generatedHtml = OnRenderPartialViewToString.Invoke(question);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateStartEnd(int testid, DateTime start, DateTime end)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
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
        }
        public void UpdateSettings(int testid, String settingKey, bool isactive, int testtime)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var settingConfig = test.SettingConfig;
                    var authen = AuthenticationSessionModel.Instance();
                    var isOwner = authen.UserId == test.CreatedUserID;
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
                    var detail = currentSetting.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == settingKey);
                    if (detail != null)
                    {
                        detail.IsActive = isactive;
                        if (detail.SettingType.SettingTypeKey == "OSM")
                        {
                            detail.NumberValue = testtime;
                        }
                        else if (detail.SettingType.SettingTypeKey == "RTC" && isactive)
                        {
                            detail.TextValue = GenerateAccessKey(8);
                        }
                        if (db.SaveChanges() >= 0)
                        {
                            if (OnRenderPartialViewToStringWithParameter != null)
                            {
                                success = true;
                                message = String.Empty;
                                generatedHtml = OnRenderPartialViewToStringWithParameter.Invoke(detail, isOwner);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void DeActiveTest(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
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
                        var authen = AuthenticationSessionModel.Instance();
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_deactivetest(test.TestID, authen.User.UserMail);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void EnableTest(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(k => k.TestID == testid);
                if (test != null)
                {
                    test.IsActive = true;
                    if (db.SaveChanges() >= 0)
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            message = "Successful active this test";
                            generatedHtml = OnRenderPartialViewToString.Invoke(test);
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void Login(string email, string password, bool remembered)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(delegate(User u)
                {
                    if (!String.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(u.UserMail) && !string.IsNullOrEmpty(u.Password))
                    {
                        var uEmail = u.UserMail.Trim();
                        var uPass = u.Password.Trim();
                        var iEmail = email.Trim();
                        var iPass = password.Trim();
                        return uEmail.Equals(iEmail) && uPass.Equals(iPass);
                    }
                    else
                    {
                        return false;
                    }
                });
                if (user != null)
                {
                    var authen = AuthenticationSessionModel.Instance();
                    authen.IsCookieEnable = remembered;
                    authen.UserId = user.UserID;
                    success = true;
                    message = String.Empty;
                }
                else
                {
                    success = false;
                    message = "Login failed, invalid email or password, pleasy try again.";
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void SignUp(User user)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var newUser = new User();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Password = user.Password;
                newUser.UserMail = user.UserMail;
                newUser.UserPhone = user.UserPhone;
                newUser.UserCountry = user.UserCountry;
                db.Users.Add(newUser);
                if (db.SaveChanges() > 0)
                {
                    success = true;
                    message = Constants.DefaultSignUpSuccessMessage;
                    var authen = AuthenticationSessionModel.Instance();
                    authen.UserId = newUser.UserID;
                    if (OnRenderPartialViewToString != null)
                    {
                        generatedHtml = OnRenderPartialViewToString.Invoke(null);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void SubmitTest(UserInTest userInTest)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;

            try
            {
                var authen = AuthenticationSessionModel.Instance();
                userInTest.UserID = authen.UserId;
                userInTest.TestTakenDate = DateTime.Now;
                var db = SingletonDb.Instance();

                var oldUserInTest = db.UserInTests.Where(i => i.TestID == userInTest.TestID && i.UserID == userInTest.UserID);
                if (oldUserInTest.Count() > 0)
                {
                    userInTest.NumberOfAttend = oldUserInTest.Max(i => i.NumberOfAttend) + 1;
                }
                else
                {
                    userInTest.NumberOfAttend = 1;
                }

                var userInTestDetails = userInTest.UserInTestDetails.ToList();
                userInTestDetails.ForEach(i =>
                {
                    var question = db.Questions.FirstOrDefault(j => j.QuestionID == i.QuestionID);
                    if (question != null)
                    {
                        if (i.AnswerIDs != null)
                        {

                            var IDs = i.AnswerIDs.Split(',');
                            var score = IDs.Sum(k =>
                            {
                                var answer = question.Answers.FirstOrDefault(m => m.AnswerID.ToString() == k);
                                return answer.Score < 0 ? 0 : answer.Score;
                            });
                            i.ChoiceScore = score;

                        }
                        else
                        {
                            if (question.QuestionType.Type == "ShortAnswer")
                            {
                                if (!string.IsNullOrEmpty(i.AnswerContent))
                                {
                                    var userAnswers = i.AnswerContent.Split(',');
                                    if (!string.IsNullOrEmpty(question.TextDescription))
                                    {
                                        var questionAnswers = question.TextDescription.Split(',');

                                        if (questionAnswers.All(o => userAnswers.Contains(o)))
                                        {
                                            i.NonChoiceScore = question.NoneChoiceScore;
                                        }
                                        else
                                        {
                                            i.NonChoiceScore = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }

                });
                userInTest.Score = userInTestDetails.Sum(i => i.NonChoiceScore ?? 0 + i.ChoiceScore ?? 0);
                userInTest.UserInTestDetails = userInTestDetails;

                db.UserInTests.Add(userInTest);


                if (db.SaveChanges() > 0)
                {
                    success = true;
                    message = Constants.DefaultSubmitTestSuccessMessage;

                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void UpdateUserEmail(int userId, string userEmail)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);//may be null
                if (user != null)
                {
                    user.UserMail = userEmail;
                    if (db.SaveChanges() >= 0) //SaveChanges return affected rows
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }
        public void UpdateUserName(int userId, string userName)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);
                if (user != null)
                {
                    user.FirstName = userName;
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateProfile(User profile)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == authen.UserId);
                if (user != null)
                {
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    user.UserMail = profile.UserMail;
                    if (!string.IsNullOrEmpty(profile.Password))
                    {
                        user.Password = profile.Password;
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = "Success on save your profile.";
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public bool IsMatchOldPass(string pass)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            var ismatch = false;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var user = authen.User;
                if (user != null)
                {
                    if (!String.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(user.Password))
                    {
                        ismatch = pass.Trim() == user.Password.Trim();
                    }
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return ismatch;
        }
        public void AssignTestToStudent(int userId, int testId)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = string.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);//find user in db
                var test = db.Tests.FirstOrDefault(k => k.TestID == testId);//find test in db
                var userRole = db.Roles.FirstOrDefault(k => k.RoleDescription == "Student");
                if (user != null && test != null)
                {
                    var invitation = new Invitation();
                    var key = GenerateInvitationAccessToken(test.TestID, user.UserID);
                    invitation.AccessToken = key;
                    invitation.Test = test;
                    invitation.Role = userRole;
                    invitation.InvitationDateTime = DateTime.Now;
                    user.Invitations.Add(invitation);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        if (OnRenderPartialViewToString != null)
                        {
                            generatedHtml = OnRenderPartialViewToString.Invoke(user);
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }
        public void AssignTestToTeacher(int userId, int testId)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = string.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);//find user in db
                var test = db.Tests.FirstOrDefault(k => k.TestID == testId);//find test in db
                var userRole = db.Roles.FirstOrDefault(k => k.RoleDescription == "Teacher");
                if (user != null && test != null)
                {
                    var invitation = new Invitation();
                    var key = GenerateInvitationAccessToken(test.TestID, user.UserID);
                    invitation.AccessToken = key;
                    invitation.Test = test;
                    invitation.Role = userRole;
                    invitation.InvitationDateTime = DateTime.Now;
                    user.Invitations.Add(invitation);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        if (OnRenderPartialViewToString != null)
                        {
                            generatedHtml = OnRenderPartialViewToString.Invoke(user);
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }
        public void UnassignTest(int userId, int testId)
        {
            success = false;
            generatedHtml = string.Empty;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userId);
                var unassignedInvitation = user.Invitations.FirstOrDefault(i => i.TestID == testId);
                user.Invitations.Remove(unassignedInvitation);
                db.Invitations.Remove(unassignedInvitation);
                if (db.SaveChanges() > 0)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderPartialViewToString.Invoke(user);
                    }


                }
            }
            catch (Exception ex)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void NewTest_ResponseTab_CheckUserIds(int testid, List<int> userids, int count)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    if (count == 0) { userids = new List<int>(); }
                    var responseTest = new ResponseTest(test, userids);
                    generatedHtml = OnRenderPartialViewToString.Invoke(responseTest);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void NewTest_ScoreTab_CheckUserIds(int testid, List<int> userids, int count)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    if (count == 0) { userids = new List<int>(); }
                    var scoreTest = new ScoreTest(test, userids);
                    generatedHtml = OnRenderPartialViewToString.Invoke(scoreTest);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public int DuplicateTest(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            var newId = 0;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var newTest = new Test();
                newTest.TestTitle = "COPY: " + test.TestTitle;
                newTest.CreatedDateTime = DateTime.Now;
                newTest.CreatedUserID = authen.UserId;
                newTest.Duration = test.Duration;
                newTest.EndDateTime = test.EndDateTime;
                newTest.StartDateTime = test.StartDateTime;
                newTest.IsActive = true;
                newTest.SettingConfig = test.SettingConfig;
                var tagsInTest = test.TagInTests.ToList();
                newTest.TagInTests = new List<TagInTest>();
                tagsInTest.ForEach(i =>
                {
                    var tagInTest = new TagInTest();
                    tagInTest.Tag = i.Tag;
                    tagInTest.SerialOrder = i.SerialOrder;
                    newTest.TagInTests.Add(tagInTest);
                });

                var questions = test.Questions.ToList();
                newTest.Questions = new List<Question>();
                questions.ForEach(i =>
                    {
                        var question = new Question();
                        question.ImageUrl = i.ImageUrl;
                        question.LabelOrder = i.LabelOrder;
                        question.NoneChoiceScore = i.NoneChoiceScore;
                        question.QuestionTitle = i.QuestionTitle;
                        question.QuestionType = i.QuestionType;
                        question.SerialOrder = i.SerialOrder;
                        var tagsInQuestion = question.TagInQuestions.ToList();
                        question.TagInQuestions = new List<TagInQuestion>();
                        tagsInQuestion.ForEach(k =>
                        {
                            var tagInquestion = new TagInQuestion();
                            tagInquestion.Tag = k.Tag;
                            tagInquestion.SerialOrder = k.SerialOrder;
                            question.TagInQuestions.Add(tagInquestion);
                        });
                        question.TextDescription = i.TextDescription;
                        var answers = i.Answers.ToList();
                        question.Answers = new List<Answer>();
                        answers.ForEach(k =>
                            {
                                var answer = new Answer();
                                answer.AnswerContent = k.AnswerContent;
                                answer.IsRight = k.IsRight;
                                answer.Score = k.Score;
                                answer.SerialOrder = k.SerialOrder;
                                question.Answers.Add(answer);
                            });
                        newTest.Questions.Add(question);
                    });
                db.Tests.Add(newTest);
                if (db.SaveChanges() > 0)
                {
                    newId = newTest.TestID;
                    success = true;
                    message = "Duplicate test successful.";
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
            return newId;
        }
        public void SearchTagsOnTest(int testid, string term, int maxrows)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<object>();
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var existTags = test.TagInTests.Select(k => k.Tag);
                    var allTags = db.Tags.ToList();
                    var filteredTags = allTags.Where(k => !existTags.Contains(k));
                    var lowerTerm = term.ToLower();
                    foreach (var item in filteredTags)
                    {
                        if (item.TagName.ToLower().Contains(lowerTerm))
                        {
                            resultlist.Add(new { id = item.TagID, tagname = item.TagName });
                        }
                    }
                    resultlist = resultlist.Take(maxrows).ToList();
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void AddTagToTest(int testid, int tagid, string tagname)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var tag = db.Tags.FirstOrDefault(i => i.TagID == tagid);
                    if (tag == null)
                    {
                        if (!string.IsNullOrEmpty(tagname))
                        {
                            tag = new Tag();
                            tag.TagName = tagname;
                        }
                    }
                    if (tag != null)
                    {
                        var tagsInTest = test.TagInTests;
                        var order = 0;
                        if (tagsInTest.Count != 0)
                        {
                            var max = tagsInTest.Max(k => k.SerialOrder);
                            if (max.HasValue)
                            {
                                order = max.Value + 1;
                            }
                        }
                        var newTagInTest = new TagInTest();
                        newTagInTest.Tag = tag;
                        newTagInTest.SerialOrder = order;
                        test.TagInTests.Add(newTagInTest);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            if (OnRenderPartialViewToString != null)
                            {
                                generatedHtml = OnRenderPartialViewToString.Invoke(tag);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void RemoveTagToTest(int testid, int tagid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var tagInTest = db.TagInTests.FirstOrDefault(i => i.TestID == testid && i.TagID == tagid);
                if (tagInTest != null)
                {
                    db.TagInTests.Remove(tagInTest);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void SortTagToTest(int testid, List<int> ids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var tagsInTest = test.TagInTests;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        var id = ids[i];
                        var tagInTest = tagsInTest.FirstOrDefault(k => k.TagID == id);
                        if (tagInTest != null)
                        {
                            tagInTest.SerialOrder = i;
                        }
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void SearchTagsOnQuestion(int questionid, string term, int maxrows)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<object>();
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var existTags = question.TagInQuestions.Select(k => k.Tag);
                    var allTags = db.Tags.ToList();
                    var filteredTags = allTags.Where(k => !existTags.Contains(k));
                    var lowerTerm = term.ToLower();
                    foreach (var item in filteredTags)
                    {
                        if (item.TagName.ToLower().Contains(lowerTerm))
                        {
                            resultlist.Add(new { id = item.TagID, tagname = item.TagName });
                        }
                    }
                    resultlist = resultlist.Take(maxrows).ToList();
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void AddTagToQuestion(int questionid, int tagid, string tagname)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var tag = db.Tags.FirstOrDefault(i => i.TagID == tagid);
                    if (tag == null)
                    {
                        if (!string.IsNullOrEmpty(tagname))
                        {
                            tag = new Tag();
                            tag.TagName = tagname;
                        }
                    }
                    if (tag != null)
                    {
                        var tagsInQuestion = question.TagInQuestions;
                        var order = 0;
                        if (tagsInQuestion.Count != 0)
                        {
                            var max = tagsInQuestion.Max(k => k.SerialOrder);
                            if (max.HasValue)
                            {
                                order = max.Value + 1;
                            }
                        }
                        var newTagInQuestion = new TagInQuestion();
                        newTagInQuestion.Tag = tag;
                        newTagInQuestion.SerialOrder = order;
                        question.TagInQuestions.Add(newTagInQuestion);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            if (OnRenderPartialViewToString != null)
                            {
                                generatedHtml = OnRenderPartialViewToString.Invoke(tag);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void RemoveTagToQuestion(int questionid, int tagid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var tagInQuestion = db.TagInQuestions.FirstOrDefault(i => i.QuestionID == questionid && i.TagID == tagid);
                if (tagInQuestion != null)
                {
                    db.TagInQuestions.Remove(tagInQuestion);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void SortTagToQuestion(int questionid, List<int> ids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var tagsInQuestion = question.TagInQuestions;
                    for (int i = 0; i < ids.Count; i++)
                    {
                        var id = ids[i];
                        var tagInQuestion = tagsInQuestion.FirstOrDefault(k => k.TagID == id);
                        if (tagInQuestion != null)
                        {
                            tagInQuestion.SerialOrder = i;
                        }
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateNoneChoiceScore(int questionid, decimal score)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    if (question.QuestionType.Type == "Essay" || question.QuestionType.Type == "ShortAnswer")
                    {
                        question.NoneChoiceScore = score;
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            message = string.Empty;
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void Index_TestListTab()
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                success = true;
                if (OnRenderPartialViewToString != null)
                {
                    var testList = new TestList(db.Tests);
                    generatedHtml = OnRenderPartialViewToString.Invoke(testList);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

        }
        public void ModalFeedBackPopup(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        AbsFeedBackUser feedBackUser = new FeedBackStudent(test);
                        generatedHtml = OnRenderPartialViewToString.Invoke(feedBackUser);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void StudentCommentFeedBack(int testid, string fbDetail)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid); //#1
                if (test != null)
                {
                    //new a FeedBack
                    var feedback = new FeedBack();
                    //assign attribute
                    feedback.UserID = authen.UserId;
                    feedback.FeedBackDetail = fbDetail;
                    feedback.FeedBackDateTime = DateTime.Now;
                    feedback.ParentID = null;

                    //add to test #1
                    test.FeedBacks.Add(feedback);
                    //save
                    if (db.SaveChanges() > 0)
                    {
                        db.Entry(feedback).Reference(p => p.User).Load();
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(feedback);
                            var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                            context.Clients.All.R_commentFeedback(testid, generatedHtml);
                        }
                    }

                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UserReplyFeedBack(int testid, int parentFeedBackId, string replyDetail)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid); //#1
                if (test != null)
                {
                    //new a FeedBack
                    var feedback = new FeedBack();
                    //assign attribute
                    feedback.UserID = authen.UserId;
                    feedback.FeedBackDetail = replyDetail;
                    feedback.FeedBackDateTime = DateTime.Now;
                    feedback.ParentID = parentFeedBackId;

                    //add to test #1
                    test.FeedBacks.Add(feedback);
                    //save
                    if (db.SaveChanges() > 0)
                    {
                        db.Entry(feedback).Reference(p => p.User).Load();
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(feedback);
                            var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                            context.Clients.All.R_replyFeedback(testid, parentFeedBackId, generatedHtml);
                        }
                    }

                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        private string GenerateAccessKey(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
        private string GenerateInvitationAccessToken(int testid, int userid)
        {
            var keys = new List<Object>();
            keys.Add(testid);
            keys.Add(userid);
            var key = ExtensionModel.createHashMD5(keys);
            var specialUrlChars = new List<string>() {
                "/","=","?"
            };
            specialUrlChars.ForEach(i => key = key.Replace(i, string.Empty));
            return key;
        }
        public void UpdateTestIntroduction(int testid, string introduction)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    test.Introduction = introduction;
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void ModalTestHistoryPopup(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        var authen = AuthenticationSessionModel.Instance();
                        var listIds = new List<int>() { authen.UserId };
                        var resTest = new ResponseTest(test, listIds);
                        generatedHtml = OnRenderPartialViewToString.Invoke(resTest);
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        private void RecalculateUserInTestScore(IEnumerable<Answer> answers)
        {
            var groupAnswers = from ans in answers
                               group ans by ans.Question into GroupAnswers
                               select new { Key = GroupAnswers.Key };
            foreach (var item in groupAnswers)
            {
                if (item.Key != null)
                {
                    var question = item.Key;
                    var inTestDetails = question.UserInTestDetails.ToList();
                    var inTests = from d in inTestDetails
                                  group d by d.UserInTest into InTestGroup
                                  select new { Key = InTestGroup.Key };
                    foreach (var intest in inTests)
                    {
                        var inTestObj = intest.Key;
                        var details = inTestObj.UserInTestDetails.Where(k => k.QuestionID == question.QuestionID).ToList();
                        details.ForEach(i =>
                        {
                            if (question != null)
                            {
                                if (i.AnswerIDs != null)
                                {
                                    var IDs = i.AnswerIDs.Split(',');
                                    var score = IDs.Sum(k =>
                                    {
                                        var answer = question.Answers.FirstOrDefault(m => m.AnswerID.ToString() == k);
                                        return answer.Score < 0 ? 0 : answer.Score;
                                    });
                                    i.ChoiceScore = score;
                                }
                                else
                                {
                                    if (question.QuestionType.Type == "ShortAnswer")
                                    {
                                        if (!string.IsNullOrEmpty(i.AnswerContent))
                                        {
                                            var userAnswers = i.AnswerContent.Split(',');
                                            if (!string.IsNullOrEmpty(question.TextDescription))
                                            {
                                                var questionAnswers = question.TextDescription.Split(',');

                                                if (questionAnswers.All(o => userAnswers.Contains(o)))
                                                {
                                                    i.NonChoiceScore = question.NoneChoiceScore;
                                                }
                                                else
                                                {
                                                    i.NonChoiceScore = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        });
                        inTestObj.Score = inTestObj.UserInTestDetails.Sum(i => i.NonChoiceScore ?? 0 + i.ChoiceScore ?? 0);
                    }
                }
            }
        }
        private void RecalculateUserInTestScore(UserInTest userInTest)
        {
            var details = userInTest.UserInTestDetails;
            var score = details.Sum(i => i.NonChoiceScore ?? 0 + i.ChoiceScore ?? 0);
            userInTest.Score = score;
        }
        public void UpdateUserNoneChoiceScore(int questionid, int userid, decimal score)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var detail = question.UserInTestDetails.FirstOrDefault(i => i.UserInTest.UserID == userid);
                    var type = question.QuestionType.Type;
                    if (detail != null && (type == "Essay" || type == "ShortAnswer"))
                    {
                        detail.NonChoiceScore = score;
                        var userInTest = detail.UserInTest;
                        RecalculateUserInTestScore(userInTest);
                        if (db.SaveChanges() >= 0)
                        {
                            success = true;
                            message = Constants.DefaultSuccessMessage;
                        }
                    }
                }
            }
            catch (Exception)
            {
                success = true;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public Invitation AnonymousDoTest(string key)
        {
            Invitation invitation = null;
            try
            {
                var db = SingletonDb.Instance();
                invitation = db.Invitations.FirstOrDefault(i => i.AccessToken == key);
                if (invitation != null)
                {
                    //anonymous login
                    var authen = AuthenticationSessionModel.Instance();
                    authen.UserId = invitation.UserID;
                }
            }
            catch (Exception)
            {
                invitation = null;
            }
            return invitation;
        }
        public ExcelPackage ScoreToExcel(int testid, List<int> userids)
        {
            ExcelPackage pack = null;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null) {
                    var scoreTest = new ScoreTest(test, userids);
                    pack = scoreTest.ToExcelPackage();
                }
            }
            catch (Exception)
            {

                pack = null;
            }
            return pack;
        }
    }
}