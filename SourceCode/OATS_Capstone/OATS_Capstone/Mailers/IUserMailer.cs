using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{ 
    public delegate void ForgotPasswordEmailCallbackDelegate(bool isSuccess);
    public interface IUserMailer
    {
        event ForgotPasswordEmailCallbackDelegate OnForgotPasswordCallback;
        void InviteUsers(List<Invitation> invitations);
        void ReInviteUsers(List<Invitation> invitations);
        void ForgotPassword(User user);
	}
}