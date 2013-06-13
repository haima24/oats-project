//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OATS_Capstone.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.FeedBacks = new HashSet<FeedBack>();
            this.Invitations = new HashSet<Invitation>();
            this.Tests = new HashSet<Test>();
            this.UserInTests = new HashSet<UserInTest>();
            this.ClientUser_UserRoleMappings = new HashSet<UserRoleMapping>();
            this.OwnerUser_UserRoleMappings = new HashSet<UserRoleMapping>();
        }
    
        public int UserID { get; set; }
        public string Password { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string UserCountry { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string Subdomain { get; set; }
    
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<UserInTest> UserInTests { get; set; }
        public virtual ICollection<UserRoleMapping> ClientUser_UserRoleMappings { get; set; }
        public virtual ICollection<UserRoleMapping> OwnerUser_UserRoleMappings { get; set; }
    }
}
