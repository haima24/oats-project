using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
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
            if (invitation.Test!=null)
            {
                link = "http://" + CurrentHttpContext.Request.Url.Authority + "/Tests/DoTest/" + invitation.Test.TestID;
            }
            ViewBag.Link = link;
            return Populate(x =>
            {
                x.Subject = "Invite User Demo Subject";
                x.ViewName = "InviteUser";
                x.To.Add(email);
            });
        }
        public virtual void InviteUsers(List<Invitation> invitations)
        {
            invitations.ForEach(i =>
            {
                this.InviteUser(i).Send();
            });
        }

        public virtual void ReInviteUsers(List<Invitation> invitations)
        {
            invitations.ForEach(i =>
            {
                this.InviteUser(i).Send();
            });
        }
    }
}