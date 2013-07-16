using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class SearchingTests
    {
        public int Id { get; set; }
        public string TestTitle { get; set; }
        public string DateDescription { get; set; }
        public bool IsCurrentUserOwnTest { get; set; }
        public string Introduction { get; set; }
        public bool IsRunning { get; set; }
    }

    public class SearchingUsers
    {
        public int UserID;
        public int? RoleID;
        public string LastName;
        public string FirstName;
    }

}