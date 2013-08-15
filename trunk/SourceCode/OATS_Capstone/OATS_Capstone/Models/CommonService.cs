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
        object _data = null;

        public object data
        {
            get { return _data; }
            set { _data = value; }
        }

        public event OnRenderPartialViewHandler OnRenderPartialViewToString;
        public event OnRenderPartialViewHandler OnRenderSubPartialViewToString;
        public event OnRenderPartialViewHandlerWithParameter OnRenderPartialViewToStringWithParameter;

        bool _isHavePermission = false;

        public bool IsHavePermission
        {
            get { return _isHavePermission; }
            set { _isHavePermission = value; }
        }

        string _accessToken = string.Empty;

        public string accessToken
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        bool _isregistered = true;
        public bool isregistered
        {
            get { return _isregistered; }
            set { _isregistered = value; }
        }

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
                    var fName = file.FileName;
                    var fNameIndex = fName.LastIndexOf('.');
                    fName = fName.Insert(fNameIndex, "_" + DateTime.Now.Ticks.ToString());

                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Resource/Images"), fName);
                    System.IO.File.WriteAllBytes(filePath, this.ReadData(file.InputStream));
                    //write path to db
                    var db = SingletonDb.Instance();
                    var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                    if (question != null)
                    {
                        question.ImageUrl = "~/Resource/Images/" + fName;
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

        public void ModalPopupUser(int testid, string term = "")
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var users = new List<User>();
                var authen = AuthenticationSessionModel.Instance();

                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var invitedUsers = test.Invitations.Select(i => i.User.UserID);
                    users = db.Users.Where(i => !invitedUsers.Contains(i.UserID) && !invitedUsers.Contains(test.CreatedUserID)).Where(k => k.UserID != authen.UserId).ToList();
                    if (!string.IsNullOrEmpty(term))
                    {
                        success = true;
                        var lower = term.ToLower();
                        users.ForEach(delegate(User user)
                        {
                            var name = false;
                            var mail = false;
                            if (user.Name != null) { if (user.Name.ToLower().Contains(lower)) { name = true; } }
                            if (user.UserMail != null) { if (user.UserMail.ToLower().Contains(lower)) { mail = true; } }
                            if (name || mail)
                            {
                                if (OnRenderSubPartialViewToString != null)
                                {
                                    var html = OnRenderSubPartialViewToString.Invoke(user);
                                    resultlist.Add(html);
                                }
                            }
                        });
                    }
                    else
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            //sort
                            users = users.OrderBy(i => i.UserMail).ToList();
                            generatedHtml = OnRenderPartialViewToString.Invoke(users);
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

        public void ModalRemovePopupUser(int testid, string role, string term)
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
                    //sort
                    users = users.OrderBy(i => i.UserMail).ToList();
                    if (!string.IsNullOrEmpty(term))
                    {
                        success = true;
                        var lower = term.ToLower();
                        users.ForEach(delegate(User user)
                        {
                            var name = false;
                            var mail = false;
                            if (user.Name != null) { if (user.Name.ToLower().Contains(lower)) { name = true; } }
                            if (user.UserMail != null) { if (user.UserMail.ToLower().Contains(lower)) { mail = true; } }
                            if (name || mail)
                            {
                                if (OnRenderSubPartialViewToString != null)
                                {
                                    var html = OnRenderSubPartialViewToString.Invoke(user);
                                    resultlist.Add(html);
                                }
                            }
                        });
                    }
                    else
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(users);
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

        public void ModalReinvitePopupUser(int testid, string role, string term)
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
                    //sort
                    users = users.OrderBy(i => i.UserMail).ToList();

                    if (!string.IsNullOrEmpty(term))
                    {
                        success = true;
                        var lower = term.ToLower();
                        users.ForEach(delegate(User user)
                        {
                            var name = false;
                            var mail = false;
                            if (user.Name != null) { if (user.Name.ToLower().Contains(lower)) { name = true; } }
                            if (user.UserMail != null) { if (user.UserMail.ToLower().Contains(lower)) { mail = true; } }
                            if (name || mail)
                            {
                                if (OnRenderSubPartialViewToString != null)
                                {
                                    var html = OnRenderSubPartialViewToString.Invoke(user);
                                    resultlist.Add(html);
                                }
                            }
                        });
                    }
                    else
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(users);
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
                            answer.DependencyAnswerID = k.DependencyAnswerID;
                            foreach (var item in k.AnswerChilds)
                            {
                                var ans = new Answer();
                                ans.AnswerContent = item.AnswerContent;
                                ans.IsRight = item.IsRight;
                                ans.Score = item.Score;
                                ans.SerialOrder = item.SerialOrder;
                                ans.DependencyAnswerID = item.DependencyAnswerID;
                                answer.AnswerChilds.Add(ans);
                                question.Answers.Add(ans);
                            }
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
                        var tags = i.TagInQuestions.Select(k => k.TagID);
                        tags=tags.Concat(i.Test.TagInTests.Select(k => k.TagID));
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

        public void TestsAssignUserSearch(int userid, List<int> tagids, string letter)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var authenUserId = authen.UserId;
                var curUser = authen.User;

                var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                var testsInInvitation = user.Invitations.Select(i => i.Test);
                var tests = curUser.Tests;
                var filteredTests = tests.Where(i =>
                {
                    return !testsInInvitation.Select(k => k.TestID).Contains(i.TestID);
                }).ToList();

                if (!string.IsNullOrEmpty(letter))
                {
                    if (tagids == null) { tagids = new List<int>(); }
                    var tags = db.Tags.Where(i => tagids.Contains(i.TagID));
                    var lower = letter.Trim().ToLower();

                    if (tagids.Count > 0)
                    {
                        var rawTests = tags.SelectMany(i => i.TagInTests.Select(k => k.Test));
                        var groupTests = from t in rawTests
                                         group t by t into GroupTests
                                         select GroupTests.Key;
                        filteredTests = groupTests.ToList();
                    }
                    filteredTests.ForEach(delegate(Test test)
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
                                testTemplate.IsRunning = test.IsRunning;
                                testTemplate.IsComplete = test.IsComplete;
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
                                testTemplate.IsRunning = test.IsRunning;
                                testTemplate.IsRunning = test.IsComplete;
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
                var curUserId = authen.UserId;
                if (!string.IsNullOrEmpty(term))
                {
                    var lower = term.ToLower();
                    var users = db.Users.Where(i => i.UserID != curUserId).ToList();
                    users.ForEach(delegate(User user)
                    {
                        var name = false;
                        var mail = false;
                        if (user.Name != null) { if (user.Name.ToLower().Contains(lower)) { name = true; } }
                        if (user.UserMail != null) { if (user.UserMail.ToLower().Contains(lower)) { mail = true; } }
                        if (name || mail)
                        {
                            var userTemplate = new SearchingUsers();
                            userTemplate.UserID = user.UserID;
                            userTemplate.Name = user.Name;
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

        public int MakeTest(string testTitle)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            var generatedId = 0;
            try
            {
                if (!string.IsNullOrEmpty(testTitle))
                {
                    
                        var db = SingletonDb.Instance();
                        testTitle = testTitle.Trim();
                        var tTestTitle = db.Tests.FirstOrDefault(i => i.TestTitle.ToLower() == testTitle.ToLower());
                        if (tTestTitle == null)
                        {
                            var authen = AuthenticationSessionModel.Instance();
                            if (authen.UserId != 0)
                            {
                                var spanDays = 7;
                                var test = new Test();
                                test.TestTitle = testTitle;
                                test.CreatedUserID = authen.UserId;//must be fix, this is for test purpose
                                var now = DateTime.Now;
                                test.CreatedDateTime = now;
                                test.StartDateTime = now;
                                test.EndDateTime = now.AddDays(spanDays);
                                test.SettingConfigID = 1;
                                test.IsActive = true;
                                test.IsRunning = true;
                                test.IsComplete = false;
                                db.Tests.Add(test);
                                if (db.SaveChanges() > 0)
                                {
                                    success = true;
                                    message = Constants.DefaultSuccessMessage;
                                    generatedId = test.TestID;
                                }
                            }
                        }
                        else {
                            success = false;
                            message = "Duplicate Test Name";
                        }
                }
                else {
                    success = false;
                    message = "Please specify test title.";
                }
            }
            catch (Exception)
            {
                generatedId = 0;
                success = false;
                message = Constants.DefaultExceptionMessage;
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
                var invitationMails = new List<Invitation>();
                if (count > 0)
                {
                    userids.ForEach(delegate(int id)
                    {
                        var user = db.Users.FirstOrDefault(k => k.UserID == id);
                        if (user != null)
                        {
                            var existInvitation = test.Invitations.FirstOrDefault(i => i.UserID == id);
                            if (existInvitation == null)
                            {
                                var invitation = new Invitation();
                                var key = GenerateInvitationAccessToken(test.TestID, id);
                                invitation.IsMailSent = false;
                                invitation.AccessToken = key;
                                invitation.User = user;
                                invitation.Role = userRole;
                                invitation.InvitationDateTime = DateTime.Now;
                                test.Invitations.Add(invitation);
                                invitationMails.Add(invitation);
                            }
                        }
                    });
                }
                if (db.SaveChanges() >= 0)
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderPartialViewToString.Invoke(test);
                    }
                    //send mail
                    var authen = AuthenticationSessionModel.Instance();
                    var userid = authen.UserId;
                    IUserMailer userMailer = new UserMailer();
                    userMailer.OnAcknowledInvitationEmail += (init, sent, unsent, list) =>
                    {
                        if (list.Count > 0)
                        {
                            list.ForEach(i =>
                            {
                                var invitation = db.Invitations.FirstOrDefault(k => k.InvitationID == i);
                                if (invitation != null)
                                {
                                    invitation.IsMailSent = true;
                                }
                            });
                            db.SaveChanges();
                        }
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_AcknowledgeEmailCallback(userid, init, sent, unsent, list);
                    };

                    userMailer.InviteUsers(invitationMails);
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
                var userid = authen.UserId;
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
                        //send mail
                        IUserMailer userMailer = new UserMailer();
                        userMailer.OnAcknowledInvitationEmail += (init, sent, unsent, list) =>
                        {
                            if (list.Count > 0)
                            {
                                list.ForEach(i =>
                                {
                                    var invitation = db.Invitations.FirstOrDefault(k => k.InvitationID == i);
                                    if (invitation != null)
                                    {
                                        invitation.IsMailSent = true;
                                    }
                                });
                                db.SaveChanges();
                            }
                            var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                            context.Clients.All.R_AcknowledgeEmailCallback(userid, init, sent, unsent, list);
                        };
                        userMailer.ReInviteUsers(invitations);
                    }
                    success = true;
                }
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
                if (test != null)
                {
                    //var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                    var inv = test.Invitations.FirstOrDefault(k => k.UserID == userid);
                    test.Invitations.Remove(inv);
                    db.Invitations.Remove(inv);
                    var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                    if (user != null)
                    {
                        if (!user.IsRegistered)
                        {
                            user.Invitations.Remove(inv);
                            if (user.Invitations.Count == 0 && user.FeedBacks.Count == 0 && user.Tests.Count == 0 && user.UserInTests.Count == 0)
                            {
                                db.Users.Remove(user);
                            }
                        }
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_removeInvitation(testid, new List<int> { userid });
                    }
                }
            }
            catch (Exception ex)
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
                                if (!user.IsRegistered)
                                {
                                    user.Invitations.Remove(inv);
                                    if (user.Invitations.Count == 0 && user.FeedBacks.Count == 0 && user.Tests.Count == 0 && user.UserInTests.Count == 0)
                                    {
                                        db.Users.Remove(user);
                                    }
                                }
                            }
                        });
                    }
                    if (db.SaveChanges() >= 0)
                    {
                        if (OnRenderPartialViewToString != null)
                        {
                            success = true;
                            generatedHtml = OnRenderPartialViewToString.Invoke(test);
                        }
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_removeInvitation(testid, userids);
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
                var tests = user.Tests.ToList();


                tests.ForEach(delegate(Test test)
                {
                    var template = new TestCalendarObject();
                    template.id = test.TestID;
                    template.testTitle = test.TestTitle;
                    template.startDateTime = test.StartDateTime;
                    template.endDateTime = test.EndDateTime;
                    template.isOwner = true;
                    resultlist.Add(template);
                });
                invitedTests.ForEach(delegate(Test test)
                {
                    var template = new TestCalendarObject();
                    template.id = test.TestID;
                    template.testTitle = test.TestTitle;
                    template.startDateTime = test.StartDateTime;
                    template.endDateTime = test.EndDateTime;
                    template.isOwner = false;
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
                    generatedHtml = OnRenderPartialViewToString.Invoke(test);
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
                        var ans1 = new Answer() { AnswerContent = string.Empty, SerialOrder = 0, Score = 0 };
                        var ans2 = new Answer() { AnswerContent = string.Empty, SerialOrder = 1, Score = 0 };
                        question.Answers.Add(ans1);
                        question.Answers.Add(ans2);
                    }
                    if (dbType.Type == "Essay" || dbType.Type == "ShortAnswer")
                    {
                        question.NoneChoiceScore = 0;
                    }
                    if (dbType.Type == "Matching")
                    {
                        //create two couple->four answer
                        //couple 1
                        var c1Ans1 = new Answer { AnswerContent = string.Empty, SerialOrder = 0 };
                        c1Ans1.Score = 1;
                        c1Ans1.IsRight = true;
                        var c1Ans2 = new Answer { AnswerContent = string.Empty };
                        c1Ans2.IsRight = false;
                        c1Ans1.AnswerChilds.Add(c1Ans2);
                        //couple 2
                        var c2Ans1 = new Answer { AnswerContent = string.Empty, SerialOrder = 1 };
                        c2Ans1.IsRight = true;
                        c2Ans1.Score = 1;
                        var c2Ans2 = new Answer { AnswerContent = string.Empty };
                        c2Ans2.IsRight = false;
                        c2Ans1.AnswerChilds.Add(c2Ans2);

                        question.Answers.Add(c1Ans1);
                        question.Answers.Add(c1Ans2);
                        question.Answers.Add(c2Ans1);
                        question.Answers.Add(c2Ans2);
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

        public void AddListQuestion(int testid, List<Question> listquestion)
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
                    var listSuccess = new List<Question>();
                    listquestion.ForEach(delegate(Question item)
                    {

                        var qItem = item;
                        if (qItem != null)
                        {
                            var type = qItem.QuestionType.Type;
                            var realType = db.QuestionTypes.FirstOrDefault(k => k.Type == type);
                            if (realType != null)
                            {
                                qItem.QuestionType = realType;
                            }
                            var ansParents = qItem.Answers.ToList();
                            foreach (var ans in ansParents)
                            {
                                foreach (var child in ans.AnswerChilds)
                                {
                                    if (string.IsNullOrEmpty(child.AnswerContent))
                                    {
                                        child.AnswerContent = string.Empty;
                                    }
                                    qItem.Answers.Add(child);
                                }
                                if (string.IsNullOrEmpty(ans.AnswerContent))
                                {
                                    ans.AnswerContent = string.Empty;
                                }
                            }
                            qItem.QuestionTitle = qItem.QuestionTitle ?? string.Empty;
                            qItem.TextDescription = qItem.TextDescription ?? string.Empty;
                            test.Questions.Add(qItem);
                            listSuccess.Add(qItem);
                        }
                    });

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = "Import questions complete.";
                        listSuccess.ForEach(delegate(Question item)
                        {
                            var html = String.Empty;
                            if (OnRenderPartialViewToString != null)
                            {
                                html = OnRenderPartialViewToString.Invoke(item);
                                arraylist.Add(html);
                            }
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
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    //get serial
                    var maxSerial = question.Answers.Max(k => k.SerialOrder);
                    var serialOrder = 0;
                    if (maxSerial.HasValue)
                    {
                        serialOrder = maxSerial.Value + 1;
                    }
                    else
                    {
                        serialOrder = 0;
                    }

                    //handle question
                    if (question.QuestionType.Type == "Matching")
                    {
                        var ans1 = new Answer() { AnswerContent = string.Empty };
                        ans1.Score = 1;
                        ans1.SerialOrder = serialOrder;
                        ans1.IsRight = true;
                        var ans2 = new Answer() { AnswerContent = string.Empty };
                        ans2.IsRight = false;
                        ans1.AnswerChilds.Add(ans2);
                        question.Answers.Add(ans1);
                        question.Answers.Add(ans2);
                    }
                    else
                    {

                        var ans = new Answer();
                        ans.AnswerContent = String.Empty;
                        ans.SerialOrder = serialOrder;
                        question.Answers.Add(ans);
                    }
                    RecalculateUserInTestScore(question.Answers);
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
                        var type = question.QuestionType.Type;
                        if (type == "Image")
                        {
                            var url = question.ImageUrl;
                            var path = HttpContext.Current.Server.MapPath(url);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }

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
                        var answerChilds = ans.AnswerChilds.ToList();
                        if (answerChilds.Count > 0)
                        {
                            answerChilds.ForEach(i =>
                            {
                                question.Answers.Remove(i);
                                db.Answers.Remove(i);
                            });
                        }
                        var answersOfThisQuestion = ans.Question.Answers.Where(i => !i.DependencyAnswerID.HasValue);
                        RecalculateUserInTestScore(answersOfThisQuestion);
                        db.Answers.Remove(ans);
                        if (db.SaveChanges() > 0)
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

                var dbAns = dbAnswers.FirstOrDefault();
                if (dbAns != null)
                {
                    var test = dbAns.Question.Test;
                    var totalScore = test.Questions.TotalScore();
                    var settingDetail = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "MTP");
                    if (settingDetail != null)
                    {
                        if (settingDetail.IsActive)
                        {
                            if (totalScore.HasValue && settingDetail.NumberValue.HasValue)
                            {
                                if ((totalScore ?? 0) != (settingDetail.NumberValue ?? 0))
                                {
                                    test.IsRunning = false;
                                }
                                else
                                {
                                    test.IsRunning = true;
                                }
                            }
                        }
                    }
                }

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
                    else
                    {
                        success = false;
                        message = "Enter test name";
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

                        //auto add answers
                        var qType = question.QuestionType.Type;
                        if (qType == "Essay" || qType == "ShortAnswer")
                        {
                            question.NoneChoiceScore = 0;
                        }
                        if (qType == "Radio" || qType == "Multiple")
                        {
                            if (question.Answers.Count == 0)
                            {
                                var ans1 = new Answer() { AnswerContent = string.Empty, SerialOrder = 0, Score = 0 };
                                var ans2 = new Answer() { AnswerContent = string.Empty, SerialOrder = 1, Score = 0 };
                                question.Answers.Add(ans1);
                                question.Answers.Add(ans2);
                            }
                        }

                        if (qType == "Matching")
                        {
                            if (question.Answers.Count == 0)
                            {
                                //create two couple->four answer
                                //couple 1
                                var c1Ans1 = new Answer { AnswerContent = string.Empty, SerialOrder = 0 };
                                c1Ans1.Score = 1;
                                c1Ans1.IsRight = true;
                                var c1Ans2 = new Answer { AnswerContent = string.Empty };
                                c1Ans2.IsRight = false;
                                c1Ans1.AnswerChilds.Add(c1Ans2);
                                //couple 2
                                var c2Ans1 = new Answer { AnswerContent = string.Empty, SerialOrder = 1 };
                                c2Ans1.IsRight = true;
                                c2Ans1.Score = 1;
                                var c2Ans2 = new Answer { AnswerContent = string.Empty };
                                c2Ans2.IsRight = false;
                                c2Ans1.AnswerChilds.Add(c2Ans2);

                                question.Answers.Add(c1Ans1);
                                question.Answers.Add(c1Ans2);
                                question.Answers.Add(c2Ans1);
                                question.Answers.Add(c2Ans2);
                            }
                            else if (question.Answers.Count > 0)
                            {
                                var answesBegins = question.Answers.Where(i => !i.DependencyAnswerID.HasValue).ToList();
                                answesBegins.ForEach(i =>
                                {
                                    i.IsRight = true;
                                    i.Score = 1;
                                    var ans = question.Answers.FirstOrDefault(k => k.DependencyAnswerID == i.AnswerID);
                                    if (ans == null)
                                    {
                                        var cloneAns = new Answer { AnswerContent = string.Empty };
                                        cloneAns.IsRight = false;
                                        i.AnswerChilds.Add(cloneAns);
                                        question.Answers.Add(cloneAns);
                                    }
                                });
                            }
                        }

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
        public void UpdateSettings(int testid, String settingKey, bool isactive, int? number, string text)
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
                    if (settingConfig.SettingConfigID == Constants.DefaultSettingConfigId)//1 is default
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
                        if (detail.SettingType.SettingTypeKey == "DRL")
                        {
                            detail.NumberValue = number;
                        }
                        else if (detail.SettingType.SettingTypeKey == "NPP")
                        {
                            detail.NumberValue = number;
                        }
                        else if (detail.SettingType.SettingTypeKey == "OSM")
                        {
                            var isConflict = false;
                            decimal? usersMaxAttemp = null;
                            if (test.UserInTests.Count > 0) {
                                usersMaxAttemp = test.UserInTests.Max(i => i.NumberOfAttend);
                                isConflict= (number ?? 0) < usersMaxAttemp;
                            }
                            
                            if (isConflict&&usersMaxAttemp.HasValue)
                            {
                                message = "The number of times is conflict with max attemps that students have done (" + usersMaxAttemp + "), please choose a number greater than : " + usersMaxAttemp;
                                success = false;
                                return;
                            }
                            else
                            {
                                detail.NumberValue = number;
                                detail.TextValue = text;
                            }
                        }
                        else if (detail.SettingType.SettingTypeKey == "RTC" && isactive)
                        {
                            detail.TextValue = GenerateAccessKey(8);
                        }
                        else if (detail.SettingType.SettingTypeKey == "MTP")
                        {
                            if (isactive)
                            {
                                detail.NumberValue = number;
                                var totalScore = test.Questions.TotalScore();
                                if (totalScore.HasValue && number != 0)
                                {
                                    if ((totalScore ?? 0) != number)
                                    {
                                        test.IsRunning = false;
                                    }
                                    else
                                    {
                                        test.IsRunning = true;
                                    }
                                }
                            }
                            else
                            {
                                test.IsRunning = true;
                            }
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
                        return uEmail.Equals(iEmail) && uPass.Equals(ExtensionModel.createHashMD5(iPass));
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
                var checkUser = db.Users.FirstOrDefault(i => i.UserMail == user.UserMail);
                if (checkUser == null)
                {
                    var newUser = new User();
                    newUser.Name = user.Name;
                    newUser.Password = ExtensionModel.createHashMD5(user.Password);
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
                else
                {
                    success = false;
                    message = Constants.DefaultDuplicateEmail;
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
                        var qType = question.QuestionType.Type;
                        if (i.AnswerIDs != null)
                        {
                            if (qType == "Matching")
                            {
                                var coupleIds = i.AnswerIDs.Split(';');
                                var scoreMatching = coupleIds.Sum(k =>
                                {
                                    int? point = null;
                                    var twoIds = k.Split(',');
                                    var firstId = twoIds.ElementAtOrDefault(0);
                                    var secondId = twoIds.ElementAtOrDefault(1);
                                    if (!string.IsNullOrEmpty(firstId))
                                    {
                                        var answerBegin = question.Answers.FirstOrDefault(t => t.AnswerID.ToString() == firstId);
                                        if (answerBegin != null && !string.IsNullOrEmpty(secondId))
                                        {
                                            var answerEnd = answerBegin.AnswerChilds.FirstOrDefault(y => y.AnswerID.ToString() == secondId);
                                            if (answerEnd != null)
                                            {
                                                point = answerBegin.Score < 0 ? 0 : answerBegin.Score;
                                            }
                                        }
                                    }
                                    return point;
                                });
                                i.ChoiceScore = scoreMatching ?? 0;
                            }
                            else
                            {
                                var IDs = i.AnswerIDs.Split(',');
                                var score = IDs.Sum(k =>
                                {
                                    var answer = question.Answers.FirstOrDefault(m => m.AnswerID.ToString() == k);
                                    return answer.Score < 0 ? 0 : answer.Score;
                                });
                                i.ChoiceScore = score ?? 0;
                            }

                        }
                        else
                        {
                            if (qType == "ShortAnswer")
                            {
                                if (!string.IsNullOrEmpty(i.AnswerContent))
                                {
                                    var userAnswers = i.AnswerContent.Split(',');
                                    if (!string.IsNullOrEmpty(question.TextDescription))
                                    {
                                        var questionAnswers = question.TextDescription.Split(',');

                                        if (userAnswers.All(o => questionAnswers.Contains(o)))
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
                if (!string.IsNullOrEmpty(userEmail))
                {
                    userEmail = userEmail.Trim();
                    var db = SingletonDb.Instance();
                    var user = db.Users.FirstOrDefault(i => i.UserID == userId);//may be null
                    if (user != null)
                    {
                        var checkUser = db.Users.FirstOrDefault(i => i.UserID != userId && i.UserMail == userEmail);
                        if (checkUser == null)
                        {
                            user.UserMail = userEmail;
                            if (db.SaveChanges() >= 0) //SaveChanges return affected rows
                            {
                                success = true;
                                message = string.Empty;
                            }
                        }
                        else
                        {
                            success = false;
                            message = Constants.DefaultDuplicateEmail;
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
                    user.Name = userName;
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
                var authenUserId = authen.UserId;
                var checkUser = db.Users.FirstOrDefault(i => i.UserID != authenUserId && i.UserMail == profile.UserMail);
                if (checkUser == null)
                {
                    var user = db.Users.FirstOrDefault(i => i.UserID == authenUserId);
                    if (user != null)
                    {
                        user.Name = profile.Name;
                        //user.UserMail = profile.UserMail;
                        user.UserCountry = profile.UserCountry;
                        user.UserPhone = profile.UserPhone;
                        if (!string.IsNullOrEmpty(profile.Password))
                        {
                            user.Password = ExtensionModel.createHashMD5(profile.Password);
                        }
                        if (db.SaveChanges() >= 0)
                        {
                            success = true;
                            message = "Success on save your profile.";
                        }
                    }
                }
                else
                {
                    success = false;
                    message = Constants.DefaultDuplicateEmail;
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
                        pass = pass.Trim();
                        var uPass = user.Password.Trim();
                        ismatch = ExtensionModel.createHashMD5(pass) == uPass;
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
                    var invitationMails = new List<Invitation>();
                    var invitation = new Invitation();
                    var key = GenerateInvitationAccessToken(test.TestID, user.UserID);
                    invitation.AccessToken = key;
                    invitation.Test = test;
                    invitation.Role = userRole;
                    invitation.InvitationDateTime = DateTime.Now;
                    invitation.IsMailSent = false;
                    user.Invitations.Add(invitation);
                    invitationMails.Add(invitation);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        if (OnRenderPartialViewToString != null)
                        {
                            generatedHtml = OnRenderPartialViewToString.Invoke(user);
                        }
                        var authen = AuthenticationSessionModel.Instance();
                        var userid = authen.UserId;
                        IUserMailer userMailer = new UserMailer();
                        userMailer.OnAcknowledInvitationEmail += (init, sent, unsent, list) =>
                        {
                            if (list.Count > 0)
                            {
                                list.ForEach(i =>
                                {
                                    var invite = db.Invitations.FirstOrDefault(k => k.InvitationID == i);
                                    if (invite != null)
                                    {
                                        invite.IsMailSent = true;
                                    }
                                });
                                db.SaveChanges();
                            }
                            var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                            context.Clients.All.R_AcknowledgeEmailCallback(userid, init, sent, unsent, list);
                        };

                        userMailer.InviteUsers(invitationMails);
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
                    var invitationMails = new List<Invitation>();
                    var invitation = new Invitation();
                    var key = GenerateInvitationAccessToken(test.TestID, user.UserID);
                    invitation.AccessToken = key;
                    invitation.Test = test;
                    invitation.Role = userRole;
                    invitation.InvitationDateTime = DateTime.Now;
                    invitation.IsMailSent = false;
                    user.Invitations.Add(invitation);
                    invitationMails.Add(invitation);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        if (OnRenderPartialViewToString != null)
                        {
                            generatedHtml = OnRenderPartialViewToString.Invoke(user);
                        }
                        var authen = AuthenticationSessionModel.Instance();
                        var userid = authen.UserId;
                        IUserMailer userMailer = new UserMailer();
                        userMailer.OnAcknowledInvitationEmail += (init, sent, unsent, list) =>
                        {
                            if (list.Count > 0)
                            {
                                list.ForEach(i =>
                                {
                                    var invite = db.Invitations.FirstOrDefault(k => k.InvitationID == i);
                                    if (invite != null)
                                    {
                                        invite.IsMailSent = true;
                                    }
                                });
                                db.SaveChanges();
                            }
                            var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                            context.Clients.All.R_AcknowledgeEmailCallback(userid, init, sent, unsent, list);
                        };

                        userMailer.InviteUsers(invitationMails);
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
                if (test != null)
                {
                    var newTitle = "COPY: " + test.TestTitle;
                    var duplicatedTestByName = db.Tests.FirstOrDefault(i => i.TestTitle.Trim() == newTitle.Trim());
                    if (duplicatedTestByName == null)
                    {
                        var newTest = new Test();
                        newTest.TestTitle = newTitle;
                        newTest.IsRunning = test.IsRunning;
                        newTest.IsComplete = test.IsComplete;
                        newTest.CreatedDateTime = DateTime.Now;
                        newTest.CreatedUserID = authen.UserId;
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

                        foreach (var i in test.Questions)
                        {
                            var question = new Question();
                            question.ImageUrl = i.ImageUrl;
                            question.LabelOrder = i.LabelOrder;
                            question.NoneChoiceScore = i.NoneChoiceScore;
                            question.QuestionTitle = i.QuestionTitle;
                            question.QuestionType = i.QuestionType;
                            question.SerialOrder = i.SerialOrder;
                            foreach (var k in i.TagInQuestions)
                            {
                                var tagInquestion = new TagInQuestion();
                                tagInquestion.Tag = k.Tag;
                                tagInquestion.SerialOrder = k.SerialOrder;
                                question.TagInQuestions.Add(tagInquestion);
                            }
                            question.TextDescription = i.TextDescription;
                            foreach (var k in i.Answers)
                            {
                                var answer = new Answer();
                                answer.AnswerContent = k.AnswerContent;
                                answer.IsRight = k.IsRight;
                                answer.Score = k.Score;
                                answer.SerialOrder = k.SerialOrder;
                                answer.DependencyAnswerID = k.DependencyAnswerID;
                                foreach (var item in k.AnswerChilds)
                                {
                                    var ans = new Answer();
                                    ans.AnswerContent = item.AnswerContent;
                                    ans.IsRight = item.IsRight;
                                    ans.Score = item.Score;
                                    ans.SerialOrder = item.SerialOrder;
                                    ans.DependencyAnswerID = item.DependencyAnswerID;
                                    answer.AnswerChilds.Add(ans);
                                    question.Answers.Add(ans);
                                }
                                question.Answers.Add(answer);
                            }
                            newTest.Questions.Add(question);
                        }
                        db.Tests.Add(newTest);
                        if (db.SaveChanges() > 0)
                        {
                            newId = newTest.TestID;
                            success = true;
                            message = "Duplicate test successful.";
                        }
                    }
                    else
                    {
                        success = false;
                        message = "The name generated of this test after copy was duplicated, try change current name of this test.";
                    }
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
                            var lowTagName = tagname.Trim().ToLower();
                            var existTag = db.Tags.FirstOrDefault(j => j.TagName.Trim().ToLower() == lowTagName);
                            if (existTag != null)
                            {
                                tag = existTag;
                            }
                            else
                            {
                                tag = new Tag();
                                tag.TagName = tagname;
                            }
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
                            var lowTagName = tagname.Trim().ToLower();
                            var existTag = db.Tags.FirstOrDefault(j => j.TagName.Trim().ToLower() == lowTagName);
                            if (existTag != null)
                            {
                                tag = existTag;
                            }
                            else
                            {
                                tag = new Tag();
                                tag.TagName = tagname;
                            }
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

                        //update running status
                        var test = question.Test;
                        var totalScore = test.Questions.TotalScore();
                        var settingDetail = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "MTP");
                        if (settingDetail != null)
                        {
                            if (settingDetail.IsActive)
                            {
                                if (totalScore.HasValue && settingDetail.NumberValue.HasValue)
                                {
                                    if ((totalScore ?? 0) != (settingDetail.NumberValue ?? 0))
                                    {
                                        test.IsRunning = false;
                                    }
                                    else
                                    {
                                        test.IsRunning = true;
                                    }
                                }
                            }
                        }

                        if (db.SaveChanges() >= 0)
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
        public void Index_TestListTab(string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                success = true;
                if (OnRenderPartialViewToString != null)
                {
                    var testList = new TestList(db.Tests,role);
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
        public void UserCommentFeedBack(int testid, string fbDetail, string role)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var userId = authen.UserId;
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid); //#1
                if (test != null)
                {
                    //new a FeedBack
                    var feedback = new FeedBack();
                    //assign attribute
                    feedback.UserID = userId;
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
                            if (!string.IsNullOrEmpty(role))
                            {

                                var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                                switch (role)
                                {
                                    case "StudentAndTeacher":
                                        context.Clients.All.R_studentAndTeacherCommentFeedback(testid, generatedHtml);
                                        break;
                                    case "TeacherAndTeacher":
                                        context.Clients.All.R_teacherAndTeacherCommentFeedback(testid, generatedHtml);
                                        break;
                                    default:
                                        break;
                                }
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
        public void UserReplyFeedBack(int testid, int parentFeedBackId, string replyDetail, string role)
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
                            if (!string.IsNullOrEmpty(role))
                            {

                                var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                                switch (role)
                                {
                                    case "StudentAndTeacher":
                                        context.Clients.All.R_studentAndTeacherReplyFeedback(testid, parentFeedBackId, generatedHtml);
                                        break;
                                    case "TeacherAndTeacher":
                                        context.Clients.All.R_teacherAndTeacherReplyFeedback(testid, parentFeedBackId, generatedHtml);
                                        break;
                                    default:
                                        break;
                                }
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
            return key;
        }
        private string GenerateUserToken(int userid)
        {
            var keys = new List<Object>();
            keys.Add(userid);
            keys.Add(DateTime.Now.Ticks);
            var key = ExtensionModel.createHashMD5(keys);
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
                                var qType = question.QuestionType.Type;
                                if (i.AnswerIDs != null)
                                {
                                    if (qType == "Matching")
                                    {
                                        var coupleIds = i.AnswerIDs.Split(';');
                                        var scoreMatching = coupleIds.Sum(k =>
                                        {
                                            int? point = null;
                                            var twoIds = k.Split(',');
                                            var firstId = twoIds.ElementAtOrDefault(0);
                                            var secondId = twoIds.ElementAtOrDefault(1);
                                            if (!string.IsNullOrEmpty(firstId))
                                            {
                                                var answerBegin = question.Answers.FirstOrDefault(t => t.AnswerID.ToString() == firstId);
                                                if (answerBegin != null && !string.IsNullOrEmpty(secondId))
                                                {
                                                    var answerEnd = answerBegin.AnswerChilds.FirstOrDefault(y => y.AnswerID.ToString() == secondId);
                                                    if (answerEnd != null)
                                                    {
                                                        point = answerBegin.Score < 0 ? 0 : answerBegin.Score;
                                                    }
                                                }
                                            }
                                            return point;
                                        });
                                        i.ChoiceScore = scoreMatching ?? 0;
                                    }
                                    else
                                    {
                                        var IDs = i.AnswerIDs.Split(',');
                                        var score = IDs.Sum(k =>
                                        {
                                            var answer = question.Answers.FirstOrDefault(m => m.AnswerID.ToString() == k);
                                            return answer.Score < 0 ? 0 : answer.Score;
                                        });
                                        i.ChoiceScore = score;
                                    }
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
                            var responseTest = new ResponseTest(question.Test, new List<int>() { userid });
                            var scoreItem = responseTest.ResponseUserList.FirstOrDefault(i => i.UserID == userid);
                            if (scoreItem != null)
                            {
                                data = scoreItem.UserPercent;
                            }
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
            isregistered = true;
            accessToken = string.Empty;
            try
            {
                var keyDecode = HttpContext.Current.Server.UrlDecode(key);
                var db = SingletonDb.Instance();
                invitation = db.Invitations.FirstOrDefault(i => i.AccessToken == keyDecode);
                if (invitation != null)
                {
                    var user = invitation.User;
                    if (user.IsRegistered)
                    {
                        isregistered = true;
                        accessToken = string.Empty;
                        //anonymous login
                        var authen = AuthenticationSessionModel.Instance();
                        authen.IsCookieEnable = true;
                        authen.UserId = invitation.UserID;
                    }
                    else
                    {
                        isregistered = false;
                        accessToken = user.AccessToken;
                    }
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
                if (test != null)
                {
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

        public void CheckMaxScoreAndTotalScore(int testid, ref TotalAndMaxScore carier)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    carier = new TotalAndMaxScore();
                    carier.TotalScore = test.Questions.TotalScore();
                    var settingDetail = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "MTP");
                    if (settingDetail != null)
                    {
                        carier.MaxScoreSetting = settingDetail.NumberValue ?? 0;
                        carier.IsRunning = test.IsRunning;
                    }
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public User DetailRegister(string key)
        {
            User user = null;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var keyDecode = HttpContext.Current.Server.UrlDecode(key);
                    var db = SingletonDb.Instance();
                    user = db.Users.FirstOrDefault(i => i.AccessToken == keyDecode && !string.IsNullOrEmpty(i.AccessToken));
                }
            }
            catch (Exception)
            {
                user = null;
            }
            return user;
        }
        public void ForgotPassword(string email, string connectionid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var tempEmail = email.Trim();
                    var db = SingletonDb.Instance();
                    var user = db.Users.FirstOrDefault(i => i.UserMail.Trim() == email);
                    if (user != null)
                    {
                        //generate token
                        var token = GenerateUserToken(user.UserID);
                        user.AccessToken = token;
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                            message = "Sending Email.";
                            //send token via email
                            IUserMailer mailer = new UserMailer();
                            mailer.OnForgotPasswordCallback += (isSuccess) =>
                            {
                                if (!string.IsNullOrEmpty(connectionid))
                                {
                                    var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                                    var client = context.Clients.Client(connectionid);
                                    if (client != null)
                                    {
                                        client.R_forgotCallBack(isSuccess);
                                    }
                                }
                            };
                            mailer.ForgotPassword(user);
                        }
                    }
                    else
                    {
                        success = false;
                        message = "Email that you have entered is not exist.";
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void UpdateUserPasswordOnRegisterDetail(int userid, string password)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                if (user != null)
                {
                    user.Password = ExtensionModel.createHashMD5(password);
                    user.AccessToken = null;
                    user.IsRegistered = true;
                    if (db.SaveChanges() > 0)
                    {
                        var authen = AuthenticationSessionModel.Instance();
                        authen.IsCookieEnable = true;
                        authen.UserId = user.UserID;
                        success = true;
                        message = Constants.DefaultSuccessMessage;
                    }
                }
            }
            catch (Exception)
            {
                success = true;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void Index_OverViewTab(string type, List<int> userids, List<int> testids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var authen = AuthenticationSessionModel.Instance();
                var user = authen.User;
                var tests = user.Tests;
                OverviewScoreTests overview = null;
                if (userids == null || testids == null)
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        overview = new OverviewScoreTests(tests);
                    }
                    else if (type == "overview")
                    {
                        overview = new OverviewScoreTests(tests, new List<int>(), new List<int>());
                    }
                }
                else
                {
                    overview = new OverviewScoreTests(tests, userids, testids);
                }

                if (string.IsNullOrEmpty(type))
                {
                    if (OnRenderPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderPartialViewToString.Invoke(overview);
                    }
                }
                else if (type == "overview")
                {
                    if (OnRenderSubPartialViewToString != null)
                    {
                        success = true;
                        generatedHtml = OnRenderSubPartialViewToString.Invoke(overview);
                    }
                }

            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void InviteUserOutSide(int testid, string email, string name)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    email = email.Trim();
                    var db = SingletonDb.Instance();
                    var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                    if (test != null)
                    {
                        //check to see this email exist or not
                        var checkUser = db.Users.FirstOrDefault(i => i.UserMail.Trim() == email);
                        if (checkUser == null)
                        {
                            //create a user and assign them NOT isregister
                            var user = new User();
                            user.UserMail = email;
                            user.IsRegistered = false;
                            if (!string.IsNullOrEmpty(name))
                            {
                                user.Name = name;
                            }
                            else
                            {
                                user.Name = string.Empty;
                            }
                            //generate token
                            var token = GenerateUserToken(user.UserID);
                            user.AccessToken = token;
                            db.Users.Add(user);
                            if (db.SaveChanges() > 0)
                            {
                                success = true;
                                message = Constants.DefaultSuccessMessage;
                                if (OnRenderPartialViewToString != null)
                                {
                                    generatedHtml = OnRenderPartialViewToString.Invoke(user);
                                }
                            }
                        }
                        else
                        {
                            success = false;
                            message = Constants.DefaultDuplicateEmail;
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
        public void RemoveNonRegisteredUser(int userid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var user = db.Users.FirstOrDefault(i => i.UserID == userid);
                if (user != null)
                {
                    if (!user.IsRegistered)
                    {
                        user.FeedBacks.ToList().ForEach(i => db.FeedBacks.Remove(i));
                        user.Invitations.ToList().ForEach(i => db.Invitations.Remove(i));
                        user.Tests.ToList().ForEach(i => db.Tests.Remove(i));
                        user.UserInTests.ToList().ForEach(i => db.UserInTests.Remove(i));
                        db.Users.Remove(user);
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
        public Test NewTest(int testid)
        {
            Test test = null;
            IsHavePermission = false;
            try
            {
                var db = SingletonDb.Instance();
                test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                var authen = AuthenticationSessionModel.Instance();
                var curUserId = authen.UserId;
                if (curUserId == test.CreatedUserID)
                {
                    IsHavePermission = true;
                }
                var invitation = test.Invitations.FirstOrDefault(i => i.UserID == curUserId);
                if (invitation != null)
                {
                    var role = invitation.Role;
                    if (role != null)
                    {
                        if (role.RoleDescription == "Teacher")
                        {
                            IsHavePermission = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                test = null;
                IsHavePermission = false;
            }
            return test;
        }
        public ExcelPackage AllScoreToExcel(List<int> userids, List<int> testids)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            ExcelPackage excel = null;
            try
            {
                var authen = AuthenticationSessionModel.Instance();
                var tests = authen.User.Tests;
                OverviewScoreTests overview = null;
                if (userids == null || testids == null)
                {
                    overview = new OverviewScoreTests(tests);
                }
                else
                {
                    overview = new OverviewScoreTests(tests, userids, testids);
                }
                excel = overview.ToExcelPackage();
            }
            catch (Exception)
            {
                success = true;
                message = Constants.DefaultExceptionMessage;
                excel = null;
            }
            return excel;
        }
        public void DeleteImage(int questionid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (question != null)
                {
                    var imageUrl = question.ImageUrl;
                    question.ImageUrl = string.Empty;
                    //delete in server
                    var path = HttpContext.Current.Server.MapPath(imageUrl);

                    if (db.SaveChanges() > 0)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        success = true;
                        message = Constants.DefaultSuccessMessage;
                        if (OnRenderPartialViewToString != null)
                        {
                            generatedHtml = OnRenderPartialViewToString.Invoke(question);
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
        public void DeleteTestPermanent(int testid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var feedBacks = test.FeedBacks.ToList();
                    feedBacks.ForEach(i =>
                    {
                        db.FeedBacks.Remove(i);
                    });
                    var invitations = test.Invitations.ToList();
                    invitations.ForEach(i =>
                    {
                        db.Invitations.Remove(i);
                    });
                    var questions = test.Questions.ToList();
                    questions.ForEach(i =>
                    {
                        i.Answers.ToList().ForEach(k => db.Answers.Remove(k));
                        i.TagInQuestions.ToList().ForEach(k => db.TagInQuestions.Remove(k));
                        i.UserInTestDetails.ToList().ForEach(k => db.UserInTestDetails.Remove(k));
                        db.Questions.Remove(i);
                    });
                    test.TagInTests.ToList().ForEach(i => db.TagInTests.Remove(i));
                    test.UserInTests.ToList().ForEach(i => db.UserInTests.Remove(i));
                    var settingConfig = test.SettingConfig;
                    if (settingConfig.SettingConfigID != Constants.DefaultSettingConfigId && settingConfig.Tests.Count <= 1)
                    {
                        settingConfig.SettingConfigDetails.ToList().ForEach(i => db.SettingConfigDetails.Remove(i));
                        db.SettingConfigs.Remove(settingConfig);
                    }
                    db.Tests.Remove(test);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                        message = Constants.DefaultSuccessMessage;
                        var authen = AuthenticationSessionModel.Instance();
                        var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                        context.Clients.All.R_deleteTestPermanent(test.TestID, authen.User.UserMail);
                    }
                }
                else
                {
                    success = false;
                    message = "This Test not exist.";
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void SearchUserItems(int testid, string term, string type)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    List<IUserItem> userItems = null;
                    switch (type)
                    {
                        case "Response":
                            userItems = new List<IUserItem>();
                            var resTest = new ResponseTest(test);
                            resTest.ResponseUserList.ForEach(i =>
                            {
                                userItems.Add(i);
                            });
                            break;
                        case "Score":
                            userItems = new List<IUserItem>();
                            var scoreTest = new ScoreTest(test);
                            scoreTest.ScoreUserList.ForEach(i =>
                            {
                                userItems.Add(i);
                            });
                            break;
                        default:
                            break;
                    }
                    if (userItems != null)
                    {
                        success = true;
                        if (!string.IsNullOrEmpty(term))
                        {
                            var termLower = term.Trim().ToLower();
                            userItems = userItems.Where(i =>
                            {
                                var result = false;
                                if (!string.IsNullOrEmpty(i.UserLabel))
                                {
                                    if (i.UserLabel.ToLower().Contains(termLower))
                                    {
                                        result = true;
                                    }
                                }
                                return result;
                            }).ToList();
                        }
                        if (OnRenderPartialViewToString != null)
                        {
                            userItems.ForEach(i =>
                            {
                                var html = OnRenderPartialViewToString.Invoke(i);
                                resultlist.Add(html);
                            });
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
        public void CreateUsers(List<User> users, string type)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            var createdResults = new List<bool>();
            try
            {
                var db = SingletonDb.Instance();
                var usersImported = db.Users.ToList();
                var importeds = new List<User>();
                users.ForEach(u =>
                {
                    if (!string.IsNullOrEmpty(u.Name) && !string.IsNullOrEmpty(u.UserMail))
                    {
                        var email = u.UserMail.Trim();
                        var name = u.Name.Trim();
                        var checkUser = usersImported.FirstOrDefault(i => i.UserMail.Trim() == email);
                        if (checkUser == null)
                        {
                            var user = new User();
                            user.UserMail = email;
                            user.Name = name;
                            user.IsRegistered = false;
                            //generate token
                            var token = GenerateUserToken(user.UserID);
                            user.AccessToken = token;
                            db.Users.Add(user);
                            usersImported.Add(user);
                            createdResults.Add(true);
                            importeds.Add(user);
                        }
                        else
                        {
                            createdResults.Add(false);
                        }
                    }
                });

                if (db.SaveChanges() >= 0)
                {
                    success = true;
                    var created = createdResults.Count(i => i);
                    var duplicated = createdResults.Count(i => !i);
                    message = "Initial: " + users.Count + " users .Created " + created + ". Duplicated " + duplicated;
                    switch (type)
                    {
                        case "Import":
                            if (OnRenderPartialViewToString != null)
                            {
                                generatedHtml = OnRenderPartialViewToString.Invoke(importeds);
                            }
                            break;
                        case "Create":
                            if (OnRenderSubPartialViewToString != null)
                            {
                                importeds.ForEach(i =>
                                {
                                    var html = OnRenderSubPartialViewToString.Invoke(i);
                                    resultlist.Add(html);
                                });
                            }
                            break;
                        default: //import
                            if (OnRenderPartialViewToString != null)
                            {
                                generatedHtml = OnRenderPartialViewToString.Invoke(importeds);
                            }
                            break;
                    }
                    //send email
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public bool MarkAsComplete(int testid)
        {
            var isComplete = false;
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
                var test = db.Tests.FirstOrDefault(i => i.TestID == testid);
                if (test != null)
                {
                    var isHaveTitle = !string.IsNullOrEmpty(test.TestTitle);
                    var questions = test.Questions.Where(i => i.QuestionType.Type != "Image");
                    var qCount = questions.Count() > 0;
                    var isCompleteQuestions = questions.All(i =>
                    {
                        var complete = false;
                        if (i != null)
                        {
                            var type = i.QuestionType.Type;
                            if (!string.IsNullOrEmpty(i.QuestionTitle))
                            {
                                if (i.Answers.Count == 0)
                                {
                                    complete = true;
                                }
                                else
                                {
                                    var completeAnswerContent = i.Answers.All(k => !string.IsNullOrEmpty(k.AnswerContent));
                                    var choosen = true;
                                    if (type == "Multiple" || type == "Radio")
                                    {
                                        choosen = i.Answers.Any(o => o.IsRight);
                                    }
                                    complete = completeAnswerContent && choosen;
                                }
                            }
                        }
                        return complete;
                    });
                    test.IsComplete = isHaveTitle && qCount && isCompleteQuestions;
                    isComplete = test.IsComplete;
                    if (db.SaveChanges() >= 0)
                    {
                        success = true;
                        message = Constants.DefaultSuccessMessage;
                    }
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
                isComplete = false;
            }
            return isComplete;
        }
    }
}