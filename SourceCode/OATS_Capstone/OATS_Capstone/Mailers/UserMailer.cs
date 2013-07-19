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
        private int ownUserId = 0;

        private int tempMailCount = 0;

        private int initMailCount = 0;

        public int InitMailCount
        {
            get { return initMailCount; }
        }

        private int sentMailCount = 0;

        public int SentMailCount
        {
            get { return sentMailCount; }
        }

        private int unSentMailCount = 0;

        public int UnSentMailCount
        {
            get { return unSentMailCount; }
        }

        public event EmailCallbackDelegate OnForgotPasswordCallback;

        public event EmailCallbackDelegate OnNotifyNewUserCallback;

        public UserMailer(int ownId)
        {
            MasterName = "_Layout";
            ownUserId = ownId;
        }
        public UserMailer()
        {
            MasterName = "_Layout";
        }
        private void AcknowledgeCallback()
        {
            if (tempMailCount == 0)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<GeneralHub>();
                context.Clients.All.R_AcknowledgeEmailCallback(ownUserId, initMailCount, sentMailCount, unSentMailCount);
            }
        }

        private MvcMailMessage InviteUser(Invitation invitation)
        {

            var teacherName = string.Empty;

            if (invitation.Test != null)
            {
                if (invitation.Test.User != null)
                {
                    teacherName = invitation.Test.User.FirstName ?? invitation.Test.User.LastName ?? invitation.Test.User.UserMail;
                }
            }
            var userName = string.Empty;
            ViewBag.TeacherName = teacherName;
            var email = string.Empty;
            var linkReg = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/Index";
            if (invitation.User != null)
            {
                email = invitation.User.UserMail;
                userName = invitation.User.FirstName ?? invitation.User.LastName ?? invitation.User.UserMail;


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
                    sentMailCount++;
                    tempMailCount--;
                    AcknowledgeCallback();
                    //if (e.Error != null || e.Cancelled)
                    //{
                    //}
                };
                this.InviteUser(invitations.FirstOrDefault()).SendAsync("oats", client);
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
                    sentMailCount++;
                    tempMailCount--;
                    AcknowledgeCallback();
                    //if (e.Error != null || e.Cancelled)
                    //{
                    //}
                };
                this.InviteUser(invitations.FirstOrDefault()).SendAsync("oats", client);
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
                    if (string.IsNullOrEmpty(invitor.FirstName) && string.IsNullOrEmpty(invitor.LastName))
                    {
                        invitorName = invitor.UserMail;
                    }
                    else
                    {
                        invitorName = invitor.FirstName ?? invitor.LastName + " (Email: " + invitor.UserMail + " )";
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