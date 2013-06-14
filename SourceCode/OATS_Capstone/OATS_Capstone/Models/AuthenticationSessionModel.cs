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
            HttpContext.Current.Session.Remove("ownerid");
        }
        private static void ClearCookieOfAuthentication()
        {
            ClearCookie("uid");
            ClearCookie("ownerid");
        }
        public static void TerminateAuthentication()
        {
            ClearSession();
            ClearCookieOfAuthentication();
        }
        public int OwnerUserId
        {
            get
            {
                var ownerid = 0;
                if (IsHaveCookie("ownerid"))
                {
                    var owneridString = GetCookie("ownerid");
                    var tempid = 0;
                    if (int.TryParse(owneridString, out tempid))
                    {
                        ownerid = tempid;
                    }
                }
                if (ownerid == 0)
                {
                    if (HttpContext.Current.Session["ownerid"] != null)
                    {
                        ownerid = (int)HttpContext.Current.Session["ownerid"];
                    }
                }
                return ownerid;
            }
            set
            {
                HttpContext.Current.Session["ownerid"] = value;
                if (_isCookieEnable)
                {
                    AddCookie("ownerid", value.ToString());
                }
                else
                {
                    ClearCookie("ownerid");
                }
            }
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
        public User OwnerUser
        {
            get
            {
                User user = null;
                var obj = SingletonDb.Instance().Users.FirstOrDefault(i => i.UserID == OwnerUserId);
                if (obj != null)
                {
                    user = obj;
                }
                return user;
            }
        }
        public bool IsCurrentUserAlsoOwner
        {
            get { return (UserId != 0 && OwnerUserId != 0 && UserId == OwnerUserId); }
        }
        public List<User> StudentsInThisOwner
        {
            get
            {
                var students = new List<User>();
                if (OwnerUser != null)
                {
                    var list = OwnerUser.OwnerUser_UserRoleMappings.Where(k => k.Role.RoleDescription == "Student").Select(i => i.ClientUser);
                    students = list.ToList();
                }
                return students;
            }
        }
        public List<User> TeachersInThisOwner
        {
            get
            {
                var students = new List<User>();
                if (OwnerUser != null)
                {
                    var list = OwnerUser.OwnerUser_UserRoleMappings.Where(k => k.Role.RoleDescription == "Teacher").Select(i => i.ClientUser);
                    students = list.ToList();
                }
                return students;
            }
        }
        public List<Test> TestsInThisOwner
        {
            get
            {
                var tests = new List<Test>();
                if (OwnerUser != null)
                {
                    tests = OwnerUser.Tests.ToList();
                }
                return tests;
            }
        }
        public bool IsStudent
        {
            get
            {
                var result = false;
                if (!IsCurrentUserAlsoOwner)
                {
                    var db = SingletonDb.Instance();
                    var roleMap = db.UserRoleMappings.FirstOrDefault(k => k.ClientUserID == UserId && k.OwnerDomainUserID == OwnerUserId);
                    if (roleMap != null)
                    {
                        result = roleMap.Role.RoleDescription == "Student";
                    }
                }
                return result;
            }
        }
        public bool IsTeacher
        {
            get
            {
                var result = false;
                if (!IsCurrentUserAlsoOwner)
                {
                    var db = SingletonDb.Instance();
                    var roleMap = db.UserRoleMappings.FirstOrDefault(k => k.ClientUserID == UserId && k.OwnerDomainUserID == OwnerUserId);
                    if (roleMap != null)
                    {
                        result = roleMap.Role.RoleDescription == "Teacher";
                    }
                }
                return result;
            }
        }
    }
}