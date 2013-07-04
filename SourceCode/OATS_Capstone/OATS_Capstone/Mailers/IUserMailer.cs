using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{ 
    public interface IUserMailer
    {
        void InviteUsers(List<Invitation> invitations);
        //void ReInviteUsers(List<Invitation> invitations);
	}
}