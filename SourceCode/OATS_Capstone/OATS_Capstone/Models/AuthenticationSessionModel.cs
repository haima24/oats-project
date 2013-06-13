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
        public int OwnerUserId
        {
            get
            {
                var uid = 0;
                if (HttpContext.Current.Session["owner"] != null)
                {
                    uid = (int)HttpContext.Current.Session["owner"];
                }
                return uid;
            }
            set { HttpContext.Current.Session["owner"] = value; }
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
            get {
                var students = new List<User>();
                if (OwnerUser != null) {
                    var list=OwnerUser.OwnerUser_UserRoleMappings.Where(k => k.Role.RoleDescription == "Student").Select(i => i.ClientUser);
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
        public List<Test> TestsInThisOwner {
            get
            {
                var tests = new List<Test>();
                if (OwnerUser != null)
                {
                    tests=OwnerUser.Tests.ToList();
                }
                return tests;
            }
        }
    }
}