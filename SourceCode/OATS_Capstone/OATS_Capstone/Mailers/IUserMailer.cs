using Mvc.Mailer;
using System.Collections.Generic;

namespace OATS_Capstone.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage InviteUser(IEnumerable<string> list);
	}
}