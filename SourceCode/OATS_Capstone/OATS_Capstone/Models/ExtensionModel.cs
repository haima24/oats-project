using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public static class ExtensionModel
    {
        public static List<User> ToStudents(this IEnumerable<Invitation> invitations)
        {
            var result = new List<User>();
            try
            {
                var authen = AuthenticationSessionModel.Instance();
                var idsInOwner = authen.StudentsInThisOwner.Select(i => i.UserID).ToList();
                var students = invitations.Select(k => k.User).Where(k => idsInOwner.Contains(k.UserID));
                result = students.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static List<User> ToTeachers(this IEnumerable<Invitation> invitations)
        {
            var result = new List<User>();
            try
            {
                var authen = AuthenticationSessionModel.Instance();
                var idsInOwner = authen.TeachersInThisOwner.Select(i => i.UserID).ToList();
                var teachers = invitations.Select(k => k.User).Where(k => idsInOwner.Contains(k.UserID));
                result = teachers.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static String ToPercent(this decimal? praction) {
            var percent = String.Empty;
            try
            {
                var per = praction * 100;
                percent = String.Format("{0:##}%", per);
            }
            catch (Exception)
            {
                percent="0";
            }
            return percent;
        }
    }
}