using Microsoft.AspNet.SignalR;
using OATS_Capstone.Hubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TugberkUg.MVC.Helpers;

namespace OATS_Capstone.Models
{
    public delegate String OnRenderPartialViewHandler(object model);
    public class CommonService
    {
        public event OnRenderPartialViewHandler OnRenderPartialViewToString;

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
                    users = db.Users.Where(i => !invitedUsers.Contains(i.UserID)).Where(k => k.UserID != authen.UserId).ToList();
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

        public void ReuseQuestionTemplate(int questionid)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            generatedHtml = String.Empty;
            try
            {
                var db = SingletonDb.Instance();
                var question = db.Questions.FirstOrDefault(i => i.QuestionID == questionid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    generatedHtml = OnRenderPartialViewToString.Invoke(question);
                }
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void ReuseSearchQuestionTemplate(int maxrows, string term)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
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
                    search.StartDate = item.StartDateTime;
                    search.EndDate = item.EndDateTime;
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

        public void TestsSearch()
        {

            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                var tests = db.Tests.ToList();
                tests.ForEach(delegate(Test test)
                {
                    var testTemplate = new SearchingTests();
                    testTemplate.Id = test.TestID;
                    testTemplate.TestTitle = test.TestTitle;
                    testTemplate.StartDate = test.StartDateTime;
                    testTemplate.EndDate = test.EndDateTime;
                    resultlist.Add(testTemplate);
                });
                success = true;
                message = String.Empty;
            }
            catch (Exception)
            {

                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }

        public void UsersSearch()
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            resultlist = new List<Object>();
            try
            {
                var db = SingletonDb.Instance();
                var authen = AuthenticationSessionModel.Instance();
                var users = db.Users.ToList();
                users.ForEach(delegate(User user)
                {
                    var userTemplate = new SearchingUsers();
                    userTemplate.UserID = user.UserID;
                    userTemplate.LastName = user.LastName;
                    userTemplate.FirstName = user.FirstName;
                    resultlist.Add(userTemplate);
                });
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
                            invitation.User = user;
                            invitation.Role = userRole;
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
                    var checkIds = new List<int>();
                    var responseTest = new ResponseTest(test, checkIds);
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
                    generatedHtml = OnRenderPartialViewToString.Invoke(test);
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }


        }

        public void NewTest_FeedBackTab(int testid)
        {
            try
            {
                var db = SingletonDb.Instance();
                var feedbacks = db.FeedBacks.Where(i => i.TestID == testid);
                if (OnRenderPartialViewToString != null)
                {
                    success = true;
                    message = Constants.DefaultSuccessMessage;
                    generatedHtml = OnRenderPartialViewToString.Invoke(feedbacks);
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

        public void AddNewQuestion(int testid, string type, string questiontitle, List<Answer> answers, int serialorder, string labelorder, string textdescription)
        {
            success = false;
            generatedHtml = String.Empty;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
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
            catch (Exception)
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
                        tags.ForEach(k => question.TagInQuestions.Remove(k));
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
        }
        public void UpdateAnswer(List<Answer> answers)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
            try
            {
                var db = SingletonDb.Instance();
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
        }
        public void UpdateQuestionType(int questionid, string type)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
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
        public void UpdateSettings(int testid, String settingKey, bool isactive)
        {
            success = false;
            message = Constants.DefaultProblemMessage;
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
                    if (db.SaveChanges() > 0) //SaveChanges return affected rows
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
                    invitation.Test = test;
                    invitation.Role = userRole;
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
                    invitation.Test = test;
                    invitation.Role = userRole;
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
        public void SearchTagsOnTest(int testid, string term)
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
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }
        }
        public void AddTagToTest(int testid, int tagid)
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
                        var id=ids[i];
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
    }
}