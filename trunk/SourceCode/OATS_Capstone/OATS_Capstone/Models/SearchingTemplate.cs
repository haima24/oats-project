using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class SearchingTests
    {
        public int Id;
        public string TestTitle;
        public DateTime StartDate;
        public DateTime? EndDate;
    }

    public class SearchingUsers
    {
        public int UserID;
        public int? RoleID;
        public string LastName;
        public string FirstName;
    }

}