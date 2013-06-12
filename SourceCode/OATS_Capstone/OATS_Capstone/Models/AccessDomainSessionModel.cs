using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class AccessDomainSessionModel
    {
        private User _currentUserSubDomain = null;
        public static bool IsValidSubDomain(string subdomain)
        {
            var result = false;
            try
            {
                if(!String.IsNullOrEmpty(subdomain)){
                var db = SingletonDb.Instance();
                var domains = db.Users.Select(k => k.Subdomain);
                return domains.Contains(subdomain.Trim());
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        private static AccessDomainSessionModel _accessDomainSessionModel = null;
        public static AccessDomainSessionModel Instance()
        {
            if (_accessDomainSessionModel == null)
            {
                _accessDomainSessionModel = new AccessDomainSessionModel();
            }

            return _accessDomainSessionModel;
        }
        public static void ClearSession()
        {
            HttpContext.Current.Session.Remove("subdomain");
        }
        public String CurrentSubdomain
        {
            get
            {
                var result = String.Empty;
                if (HttpContext.Current.Session["subdomain"] != null)
                { result = HttpContext.Current.Session["subdomain"].ToString(); }
                return result;
            }
            set
            {
                HttpContext.Current.Session["subdomain"] = value;
                if (!string.IsNullOrEmpty(value))
                {
                    var db = SingletonDb.Instance();
                    var user = db.Users.FirstOrDefault(i => i.Subdomain.Trim().ToLower() == value.Trim().ToLower());
                    if (user != null)
                    {
                        _currentUserSubDomain = user;
                    }
                }
            }
        }
        public bool IsNewSession
        {
            get { return HttpContext.Current.Session.IsNewSession; }
        }
        public User CurrentOwnerDomain
        {
            get
            {
                return _currentUserSubDomain;
            }
        }
        public List<User> StudentsInThisDomain
        {
            get
            {
                var students = new List<User>();
                if (CurrentOwnerDomain != null)
                {
                    var list = CurrentOwnerDomain.OwnerUser_UserRoleMappings.Where(t => t.Role.RoleDescription == "Student").Select(k => k.ClientUser);
                    students = list.ToList();
                }
                return students;
            }
        }
        public List<User> TeachersInThisDomain
        {
            get
            {
                var students = new List<User>();
                if (CurrentOwnerDomain != null)
                {
                    var list = CurrentOwnerDomain.OwnerUser_UserRoleMappings.Where(t => t.Role.RoleDescription == "Teacher").Select(k => k.ClientUser);
                    students = list.ToList();
                }
                return students;
            }
        }
    }
}