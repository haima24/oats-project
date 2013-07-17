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

        public event ForgotPasswordEmailCallbackDelegate OnForgotPasswordCallback;

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
            if (invitation.User != null)
            {
                email = invitation.User.UserMail;
                userName = invitation.User.FirstName ?? invitation.User.LastName ?? invitation.User.UserMail;
            }
            ViewBag.UserName = userName;
            var link = string.Empty;
            var anonymousLink = string.Empty;
            if (invitation.Test != null)
            {
                link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Tests/DoTest/" + invitation.Test.TestID;
                anonymousLink = "http://" + CurrentHttpContext.Request.Url.Authority + "/Tests/AnonymousDoTest/" + CurrentHttpContext.Server.HtmlEncode(invitation.AccessToken);
            }
            ViewBag.Link = link;
            ViewBag.AnonymousLink = anonymousLink;
            return Populate(x =>
            {
                x.Subject = "OATS Invite User";
                x.ViewName = "InviteUser";
                x.To.Add(email);
            });
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
            invitations.ForEach(i =>
            {
                this.InviteUser(i).Send();
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
                    link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Account/DetailRegister/" + CurrentHttpContext.Server.HtmlEncode(user.AccessToken);
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


    }
}