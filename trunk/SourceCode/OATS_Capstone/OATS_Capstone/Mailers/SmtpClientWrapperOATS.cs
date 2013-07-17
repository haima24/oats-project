using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Mailers
{
    public delegate void OnSendingErrorDelegate(Exception ex);
    public class SmtpClientWrapperOATS : SmtpClientWrapper
    {
        public event OnSendingErrorDelegate OnSendingError = null;
        public override void SendAsync(System.Net.Mail.MailMessage mailMessage, object userState)
        {
            try
            {
                base.SendAsync(mailMessage, userState);
            }
            catch (Exception ex)
            {
                if (OnSendingError != null) {
                    OnSendingError.Invoke(ex);
                }
            }
        }
    }
}