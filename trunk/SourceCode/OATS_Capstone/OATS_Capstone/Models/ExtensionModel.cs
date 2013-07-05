using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public static class ExtensionModel
    {
        public static String ToPercent(this decimal? praction) {
            var percent = String.Empty;
            try
            {
                var per = praction.RoundTwo() * 100;
                percent = String.Format("{0:##}%", per);
            }
            catch (Exception)
            {
                percent="0";
            }
            return percent;
        }
        public static String ToPercent(this decimal praction)
        {
            var percent = String.Empty;
            if (praction == 0) { return "0%"; }
            try
            {
                var per = praction.RoundTwo() * 100;
                percent = String.Format("{0:##}%", per);
            }
            catch (Exception)
            {
                percent = "0%";
            }
            return percent;
        }
        public static decimal? RoundTwo(this decimal? value)
        {
            return Math.Round(value??0, 2);
        }
        public static decimal RoundTwo(this decimal value)
        {
            return Math.Round(value, 2);
        }
        public static decimal? TotalScore(this IEnumerable<Question> questions)
        {
            decimal? score = 0;
            try
            {
                score = questions.Sum(i => 
                {
                    var temp= i.NoneChoiceScore??0 + i.Answers.Sum(k => k.Score??0);
                    return temp;
                });
            }
            catch (Exception)
            {
                score = 0;   
            }
            return score;
        }
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
        public static decimal StandardDeviation(this IEnumerable<decimal> values)
        {
            decimal avg = values.Average();
            return (decimal)Math.Sqrt(values.Average(v => Math.Pow((double)(v - avg), 2)));
        }
        public static string ToDateDefaultFormat(this DateTime datetime)
        {
            return String.Format("{0:dd MMM yyyy}", datetime);
        }
        public static string ToDateDefaultFormat(this DateTime? datetime)
        {
            var s = string.Empty;
            if (datetime.HasValue) { s = String.Format("{0:dd MMM yyyy}", datetime.Value); }
            return s;
        }
        public static IEnumerable<Test> FilterByRecents(this IEnumerable<Test> tests)
        {
            var today = DateTime.Now;
            var recentTests = tests.Where(condition =>
            {
                //when end date is have value and end date earlier today
                var isRecent = false;
                var end = condition.EndDateTime;
                if (end.HasValue)
                {
                    if (end.Value.CompareTo(today) < 0)
                    {
                        isRecent = true;
                    }
                }
                return isRecent;
            });
            return recentTests;
        }
        public static IEnumerable<Test> FilterByRuning(this IEnumerable<Test> tests)
        {
            var today = DateTime.Now;
            var runningTests = tests.Where(condition =>
            {
                var isRunning = false;
                var start = condition.StartDateTime;
                var end = condition.EndDateTime;
                if (end.HasValue)
                {
                    if (start.CompareTo(today) <= 0 && today.CompareTo(end) <= 0)
                    {
                        isRunning = true;
                    }
                }
                else
                {
                    if (start.CompareTo(today) <= 0)
                    {
                        isRunning = true;
                    }
                }
                return isRunning;
            });
            return runningTests;
        }
        public static IEnumerable<Test> FilterByUpcoming(this IEnumerable<Test> tests)
        {
            var today = DateTime.Now;
            var upComingTests = tests.Where(condition =>
            {
                var isUpComing = false;
                var start = condition.StartDateTime;
                if (today.CompareTo(start) < 0)
                {
                    isUpComing = true;
                }
                return isUpComing;
            });
            return upComingTests;
        }
    }

}