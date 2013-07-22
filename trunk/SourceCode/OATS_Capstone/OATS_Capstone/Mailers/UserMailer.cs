using Microsoft.AspNet.SignalR;
using Mvc.Mailer;
using OATS_Capstone.Hubs;
using OATS_Capstone.Models;
using System.Collections.Generic;
using System.Linq;

namespace OATS_Capstone.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        private List<int> listSent = null;

        private int tempMailCount = 0;

        private int initMailCount = 0;

        private int sentMailCount = 0;

        private int unSentMailCount = 0;

        public event EmailCallbackDelegate OnForgotPasswordCallback;

        public event EmailCallbackDelegate OnNotifyNewUserCallback;

        public event AcknowledgeDelegate OnAcknowledInvitationEmail;

        public UserMailer()
        {
            MasterName = "_Layout";
            listSent = new List<int>();
        }
        private void AcknowledgeCallback()
        {
            if (tempMailCount == 0)
            {
                if (OnAcknowledInvitationEmail != null) {
                    OnAcknowledInvitationEmail.Invoke(initMailCount, sentMailCount, unSentMailCount, listSent);
                }
            }
        }

        private MvcMailMessage InviteUser(Invitation invitation)
        {

            var teacherName = string.Empty;

            if (invitation.Test != null)
            {
                if (invitation.Test.User != null)
                {
                    teacherName = !string.IsNullOrEmpty(invitation.Test.User.Name) ? invitation.Test.User.Name : invitation.Test.User.UserMail;
                }
            }
            var userName = string.Empty;
            ViewBag.TeacherName = teacherName;
            var email = string.Empty;
            var linkReg = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/Index";
            if (invitation.User != null)
            {
                email = invitation.User.UserMail;
                userName = !string.IsNullOrEmpty(invitation.User.Name) ? invitation.User.Name : invitation.User.UserMail;


                if (!string.IsNullOrEmpty(invitation.User.AccessToken))
                {
                    linkReg = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/DetailRegister/" + CurrentHttpContext.Server.UrlEncode(invitation.User.AccessToken);
                }
            }
            ViewBag.UserName = userName;
            ViewBag.LinkRegister = linkReg;
            var link = string.Empty;
            var anonymousLink = string.Empty;
            if (invitation.Test != null)
            {
                link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Tests/DoTest/" + invitation.Test.TestID;
                anonymousLink = "http://" + CurrentHttpContext.Request.Url.Authority + "/Tests/AnonymousDoTest/" + CurrentHttpContext.Server.UrlEncode(invitation.AccessToken);
            }
            ViewBag.Link = link;
            ViewBag.AnonymousLink = anonymousLink;

            MvcMailMessage obj = null;
            if (invitation.User.IsRegistered)
            {
                obj = Populate(x =>
                {
                    x.Subject = "OATS Invite User";
                    x.ViewName = "InviteUser";
                    x.To.Add(email);
                });
            }
            else
            {
                obj = Populate(x =>
            {
                x.Subject = "OATS Invite User";
                x.ViewName = "InviteNonRegisteredUser";
                x.To.Add(email);
            });
            }
            return obj;
        }
        public virtual void InviteUsers(List<Invitation> invitations)
        {
            initMailCount = invitations.Count;
            tempMailCount = initMailCount;
            invitations.ForEach(i =>
            {
                var client = new SmtpClientWrapperOATS();
                client.OnSendingError += (ex) =>
                {
                    unSentMailCount++;
                    tempMailCount--;
                    AcknowledgeCallback();
                };
                client.SendCompleted += (sender, e) =>
                {
                    if (e.Error != null || e.Cancelled)
                    {
                        unSentMailCount++;
                    }
                    else {
                        sentMailCount++;
                        var invitationIdObj = e.UserState;
                        if (invitationIdObj != null&&invitationIdObj is int) {
                            var invitationId = (int)invitationIdObj;
                            listSent.Add(invitationId);

                        }
                    }
                    tempMailCount--;
                    AcknowledgeCallback();
                };
                this.InviteUser(invitations.FirstOrDefault()).SendAsync(i.InvitationID, client);
            });
        }
        public virtual void ReInviteUsers(List<Invitation> invitations)
        {
            initMailCount = invitations.Count;
            tempMailCount = initMailCount;
            invitations.ForEach(i =>
            {
                var client = new SmtpClientWrapperOATS();
                client.OnSendingError += (ex) =>
                {
                    unSentMailCount++;
                    tempMailCount--;
                    AcknowledgeCallback();
                };
                client.SendCompleted += (sender, e) =>
                {
                    if (e.Error != null || e.Cancelled)
                    {
                        unSentMailCount++;
                    }
                    else
                    {
                        sentMailCount++;
                        var invitationIdObj = e.UserState;
                        if (invitationIdObj != null && invitationIdObj is int)
                        {
                            var invitationId = (int)invitationIdObj;
                            listSent.Add(invitationId);

                        }
                    }
                    tempMailCount--;
                    AcknowledgeCallback();
                };
                this.InviteUser(invitations.FirstOrDefault()).SendAsync(i.InvitationID, client);
            });
        }
        public virtual void ForgotPassword(User user)
        {
            if (user != null)
            {
                var mail = user.UserMail;
                var link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/Index";
                if (!string.IsNullOrEmpty(user.AccessToken))
                {
                    link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/DetailRegister/" + CurrentHttpContext.Server.UrlEncode(user.AccessToken);
                }
                ViewBag.Mail = mail;
                ViewBag.Link = link;

                var mailObj = Populate(x =>
                {
                    x.Subject = "OATS Reset Forgot Password";
                    x.ViewName = "ForgotPassword";
                    x.To.Add(user.UserMail);
                });
                var client = new SmtpClientWrapperOATS();
                client.OnSendingError += (ex) =>
                {
                    if (OnForgotPasswordCallback != null)
                    {
                        OnForgotPasswordCallback.Invoke(false);
                    }
                };
                client.SendCompleted += (sender, e) =>
                {
                    if (OnForgotPasswordCallback != null)
                    {
                        OnForgotPasswordCallback.Invoke(true);
                    }
                };
                mailObj.SendAsync("oats", client);
            }
        }
        public virtual void NofityNewUser(User user, User invitor)
        {
            if (user != null)
            {
                var mail = user.UserMail;
                var link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/Index";
                var invitorName = string.Empty;
                if (invitor != null)
                {
                    if (string.IsNullOrEmpty(invitor.Name))
                    {
                        invitorName = invitor.UserMail;
                    }
                    else {
                        invitorName = invitor.Name + " (Email: "+invitor.UserMail+" )";
                    }
                }
                if (!string.IsNullOrEmpty(user.AccessToken))
                {
                    link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/DetailRegister/" + CurrentHttpContext.Server.UrlEncode(user.AccessToken);
                }
                ViewBag.Invitor = invitorName;
                ViewBag.Mail = mail;
                ViewBag.Link = link;

                var mailObj = Populate(x =>
                {
                    x.Subject = "OATS Notify for New User";
                    x.ViewName = "NotifyNewUser";
                    x.To.Add(user.UserMail);
                });
                var client = new SmtpClientWrapperOATS();
                client.OnSendingError += (ex) =>
                {
                    if (OnNotifyNewUserCallback != null)
                    {
                        OnNotifyNewUserCallback.Invoke(false);
                    }
                };
                client.SendCompleted += (sender, e) =>
                {
                    if (OnNotifyNewUserCallback != null)
                    {
                        OnNotifyNewUserCallback.Invoke(true);
                    }
                };
                mailObj.SendAsync("oats", client);
            }
        }



        
    }
}