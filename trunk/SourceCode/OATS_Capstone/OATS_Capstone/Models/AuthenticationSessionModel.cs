using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class AuthenticationSessionModel
    {
        private static AuthenticationSessionModel _authenticationSessionModel = null;
        public static AuthenticationSessionModel Instance()
        {
            if (_authenticationSessionModel == null)
            {
                _authenticationSessionModel = new AuthenticationSessionModel();
            }

            return _authenticationSessionModel;
        }
        public static void ClearSession()
        {
            HttpContext.Current.Session.Remove("uid");
        }

        public int UserId
        {
            get
            {
                var uid = 0;
                if (HttpContext.Current.Session["uid"] != null)
                {
                    uid = (int)HttpContext.Current.Session["uid"];
                }
                return uid;
            }
            set { HttpContext.Current.Session["uid"] = value; }
        }
        public Boolean IsAuthentication
        {
            get { return UserId != 0; }
        }
        public bool IsNewSession
        {
            get { return HttpContext.Current.Session.IsNewSession; }
        }
        public User User
        {
            get
            {
                User user = null;
                var obj = SingletonDb.Instance().Users.FirstOrDefault(i => i.UserID == UserId);
                if (obj != null)
                {
                    user = obj;
                }
                return user;
            }
        }
    }
}