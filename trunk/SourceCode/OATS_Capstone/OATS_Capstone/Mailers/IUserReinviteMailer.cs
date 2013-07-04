using Mvc.Mailer;
using OATS_Capstone.Models;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{
    public interface IUserReinviteMailer
    {
        void ReInviteUsers(List<Invitation> invitations);
    }
}