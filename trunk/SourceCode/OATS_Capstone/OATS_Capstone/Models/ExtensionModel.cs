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
                var per = praction * 100;
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
                var per = praction * 100;
                percent = String.Format("{0:##}%", per);
            }
            catch (Exception)
            {
                percent = "0%";
            }
            return percent;
        }
        public static decimal? TotalScore(this IEnumerable<Question> questions)
        {
            decimal? score = 0;
            try
            {
                score = questions.Sum(i => 
                {
                    var temp= i.NoneChoiceScore??0 + i.Answers.Sum(k => k.Score);
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
    }

}