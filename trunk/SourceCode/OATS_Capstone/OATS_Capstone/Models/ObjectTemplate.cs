using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class TestCalendarObject
    {
        public int id;
        public string testTitle;
        public DateTime startDateTime;
        public DateTime? endDateTime;
    }

    /// <summary>
    /// use for add list of question purpose
    /// </summary>
    public class QuestionItemTemplate {
        public string ClientID { get; set; }
        public Question QuestionItem { get; set; }
    }
}