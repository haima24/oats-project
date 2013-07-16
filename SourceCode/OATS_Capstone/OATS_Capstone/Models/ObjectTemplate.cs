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
    public class QuestionItemTemplate
    {
        public string ClientID { get; set; }
        public Question QuestionItem { get; set; }
    }

    public class TotalAndMaxScore
    {
        private decimal? totalScore = 0;

        public decimal? TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        private decimal maxScoreSetting = 0;

        public decimal MaxScoreSetting
        {
            get { return maxScoreSetting; }
            set { maxScoreSetting = value; }
        }

        private bool isRunning = false;

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }


    }
}