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
    
    public partial class Role
    {
        public Role()
        {
            this.UserRoleMappings = new HashSet<UserRoleMapping>();
        }
    
        public int RoleID { get; set; }
        public string RoleDescription { get; set; }
    
        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
