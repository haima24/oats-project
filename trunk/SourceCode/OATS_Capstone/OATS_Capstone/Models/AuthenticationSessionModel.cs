using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class AuthenticationSessionModel
    {
        private bool _isCookieEnable = false;

        public bool IsCookieEnable
        {
            get { return _isCookieEnable; }
            set { _isCookieEnable = value; }
        }
        private static AuthenticationSessionModel _authenticationSessionModel = null;
        public static AuthenticationSessionModel Instance()
        {
            if (_authenticationSessionModel == null)
            {
                _authenticationSessionModel = new AuthenticationSessionModel();
            }

            return _authenticationSessionModel;
        }
        private static void ClearCookie(string key)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var response = httpContext.Response;

            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };
            response.Cookies.Set(cookie);
        }
        private static bool IsHaveCookie(String key)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var request = httpContext.Request;
            return request.Cookies.AllKeys.Contains(key);
        }
        private static String GetCookie(String key)
        {
            var result = String.Empty;
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            if (httpContext.Request.Cookies.AllKeys.Contains(key))
            {
                result = httpContext.Request.Cookies[key].Value;
            }
            return result;
        }
        private static void AddCookie(string key, string value)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var response = httpContext.Response;

            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddDays(7)
            };
            response.Cookies.Set(cookie);
        }
        private static void ClearSession()
        {
            HttpContext.Current.Session.Remove("uid");
        }
        private static void ClearCookieOfAuthentication()
        {
            ClearCookie("uid");
        }
        public static void TerminateAuthentication()
        {
            ClearSession();
            ClearCookieOfAuthentication();
        }
        public int UserId
        {
            get
            {
                var uid = 0;
                if (IsHaveCookie("uid"))
                {
                    var uidString = GetCookie("uid");
                    var tempid = 0;
                    if (int.TryParse(uidString, out tempid))
                    {
                        uid = tempid;
                    }
                }
                if (uid == 0)
                {
                    if (HttpContext.Current.Session["uid"] != null)
                    {
                        uid = (int)HttpContext.Current.Session["uid"];
                    }
                }
                return uid;
            }
            set
            {
                HttpContext.Current.Session["uid"] = value;
                if (_isCookieEnable)
                {
                    AddCookie("uid", value.ToString());
                }
                else
                {
                    ClearCookie("uid");
                }
            }
        }
        public Boolean IsAuthentication
        {
            get { return UserId != 0; }
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