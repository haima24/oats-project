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
    
    public partial class TestSchedule
    {
        public int TestDateID { get; set; }
        public int TestID { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual Test Test { get; set; }
    }
}
