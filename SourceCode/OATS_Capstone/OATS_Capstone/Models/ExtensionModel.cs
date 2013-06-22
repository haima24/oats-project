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
    }

}