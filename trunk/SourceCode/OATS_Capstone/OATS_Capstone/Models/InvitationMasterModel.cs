using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class InvitationMasterModel
    {
        private List<Invitation> invitationList = null;

        public List<Invitation> InvitationList
        {
            get { return invitationList; }
            set { invitationList = value; }
        }
        private List<User> userList = null;

        public List<User> UserList
        {
            get { return userList; }
            set { userList = value; }
        }
    }
}