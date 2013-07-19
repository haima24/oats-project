using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{ 
    public delegate void EmailCallbackDelegate(bool isSuccess);
    public interface IUserMailer
    {
        event EmailCallbackDelegate OnForgotPasswordCallback;
        event EmailCallbackDelegate OnNotifyNewUserCallback;
        void InviteUsers(List<Invitation> invitations);
        void ReInviteUsers(List<Invitation> invitations);
        void ForgotPassword(User user);
        void NofityNewUser(User user,User invitor);
	}
}