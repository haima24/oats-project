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
    
    public partial class FeedBack
    {
        public int FeedBackID { get; set; }
        public int UserID { get; set; }
        public int TestID { get; set; }
        public string FeedBackDetail { get; set; }
        public Nullable<System.DateTime> FeedBackDateTime { get; set; }
    
        public virtual Test Test { get; set; }
        public virtual User User { get; set; }
    }
}