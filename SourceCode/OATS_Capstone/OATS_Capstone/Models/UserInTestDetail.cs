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
    
    public partial class UserInTestDetail
    {
        public int UserInTestID { get; set; }
        public int QuestionID { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerIDs { get; set; }
    
        public virtual Question Question { get; set; }
        public virtual UserInTest UserInTest { get; set; }
    }
}