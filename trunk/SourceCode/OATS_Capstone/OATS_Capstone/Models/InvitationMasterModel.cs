using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class InvitationMasterModel
    {
        public List<Invitation> InvitationList { get; set; }
        public List<User> UserList { get; set; }
    }
}