using Mvc.Mailer;

namespace OATS_Capstone.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
			MvcMailMessage PasswordReset();
	}
}