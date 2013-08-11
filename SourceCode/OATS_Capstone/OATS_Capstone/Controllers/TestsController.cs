﻿using Microsoft.AspNet.SignalR;
using OATS_Capstone.Hubs;
using OATS_Capstone.Mailers;
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

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files, int questionid)
        {
            var service = new CommonService();
            service.UploadFiles(files, questionid);
            return this.Json(new { service.message, service.success });
        }

        public JsonResult RemoveUser(int testid, int userid)
        {
            var common = new CommonService();
            common.RemoveUser(testid, userid);
            return Json(new { common.success, common.message });
        }

        public JsonResult ModalPopupUser(int testid, string role = "", string term = "")
        {
            var common = new CommonService();
            common.OnRenderSubPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Invitation_User_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.OnRenderPartialViewToString += (model) =>
            {
                ViewBag.Role = role;
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Assign_Tests_For_Users", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.ModalPopupUser(testid, term);
            return Json(new { common.message, common.success, common.generatedHtml, common.resultlist });
        }

        public JsonResult ModalRemovePopupUser(int testid, string role, string term = "")
        {
            var common = new CommonService();
            common.OnRenderSubPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Invitation_User_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.Role = role;
                    result = this.RenderPartialViewToString("P_Modal_Remove_Tests_For_User", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;

                }
                return result;
            };
            common.ModalRemovePopupUser(testid, role, term);
            return Json(new { common.success, common.message, common.generatedHtml, common.resultlist });
        }

        public JsonResult ModalReinvitePopupUser(int testid, string role, string term = "")
        {
            var common = new CommonService();
            common.OnRenderSubPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Invitation_User_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.Role = role;
                    result = this.RenderPartialViewToString("P_Modal_Reinvite_Tests_For_Users", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;

                }
                return result;
            };
            common.ModalReinvitePopupUser(testid, role, term);
            return Json(new { common.success, common.message, common.generatedHtml, common.resultlist });
        }

        public JsonResult CloneQuestion(int targetTestID, int questionid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Question_Instance", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.CloneQuestion(targetTestID, questionid);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult ReuseSearchQuestionTemplate(string term, List<int> tagids)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Reuse_Template_Question_Instance", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.ReuseSearchQuestionTemplate(term, tagids);
            return Json(new { common.success, common.message, common.resultlist });
        }
        public JsonResult TestsAssignUserSearch(int userid, List<int> tagids, string letter)
        {
            var common = new CommonService();
            common.TestsAssignUserSearch(userid, tagids, letter);
            return Json(new { common.resultlist, common.success, common.message });
        }


        public JsonResult TestsSearch(string term, List<int> tagids)
        {
            var common = new CommonService();
            common.TestsSearch(term, tagids);
            return Json(new { common.resultlist, common.success, common.message });
        }

        public JsonResult TestsSearchTag(string term)
        {
            var common = new CommonService();
            common.TestsSearchTag(term);
            return Json(new { common.resultlist, common.success, common.message });
        }

        public ActionResult Index(string id = "")
        {
            ViewBag.Tab = id;
            return View();
        }
        public ActionResult MakeTest()
        {
            var common = new CommonService();
            var generatedId = common.MakeTest();
            return RedirectToAction("NewTest", new { id = generatedId });
        }
        public ActionResult NewTest(int id, string tab = "")
        {
            var common = new CommonService();
            var test = common.NewTest(id);
            ActionResult action = null;
            if (common.IsHavePermission && test != null)
            {
                ViewBag.Tab = tab;
                action = View(test);
            }
            else
            {
                action = View("ErrorDetail", (object)Constants.DefaultNoPermission);
            }
            return action;
        }
        public JsonResult AddUserToInvitationTest(int testid, int count, List<int> userids, string role)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_InvitationTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.OnRenderSubPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_InvitationTab_Invitation_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.AddUserToInvitationTest(testid, count, userids, role);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult ReinviteUserToInvitationTest(int testid, int count, List<int> userids, string role)
        {
            var common = new CommonService();
            common.ReinviteUserToInvitationTest(testid, count, userids);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult RemoveUserToInvitationTest(int testid, int count, List<int> userids)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_InvitationTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.RemoveUserToInvitationTest(testid, count, userids);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult QuestionTypes()
        {
            Object obj = null;
            var success = false;
            var message = Constants.DefaultProblemMessage;
            try
            {
                obj = new
                {
                    imagepreview = this.RenderPartialViewToString("P_RenderPreviewImage"),
                    empty = this.RenderPartialViewToString("P_Type_Empty_Template")
                };
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { obj, message, success });
        }
        public JsonResult NewTest_ContentTab(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
                {
                    var result = String.Empty;
                    try
                    {
                        result = this.RenderPartialViewToString("P_ContentTab", model);
                    }
                    catch (Exception)
                    {
                        common.success = false;
                        common.message = Constants.DefaultExceptionMessage;
                    }
                    return result;
                };
            common.NewTest_ContentTab(testid);
            return Json(new { common.generatedHtml, common.success, common.message });
        }
        public ActionResult AccessCode(int id)
        {
            var db = SingletonDb.Instance();
            var test = db.Tests.FirstOrDefault(i => i.TestID == id);
            return View(test);
        }
        public ActionResult DoTest(int id, string accesscode = "", bool check = false)
        {
            var db = SingletonDb.Instance();
            ActionResult action = View("ErrorDetail", (object)Constants.DefaultTestNotExist);

            //NEED: code for checking number of attemp

            var test = db.Tests.FirstOrDefault(i => i.TestID == id);
            if (test != null)
            {
                var testLogic = new TestLogic(test);
                if (testLogic.IsRunning && testLogic.IsActive&&testLogic.IsComplete)
                {
                    if (!testLogic.IsDateTimeNotValid)
                    {
                        if (testLogic.IsInInvitationRoleStudent)
                        {
                            if (!testLogic.IsNumberOfAttempMax)
                            {
                                if (testLogic.RequireAccessCodeSetting != null)
                                {
                                    if (check)
                                    {
                                        if (testLogic.RequireAccessCodeSetting.IsActive)
                                        {
                                            if (testLogic.RequireAccessCodeSetting.TextValue == accesscode)
                                            {
                                                action = View(testLogic);
                                            }
                                            else
                                            {
                                                action = RedirectToAction("AccessCode", new { id = id });
                                            }
                                        }
                                        else
                                        {
                                            action = View(testLogic);
                                        }
                                    }
                                    else
                                    {
                                        action = RedirectToAction("AccessCode", new { id = id });
                                    }
                                }
                            }
                            else
                            {
                                action = View("ErrorDetail", (object)Constants.DefaultMaxOfAttemp);
                            }
                        }
                        else
                        {
                            action = View("ErrorDetail", (object)Constants.DefaultNoPermission);
                        }
                    }
                    else
                    {
                        action = View("ErrorDetail", (object)Constants.DefaultTestInInvalidDateTime);
                    }
                }
                else
                {
                    action = View("ErrorDetail", (object)Constants.DefaultTestNotRunningOrActive);
                }
            }

            return action;
        }
        public ActionResult AnonymousDoTest(string id)
        {
            ActionResult action = RedirectToActionPermanent("Index", "Account");
            try
            {
                var common = new CommonService();
                var invitation = common.AnonymousDoTest(id);
                if (invitation != null)
                {
                    if (common.isregistered)
                    {
                        action = RedirectToActionPermanent("DoTest", new { id = invitation.TestID });
                    }
                    else
                    {
                        action = RedirectToActionPermanent("DetailRegister", "Account", new { id = HttpContext.Server.UrlEncode(common.accessToken), forward = Url.Action("DoTest", new { id = invitation.TestID }) });
                    }
                }
            }
            catch (Exception)
            {
                action = RedirectToActionPermanent("Index", "Account");
            }
            return action;

        }
        public JsonResult TestCalendarObjectResult()
        {
            var common = new CommonService();
            common.TestCalendarObjectResult();
            return Json(new { common.success, common.message, common.resultlist });
        }
        public JsonResult Index_CalendarTab()
        {
            var success = false;
            var message = Constants.DefaultProblemMessage;
            var generatedHtml = String.Empty;
            try
            {
                generatedHtml = this.RenderPartialViewToString("P_CalendarTab");
                success = true;
            }
            catch (Exception)
            {
                success = false;
                message = Constants.DefaultExceptionMessage;
            }

            return Json(new { success, message, generatedHtml });

        }
        public JsonResult Index_TestListTab()
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_TestListTab", model);
                }
                catch (Exception)
                {

                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.Index_TestListTab();
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult NewTest_ResponseTab(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.canEdit = true;
                    result = this.RenderPartialViewToString("P_ResponseTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_ResponseTab(testid);
            return Json(new { common.generatedHtml, common.success, common.message });
        }
        public JsonResult NewTest_ScoreTab(int testid, string tab)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.Tab = tab;
                    result = this.RenderPartialViewToString("P_ScoreTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_ScoreTab(testid);
            return Json(new { common.generatedHtml, common.success, common.message });

        }

        public JsonResult NewTest_FeedBackTab(int testid, string feedbacktab)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.Tab = feedbacktab;
                    result = this.RenderPartialViewToString("P_FeedBackTab", model);
                }
                catch (Exception e)
                {
                    throw e;
                }
                return result;
            };
            common.NewTest_FeedBackTab(testid, feedbacktab);
            return Json(new { common.generatedHtml, common.success, common.message });
        }


        public JsonResult NewTest_SettingTab(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_SettingTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_SettingTab(testid);
            return Json(new { common.generatedHtml, common.success, common.message });
        }
        public JsonResult NewTest_InvitationTab(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_InvitationTab", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_InvitationTab(testid);
            return Json(new { common.generatedHtml, common.success, common.message });
        }

        public JsonResult AddNewQuestion(int testid, string type)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
                {
                    var result = string.Empty;
                    try
                    {
                        result = this.RenderPartialViewToString("P_Question_Instance", model);
                    }
                    catch (Exception)
                    {
                        common.success = false;
                        common.message = Constants.DefaultExceptionMessage;

                    }
                    return result;
                };
            common.AddNewQuestion(testid, type);
            return Json(new { common.generatedHtml, common.success, common.message });
        }
        public JsonResult AddListQuestion(int testid, List<Question> listquestion)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Question_Instance", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;

                }
                return result;
            };
            common.AddListQuestion(testid, listquestion);
            return Json(new { common.arraylist, common.success, common.message });
        }
        public JsonResult AddAnswer(int questionid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Question_Instance", model);
                }
                catch (Exception)
                {

                    common.message = Constants.DefaultExceptionMessage;
                    common.success = false;
                }
                return result;
            };
            common.AddAnswer(questionid);
            return Json(new { common.generatedHtml, common.success, common.message });
        }

        public JsonResult ResortQuestions(int count, List<Question> questions)
        {
            var common = new CommonService();
            common.ResortQuestions(count, questions);
            return Json(new { common.success, common.message });
        }
        public JsonResult DeleteQuestion(int questionid)
        {
            var common = new CommonService();
            common.DeleteQuestion(questionid);
            return Json(new { common.success, common.message });
        }
        public JsonResult DeleteAnswer(int answerid)
        {
            var common = new CommonService();
            common.DeleteAnswer(answerid);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateAnswer(List<Answer> answers)
        {
            var common = new CommonService();
            common.UpdateAnswer(answers);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateQuestionTitle(int questionid, string newtext)
        {
            var common = new CommonService();
            common.UpdateQuestionTitle(questionid, newtext);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateQuestionTextDescription(int questionid, string text)
        {
            var common = new CommonService();
            common.UpdateQuestionTextDescription(questionid, text);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateTestTitle(int testid, string text)
        {
            var common = new CommonService();
            common.UpdateTestTitle(testid, text);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateQuestionType(int questionid, string type)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Question_Instance", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.UpdateQuestionType(questionid, type);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult UpdateStartEnd(int testid, DateTime start, DateTime end)
        {
            var common = new CommonService();
            common.UpdateStartEnd(testid, start, end);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateSettings(int testid, String settingKey, bool isactive, int? number, string text)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToStringWithParameter += (model, isOwner) =>
            {
                var result = string.Empty;
                try
                {
                    ViewBag.isOwner = isOwner;
                    result = this.RenderPartialViewToString("P_SettingTab_ConfigDetail", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.UpdateSettings(testid, settingKey, isactive, number, text);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult DeActiveTest(int testid)
        {
            var common = new CommonService();
            common.DeActiveTest(testid);
            return Json(new { common.success, common.message });
        }
        public JsonResult EnableTest(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Inner_Container", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.EnableTest(testid);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult NewTest_ResponseTab_CheckUserIds(int testid, List<int> userids, int count)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.canEdit = true;
                    result = this.RenderPartialViewToString("P_ResponseTab_Inner", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_ResponseTab_CheckUserIds(testid, userids, count);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult NewTest_ScoreTab_CheckUserIds(int testid, List<int> userids, int count, string tab)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    ViewBag.Tab = tab;
                    result = this.RenderPartialViewToString("P_ScoreTab_Inner", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.NewTest_ScoreTab_CheckUserIds(testid, userids, count);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult DuplicateTest(int testid)
        {
            var common = new CommonService();
            var id = common.DuplicateTest(testid);
            return Json(new { common.success, common.message, id });
        }
        public JsonResult SearchTagsOnTest(int testid, string term, int maxrows)
        {
            var common = new CommonService();
            common.SearchTagsOnTest(testid, term, maxrows);
            return Json(new { common.success, common.message, common.resultlist });
        }
        public JsonResult AddTagToTest(int testid, int tagid, string tagname)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Tag_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.AddTagToTest(testid, tagid, tagname);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult RemoveTagToTest(int testid, int tagid)
        {
            var common = new CommonService();
            common.RemoveTagToTest(testid, tagid);
            return Json(new { common.success, common.message });
        }
        public JsonResult SortTagToTest(int testid, List<int> ids)
        {
            var common = new CommonService();
            common.SortTagToTest(testid, ids);
            return Json(new { common.success, common.message });
        }
        public JsonResult SearchTagsOnQuestion(int questionid, string term, int maxrows)
        {
            var common = new CommonService();
            common.SearchTagsOnQuestion(questionid, term, maxrows);
            return Json(new { common.success, common.message, common.resultlist });
        }
        public JsonResult AddTagToQuestion(int questionid, int tagid, string tagname)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Tag_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.AddTagToQuestion(questionid, tagid, tagname);
            return Json(new { common.success, common.message, common.generatedHtml });
        }
        public JsonResult RemoveTagToQuestion(int questionid, int tagid)
        {
            var common = new CommonService();
            common.RemoveTagToQuestion(questionid, tagid);
            return Json(new { common.success, common.message });
        }
        public JsonResult SortTagToQuestion(int questionid, List<int> ids)
        {
            var common = new CommonService();
            common.SortTagToQuestion(questionid, ids);
            return Json(new { common.success, common.message });
        }
        public JsonResult UpdateNoneChoiceScore(int questionid, decimal score)
        {
            var common = new CommonService();
            common.UpdateNoneChoiceScore(questionid, score);
            return Json(new { common.success, common.message });
        }
        public JsonResult SubmitTest(UserInTest userInTest)
        {
            var common = new CommonService();
            common.SubmitTest(userInTest);
            return Json(new { common.success, common.message });
        }
        public JsonResult ModalFeedBackPopup(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_FeedBack_Form", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.ModalFeedBackPopup(testid);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult UserCommentFeedBack(int testid, string fbDetail, string role)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_FeedBack_Form_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.UserCommentFeedBack(testid, fbDetail, role);
            return Json(new { common.success, common.message, common.generatedHtml });
        }


        public JsonResult UserReplyFeedBack(int testid, int parentFeedBackId, string replyDetail, string role)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_FeedBack_Form_Item_Reply", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.UserReplyFeedBack(testid, parentFeedBackId, replyDetail, role);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult UpdateTestIntroduction(int testid, string introduction)
        {
            var common = new CommonService();
            common.UpdateTestIntroduction(testid, introduction);
            return Json(new { common.success, common.message });
        }

        public JsonResult ModalTestHistoryPopup(int testid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Test_History", model);
                }
                catch (Exception)
                {
                    common.message = Constants.DefaultExceptionMessage;
                    common.success = false;
                }
                return result;
            };
            common.ModalTestHistoryPopup(testid);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult UpdateUserNoneChoiceScore(int questionid, int userid, decimal score)
        {
            var common = new CommonService();
            common.UpdateUserNoneChoiceScore(questionid, userid, score);
            return Json(new { common.message, common.success, common.data });
        }

        public ActionResult ScoreToExcel(int testid, List<int> userids)
        {
            ActionResult action = View("ErrorDetail", (object)Constants.DefaultCannotExportFile);
            var common = new CommonService();
            var package = common.ScoreToExcel(testid, userids);
            if (package != null)
            {
                var result = new ExcelResult();
                result.Package = package;
                result.FileName = "OATS_StudentScore_Test_" + testid + ".xlsx";
                action = result;
            }
            return action;
        }

        public JsonResult CheckMaxScoreAndTotalScore(int testid)
        {
            var common = new CommonService();
            TotalAndMaxScore carier = null;
            common.CheckMaxScoreAndTotalScore(testid, ref carier);
            Object json = null;
            if (carier == null)
            {
                json = new { common.success, common.message };
            }
            else
            {
                json = new { common.success, common.message, carier.MaxScoreSetting, carier.TotalScore, carier.IsRunning };
            }
            return Json(json);
        }

        public JsonResult InviteUserOutSide(int testid, string email, string name)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = String.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Modal_Invitation_User_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.InviteUserOutSide(testid, email, name);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public JsonResult Index_OverViewTab(string type, List<int> userids = null, List<int> testids = null)
        {
            var common = new CommonService();
            common.OnRenderSubPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_OverviewTab_Inner", model);
                }
                catch (Exception)
                {
                    common.message = Constants.DefaultExceptionMessage;
                    common.success = false;
                }
                return result;

            };
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_OverviewTab", model);
                }
                catch (Exception)
                {
                    common.message = Constants.DefaultExceptionMessage;
                    common.success = false;
                }
                return result;
            };
            common.Index_OverViewTab(type, userids, testids);
            return Json(new { common.success, common.message, common.generatedHtml });
        }

        public ActionResult AllScoreToExcel(List<int> userids, List<int> testids)
        {
            ActionResult action = View("ErrorDetail", (object)Constants.DefaultCannotExportFile);
            var common = new CommonService();
            var package = common.AllScoreToExcel(userids, testids);
            if (package != null)
            {
                var result = new ExcelResult();
                result.Package = package;
                result.FileName = "OATS_ScoresOfStudent.xlsx";
                action = result;
            }
            return action;

        }

        public JsonResult DeleteImage(int questionid)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_Question_Instance", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.DeleteImage(questionid);
            return Json(new { common.message, common.success, common.generatedHtml });
        }

        public JsonResult DeleteTestPermanent(int testid)
        {
            var common = new CommonService();
            common.DeleteTestPermanent(testid);
            return Json(new { common.success, common.message });
        }
        public JsonResult SearchUserItems(int testid, string term, string type)
        {
            var common = new CommonService();
            common.OnRenderPartialViewToString += (model) =>
            {
                var result = string.Empty;
                try
                {
                    result = this.RenderPartialViewToString("P_User_Item", model);
                }
                catch (Exception)
                {
                    common.success = false;
                    common.message = Constants.DefaultExceptionMessage;
                }
                return result;
            };
            common.SearchUserItems(testid, term, type);
            return Json(new { common.message, common.success, common.resultlist });
        }
        public JsonResult MarkAsComplete(int testid)
        {
            var common = new CommonService();
            var isComplete = common.MarkAsComplete(testid);
            return Json(new { isComplete, common.success, common.message });
        }
    }
}
