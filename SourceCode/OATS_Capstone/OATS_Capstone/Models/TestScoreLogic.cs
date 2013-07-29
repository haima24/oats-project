using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class ScoreTest
    {
        public List<UserInTest> InTests { get; set; }
        public string TestTitle { get; set; }
        public List<ScoreUserItem> ScoreUserList { get; set; }
        public decimal? TotalScoreOfTest { get; set; }
        public List<int> CheckedUserIds { get; set; }
        public int CheckedUserIdsCount { get { return CheckedUserIds.Count; } }
        public string ScoreUsersNameLabel { get; set; }
        public List<ScoreTag> AvailableTags { get; set; }
        public ScoreStatistics Overall { get; set; }
        public List<ScoreOnUser> UsersScores { get; set; }
        public ScoreTest(Test test)
        {
            var details = test.UserInTests.FilterInTestsOnAttempSetting().Select(i => i.UserID).ToList();
            InitScoreTest(test, details);
        }
        public ScoreTest(Test test, List<int> checkIds)
        {
            InitScoreTest(test, checkIds);
        }
        private void InitScoreTest(Test test, List<int> checkIds)
        {
            var db = SingletonDb.Instance();
            TestTitle = test.TestTitle;
            CheckedUserIds = checkIds;
            var tags = test.Questions.SelectMany(k => k.TagInQuestions.Select(i => i.Tag)).ToList();
            var lGroupTags = from tag in tags
                             group tag by tag into GroupTags
                             select GroupTags;
            var gTags = lGroupTags.Select(i => i.Key).ToList();
            TotalScoreOfTest = test.Questions.Sum(i =>
            {
                return i.NoneChoiceScore ?? 0 + i.Answers.Sum(k => k.Score ?? 0);
            });
            InTests = test.UserInTests.FilterInTestsOnAttempSetting();
            ScoreUserList = new List<ScoreUserItem>();
            InTests.ForEach(i =>
            {
                if (i.User != null)
                {
                    var item = new ScoreUserItem(gTags, i, TotalScoreOfTest);
                    ScoreUserList.Add(item);
                }
            });
            if (CheckedUserIds.Count != 0)
            {
                var names = ScoreUserList.Where(t => CheckedUserIds.Contains(t.UserID)).Select(k => k.UserLabel).Aggregate((i, o) => i + "," + o);
                if (names != null) { ScoreUsersNameLabel = names; }

                var groupTags = lGroupTags.Select(i => new ScoreTag(i.Key, test.TestID, checkIds));
                AvailableTags = groupTags.ToList();
                Overall = new ScoreStatistics();
                Overall.Name = "Overall";
                var totalScore = test.Questions.TotalScore();
                var groupScoreList = InTests.Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var stdevScore = groupScoreList.StandardDeviation();
                var averageScore = groupScoreList.Average();
                var userScoreList = InTests.Where(k => checkIds.Contains(k.UserID)).Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var selectionAverageScore = userScoreList.Average();

                Overall.SelectionAVGScore = new ScoreStatistic(selectionAverageScore, totalScore);
                Overall.GroupAVGScore = new ScoreStatistic(averageScore, totalScore);
                Overall.DifferenceScore = new ScoreStatistic(selectionAverageScore - averageScore, 1);
                Overall.GroupSTDEVScore = new ScoreStatistic(stdevScore, 1);
                Overall.ScoreList = userScoreList.ToList();

                UsersScores = InTests.Where(k => checkIds.Contains(k.UserID)).Select(i =>
                {
                    var userScore = new ScoreOnUser();
                    userScore.Name = !string.IsNullOrEmpty(i.User.Name) ? i.User.Name : i.User.UserMail;
                    decimal? percent = 0;
                    if (totalScore != 0) { percent = (i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0) / totalScore).RoundTwo() * 100; }
                    userScore.Percent = percent;
                    userScore.UserId = i.UserID;
                    return userScore;
                }).ToList();
            }
        }
        public ExcelPackage ToExcelPackage()
        {
            var excelPackage = new ExcelPackage();
            var wb = excelPackage.Workbook;
            var wsScore = wb.Worksheets.Add("Score");
            wsScore.Cells[1, 1].Value = "Test:";
            wsScore.Cells[1, 2].Value = TestTitle;
            var checkedUsers = ScoreUserList.Where(i => CheckedUserIds.Contains(i.UserID)).ToList();
            if (checkedUsers.Count > 0)
            {
                var dt = new DataTable();
                var columnName = dt.Columns.Add("Name");
                //init columns
                var fObj = checkedUsers.FirstOrDefault();
                var columnOverall = dt.Columns.Add(fObj.Overall.Name, typeof(decimal));
                var otherColumns = new Dictionary<DataColumn, String>();
                fObj.Statistics.ForEach(i =>
                {
                    var column = dt.Columns.Add(i.Name, typeof(decimal));
                    var name = i.Name;
                    otherColumns.Add(column, name);
                });

                checkedUsers.ForEach(i =>
                {
                    var row = dt.NewRow();
                    row[columnName] = i.UserLabel;
                    row[columnOverall] = i.Overall.Percent;
                    foreach (var item in otherColumns)
                    {
                        var obj = i.Statistics.FirstOrDefault(k => k.Name == item.Value);
                        if (obj != null)
                        {
                            row[item.Key] = obj.Percent.RoundTwo();
                        }
                        else
                        {
                            row[item.Key] = 0;
                        }
                    }
                    dt.Rows.Add(row);
                });
                var rangeBase = wsScore.Cells["A2"].LoadFromDataTable(dt, true, TableStyles.Medium9);
                rangeBase.Style.Numberformat.Format = Constants.DefaultExcelPercentFormat;
            }
            else
            {
                wsScore.Cells[2, 1].Value = "No Data Matching Your Selection";
            }
            return excelPackage;
        }
    }
    public class ScoreOnUser
    {
        public int UserId { get; set; }
        public decimal? Percent { get; set; }
        public string Name { get; set; }
    }
    public class ScoreTag
    {
        public Tag Tag { get; set; }
        private ScoreStatistics statistic = null;
        public ScoreStatistics Statistic
        {
            get { return statistic; }
        }
        public ScoreTag(Tag tag, int testid, List<int> checkIds)
        {
            Tag = tag;
            if (tag != null)
            {
                var questions = tag.TagInQuestions.Where(i => i.Question.TestID == testid).Select(k => k.Question);
                statistic = new ScoreStatistics();
                var totalScore = questions.TotalScore();
                var groupDetails = questions.SelectMany(i => i.UserInTestDetails);
                var groupCalculate = from g in groupDetails
                                     group g by g.UserInTestID into GroupDetails
                                     select new { Score = GroupDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0) };
                var userDetails = groupDetails.Where(i => checkIds.Contains(i.UserInTest.UserID));
                var userCalculate = from g in userDetails
                                    group g by g.UserInTestID into UserDetails
                                    select new { Score = UserDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0) };
                var selectionAverageScore = userCalculate.Average(i => i.Score);
                var groupAverageScore = groupCalculate.Average(i => i.Score);
                var stdevScore = groupCalculate.Select(i => i.Score).StandardDeviation();
                var userScoreList = userCalculate.Select(i => i.Score);

                statistic.Name = tag.TagName;
                statistic.SelectionAVGScore = new ScoreStatistic(selectionAverageScore, totalScore);
                statistic.GroupAVGScore = new ScoreStatistic(groupAverageScore, totalScore);
                statistic.DifferenceScore = new ScoreStatistic(selectionAverageScore - groupAverageScore, 1);
                statistic.GroupSTDEVScore = new ScoreStatistic(stdevScore, 1);
                statistic.ScoreList = userScoreList.ToList();
            }
        }
    }
    public class ScoreStatistics
    {
        public string Name { get; set; }
        public ScoreStatistic SelectionAVGScore { get; set; }
        public ScoreStatistic GroupAVGScore { get; set; }
        public ScoreStatistic DifferenceScore { get; set; }
        public ScoreStatistic GroupSTDEVScore { get; set; }
        public List<decimal> ScoreList { get; set; }
        public String ScoreListString
        {
            get
            {
                var s = string.Empty;
                try
                {
                    s = ScoreList.Select(i => i.ToString()).Aggregate((f, o) => f + "," + o);
                }
                catch (Exception)
                {
                    s = string.Empty;
                }
                return s;
            }
        }
    }
    public class ScoreStatistic
    {
        private decimal? numerator = 0;

        public decimal? Numerator
        {
            get { return numerator; }
        }
        private decimal? denominator = 0;

        public decimal? Denominator
        {
            get { return denominator; }
        }
        public decimal NumberValue {
            get {
                return (numerator / denominator).RoundTwo()??0;
            }
        }
        public ScoreStatistic(decimal? num, decimal? denom)
        {
            this.numerator = num;
            this.denominator = denom;
        }
        public string Praction { get { return string.Format("{0:##0.##}", numerator) + "/" + string.Format("{0:##0.##}", denominator); } }
        public string Percent
        {
            get
            {
                var s = "0";
                if (denominator != 0)
                {
                    s = (numerator / denominator).ToPercent();
                }
                return s;
            }
        }
        public string Decimal
        {
            get
            {
                var s = "0";
                if (denominator != 0)
                {
                    s = string.Format("{0:##0.##}", (numerator / denominator));
                }
                return s;
            }
        }
    }
    public class ScoreUserItem
    {
        public decimal? UserDecimalPercent { get; set; }
        public Test Test { get; set; }
        public string UserLabel { get; set; }
        public string UserPercent { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ScoreUserItemStatistic Overall { get; set; }
        public List<ScoreUserItemStatistic> Statistics { get; set; }
        private void Initialize(User user)
        {
            UserID = user.UserID;
            UserDecimalPercent = null;
            User = user;
            UserLabel = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserMail;
            UserPercent = "N/A";
        }
        public ScoreUserItem(List<Tag> tags, UserInTest inTest, decimal? totalScoreOfTest)
        {
            UserLabel = !string.IsNullOrEmpty(inTest.User.Name) ? inTest.User.Name : inTest.User.UserMail;
            var percent = (decimal)inTest.Score / totalScoreOfTest;
            if (inTest.Score == 0)
            {
                UserDecimalPercent = null;
            }
            else {
                UserDecimalPercent = percent;
            }
            
            UserPercent = (percent).ToPercent();
            UserID = inTest.User.UserID;
            User = inTest.User;
            Overall = new ScoreUserItemStatistic() { Name = "Overall Score", Percent = percent.RoundTwo() ?? 0 };
            Statistics = new List<ScoreUserItemStatistic>();
            tags.ForEach(i =>
            {
                var item = new ScoreUserItemStatistic(i, inTest, totalScoreOfTest);
                Statistics.Add(item);
            });
            Test = inTest.Test;
        }
        public ScoreUserItem(User user, Test test)
        {
            var inTest = test.UserInTests.FilterInTestsOnAttempSetting().FirstOrDefault(i => i.UserID == user.UserID);
            if (inTest != null)
            {
                var totalScoreOfTest = inTest.Test.Questions.TotalScore();
                UserID = inTest.User.UserID;
                User = inTest.User;
                UserLabel = !string.IsNullOrEmpty(inTest.User.Name) ? inTest.User.Name : inTest.User.UserMail;
                var percent = (decimal)inTest.Score / totalScoreOfTest;
                if (inTest.Score == 0)
                {
                    UserDecimalPercent = null;
                }
                else
                {
                    UserDecimalPercent = percent;
                }
                UserPercent = (percent).ToPercent();
                Test = inTest.Test;
            }
            else
            {
                Initialize(user);
            }
        }
        public ScoreUserItem(User user)
        {
            Initialize(user);
        }
    }
    public class ScoreUserItemStatistic
    {
        public string Name { get; set; }
        public decimal Percent { get; set; }
        public ScoreUserItemStatistic() { }
        public ScoreUserItemStatistic(Tag tag, UserInTest inTest, decimal? totalScore)
        {
            Name = tag.TagName;
            var details = inTest.UserInTestDetails.Where(i =>
            {
                var tagIds = i.Question.TagInQuestions.Select(k => k.Tag.TagID);
                return tagIds.Contains(tag.TagID);
            });
            var score = details.Sum(i => i.ChoiceScore ?? 0 + i.NonChoiceScore ?? 0);
            decimal percent = 0;
            if (totalScore.HasValue)
            {
                percent = score / totalScore.Value;
            }
            Percent = percent;
        }
    }
    public class OverviewScoreTests
    {
        public string ScoreUsersNameLabel { get; set; }
        public List<Test> AllTests { get; set; }
        public List<User> AllUsers { get; set; }
        public List<Test> CheckedTestsSorted { get; set; }
        public List<User> CheckedUsersSorted { get; set; }
        public List<OverviewScoreOnUser> OverviewScores { get; set; }
        public ExcelPackage ToExcelPackage()
        {
            var excelPackage = new ExcelPackage();
            var wb = excelPackage.Workbook;
            var wsScore = wb.Worksheets.Add("Score");
            var stRange = wsScore.Cells[1, 1, 1, 2];
            stRange.Merge = true;
            stRange.Value = "Student";
            wsScore.Cells[2, 1].Value = "Name";
            wsScore.Cells[2, 2].Value = "Overall";
            if (CheckedUsersSorted.Count != 0 || CheckedTestsSorted.Count != 0)
            {

                //student info
                var beginRow = 3;
                for (int i = 0; i < CheckedUsersSorted.Count; i++)
                {
                    //column always 1
                    var user = CheckedUsersSorted[i];
                    wsScore.Cells[beginRow, 1].Value = user.Name;
                    beginRow++;
                }

                //header tests
                var beginColumn = 3;
                for (int i = 0; i < CheckedTestsSorted.Count; i++)
                {
                    var test = CheckedTestsSorted[i];
                    var titleRange = wsScore.Cells[1, beginColumn, 2, beginColumn];
                    titleRange.Merge = true;
                    titleRange.Value = test.TestTitle;
                    ++beginColumn;
                }

                //details
                beginRow = 3;
                for (int i = 0; i < OverviewScores.Count; i++)
                {
                    var item = OverviewScores[i];
                    var scores = item.Scores;
                    var rOverall=wsScore.Cells[beginRow, 2];
                    object value = null;
                    if (item.OverallDecimalPercent.HasValue)
                    {
                        value = item.OverallDecimalPercent.Value;
                    }
                    else {
                        value = "N/A";
                    }
                    rOverall.Value = value;
                    rOverall.Style.Numberformat.Format = Constants.DefaultExcelPercentFormat;
                    beginColumn = 3;
                    for (int k = 0; k < scores.Count; k++)
                    {
                        var score = scores[k];
                        var range = wsScore.Cells[beginRow, beginColumn];
                        range.Style.Numberformat.Format = Constants.DefaultExcelPercentFormat;
                        object itemValue = null;
                        if (score.UserDecimalPercent.HasValue)
                        {
                            itemValue = score.UserDecimalPercent.Value;
                        }
                        else
                        {
                            itemValue = "N/A";
                        }
                        range.Value = itemValue;
                        ++beginColumn;
                    }
                    ++beginRow;
                }

            }
            else
            {
                wsScore.Cells[1, 1, 1, 2].Value = "No Data Matching Your Selection";
            }
            return excelPackage;
        }
        private void InitOverviewScoreTests(IEnumerable<Test> tests, List<int> userids, List<int> testids)
        {
            AllTests = tests.OrderBy(t => t.TestTitle).ToList();
            AllUsers = tests.SelectMany(i => i.UserInTests.Select(k => k.User)).GroupBy(i => i).Select(i => i.Key).OrderBy(k => k.Name).ToList();
            var checkedTests = AllTests.Where(i => testids.Contains(i.TestID));
            CheckedTestsSorted = checkedTests.OrderBy(i => i.TestTitle).ToList();
            var checkedUsers = AllUsers.Where(i => userids.Contains(i.UserID));
            CheckedUsersSorted = checkedUsers.OrderBy(i =>
            {
                var key = i.Name;
                if (string.IsNullOrEmpty(key))
                {
                    key = i.UserMail;
                }
                return key;
            }).ToList();
            OverviewScores = new List<OverviewScoreOnUser>();
            foreach (var u in CheckedUsersSorted)
            {
                var scoreOnUser = new OverviewScoreOnUser();
                var scoreList = new List<ScoreUserItem>();
                foreach (var t in CheckedTestsSorted)
                {
                    ScoreUserItem item = null;
                    if (CheckedTestsSorted.Select(i => i.TestID).Contains(t.TestID))
                    {
                        item = new ScoreUserItem(u, t);
                    }
                    else
                    {
                        item = new ScoreUserItem(u);
                    }
                    scoreList.Add(item);
                }
                var totalOfAllTests = CheckedTestsSorted.Where(i => i.UserInTests.Select(t => t.UserID).Contains(u.UserID)).SelectMany(i => i.Questions).TotalScore();
                var gainScore = CheckedTestsSorted.SelectMany(i => i.UserInTests).Where(i => i.UserID == u.UserID).Sum(i => i.Score);
                var percentString = string.Empty;
                if (totalOfAllTests == 0)
                {
                    percentString = "N/A";
                    scoreOnUser.OverallDecimalPercent = null;
                }
                else
                {
                    percentString = (gainScore / totalOfAllTests).ToPercent();
                    if (gainScore == 0) {
                        scoreOnUser.OverallDecimalPercent = null;
                    }
                    else {
                        scoreOnUser.OverallDecimalPercent = (gainScore / totalOfAllTests);
                    }
                }
                scoreOnUser.Overall = percentString;
                scoreOnUser.Name = u.Name;
                scoreOnUser.Scores = scoreList;
                OverviewScores.Add(scoreOnUser);
                var names = CheckedUsersSorted.Select(i => i.Name).Aggregate((i, o) => i + "," + o);
                if (names != null) { ScoreUsersNameLabel = names; }
            }
        }
        public OverviewScoreTests(IEnumerable<Test> tests, List<int> userids, List<int> testids)
        {
            InitOverviewScoreTests(tests, userids, testids);
        }
        public OverviewScoreTests(IEnumerable<Test> tests)
        {
            var testids = tests.Select(i => i.TestID).ToList();
            var userids = tests.SelectMany(i => i.UserInTests.Select(k => k.User)).GroupBy(i => i).Select(i => i.Key.UserID).ToList();
            InitOverviewScoreTests(tests, userids, testids);
        }
    }
    public class OverviewScoreOnUser
    {
        public string Name { get; set; }
        public string Overall { get; set; }
        public decimal? OverallDecimalPercent { get; set; }
        public List<ScoreUserItem> Scores { get; set; }
    }
}