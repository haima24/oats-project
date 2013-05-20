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
    
    public partial class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Tags = new HashSet<Tag>();
        }
    
        public int QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public int TestID { get; set; }
        public int QuestionTypeID { get; set; }
        public string ImageUrl { get; set; }
        public string TextDescription { get; set; }
        public decimal QuestionScore { get; set; }
    
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual QuestionType QuestionType { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
