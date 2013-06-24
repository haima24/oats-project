using Mvc.Mailer;

namespace OATS_Capstone.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
        public virtual MvcMailMessage InviteUser(System.Collections.Generic.IEnumerable<string> list)
        {
            return Populate(x =>
            {
                x.Subject = "Welcome";
                x.ViewName = "Welcome";
                foreach (var item in list)
                {
                    x.To.Add(item);
                }
            });
        }
    }
}