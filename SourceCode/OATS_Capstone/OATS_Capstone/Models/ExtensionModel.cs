using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public static class ExtensionModel
    {
        public static String ToPercent(this decimal? praction)
        {
            var percent = String.Empty;
            try
            {
                var per = praction.RoundTwo() * 100;
                percent = String.Format("{0:#0}%", per);
            }
            catch (Exception)
            {
                percent = "0";
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
                percent = String.Format("{0:#0}%", per);
            }
            catch (Exception)
            {
                percent = "0%";
            }
            return percent;
        }
        public static decimal? RoundTwo(this decimal? value)
        {
            return Math.Round(value ?? 0, 2);
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
                    var temp = i.NoneChoiceScore ?? 0 + i.Answers.Sum(k => k.Score ?? 0);
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
            var s = string.Empty;
            if (datetime != null)
            {
                s = String.Format("{0:dd MMM yyyy}", datetime);
            }
            else {
                s = "N/A";
            }
            return s;
        }
        public static string ToDateDefaultFormat(this DateTime? datetime)
        {
            var s = string.Empty;
            if (datetime.HasValue) { s = String.Format("{0:dd MMM yyyy}", datetime.Value); }
            else {
                s = "N/A";
            }
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
        private class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _lambdaComparer;
            private readonly Func<T, int> _lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer) :
                this(lambdaComparer, o => 0)
            {
            }

            public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
            {
                if (lambdaComparer == null)
                    throw new ArgumentNullException("lambdaComparer");
                if (lambdaHash == null)
                    throw new ArgumentNullException("lambdaHash");

                _lambdaComparer = lambdaComparer;
                _lambdaHash = lambdaHash;
            }

            public bool Equals(T x, T y)
            {
                return _lambdaComparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _lambdaHash(obj);
            }
        }
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
        }
        public static bool Contains<TSource>(this IEnumerable<TSource> enumerable,TSource item, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Contains(item,new LambdaComparer<TSource>(comparer));
        }
        public static List<UserInTest> FilterInTestsOnAttempSetting(this IEnumerable<UserInTest> inTests)
        {
            var groups = from i in inTests
                         group i by i.UserID into InTestGroup
                         select new { UserId = InTestGroup.Key, MaxAttend = InTestGroup.Max(i => i.NumberOfAttend) };
            var result = new List<UserInTest>();
            foreach (var item in groups)
            {
                var id = item.UserId;
                var attend = item.MaxAttend;
                var inTest = inTests.FirstOrDefault(i => i.UserID == id && i.NumberOfAttend == attend);
                if (inTest != null)
                {
                    result.Add(inTest);
                }
            }
            return result;
        }
        public static String createHashMD5(IEnumerable<Object> keys)
        {
            // Get a byte array containing the combined password + salt.
            string authDetails = keys.Select(i => i.ToString()).Aggregate((f, n) => f + n);
            byte[] authBytes = System.Text.Encoding.ASCII.GetBytes(authDetails);
            // Use MD5 to compute the hash of the byte array, and return the hash as
            // a Base64-encoded string.
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes = md5.ComputeHash(authBytes);
            string hash = BitConverter.ToString(hashedBytes);
            return hash;
        }
        public static String createHashMD5(string key)
        {
            // Get a byte array containing the combined password + salt.
            string authDetails = key;
            byte[] authBytes = System.Text.Encoding.ASCII.GetBytes(authDetails);
            // Use MD5 to compute the hash of the byte array, and return the hash as
            // a Base64-encoded string.
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes = md5.ComputeHash(authBytes);
            string hash = BitConverter.ToString(hashedBytes);
            return hash;
        }
        public static bool IsTotalScoreEqualMaxScore(this Test test)
        {
            var result = false;
            if (test != null)
            {
                var settingDetail = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "MTP");
                if (settingDetail != null)
                {
                    var total = test.Questions.TotalScore();
                    var max = settingDetail.NumberValue;
                    result = total == max;
                }
            }
            return result;
        }
        public static IEnumerable<Question> RandomQuestion(this IEnumerable<Question> questions)
        {
            //sorting
            var sortedQuestions = questions.OrderBy(i => i.SerialOrder);
            var sortedCount = sortedQuestions.Count();
            var questionsResult = new List<Question>();
            //grouping
            var groupList = new List<List<Question>>();
            var currentList = new List<Question>();
            var isBeginAnchor = false;
            for (int i = 0; i < sortedCount; i++)
            {
                var item = sortedQuestions.ElementAt(i);
                var previousItem = sortedQuestions.ElementAtOrDefault(i - 1);
                if (item.IsTextOrImage())
                {
                    isBeginAnchor = true;
                    var isPreviousIsTextOrImage = false;
                    if (previousItem != null)
                    {
                        if (previousItem.IsTextOrImage())
                        {
                            isPreviousIsTextOrImage = true;
                        }
                    }
                    if (!isPreviousIsTextOrImage)
                    {
                        if (currentList.Count > 0)
                        {
                            groupList.Add(currentList);
                        }
                        currentList = new List<Question>();
                        currentList.Add(item);
                    }
                    else
                    {
                        currentList.Add(item);
                    }
                }
                else
                {
                    if (!isBeginAnchor)
                    {
                        currentList.Add(item);
                        groupList.Add(currentList);
                        currentList = new List<Question>();
                    }
                    else
                    {
                        currentList.Add(item);
                    }
                }

                if ((i + 1) == sortedCount && currentList.Count > 0)
                {
                    var last = groupList.LastOrDefault();
                    if (last != null) {
                        last.AddRange(currentList);
                    }
                }
            }

            //randomize
            groupList.ForEach(i =>
            {
                i = i.Shuffle().ToList();
            });
            groupList = groupList.Shuffle().ToList();

            questionsResult = groupList.SelectMany(i => i.Select(k => k)).ToList();

            return questionsResult;
        }
        public static IEnumerable<Question> RandomAnswers(this IEnumerable<Question> questions)
        {
            var randomAns = questions.ToList();
            randomAns.ForEach(i => i.Answers = i.Answers.Shuffle().ToList());
            return randomAns;
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            List<T> buffer = source.ToList();
            var rng = new Random();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];
                buffer[j] = buffer[i];
            }
        }
        public static bool IsTextOrImage(this Question question)
        {
            var result = false;
            if (question != null)
            {
                var qType = question.QuestionType;
                if (qType != null)
                {
                    if (qType.Type == "Text" || qType.Type == "Image")
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}

namespace System.Web.Mvc
{
    public class ExcelResult : ActionResult
    {
        public ExcelPackage Package { get; set; }
        public String FileName { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Package != null)
            {
                context.HttpContext.Response.Buffer = true;
                context.HttpContext.Response.Clear();
                Package.SaveAs(context.HttpContext.Response.OutputStream);
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                context.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            //context.HttpContext.Response.WriteFile(context.HttpContext.Server.MapPath(Path));
        }
    }
}