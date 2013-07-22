using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{ 
    public delegate void EmailCallbackDelegate(bool isSuccess);
    public delegate void AcknowledgeDelegate(int init,int sent,int unsent,List<int> listInvitationIdSent);
    public interface IUserMailer
    {
        event EmailCallbackDelegate OnForgotPasswordCallback;
        event EmailCallbackDelegate OnNotifyNewUserCallback;
        event AcknowledgeDelegate OnAcknowledInvitationEmail;
        void InviteUsers(List<Invitation> invitations);
        void ReInviteUsers(List<Invitation> invitations);
        void ForgotPassword(User user);
        void NofityNewUser(User user,User invitor);
	}
}