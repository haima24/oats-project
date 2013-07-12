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
            var details = test.UserInTests.FilterValidMaxAttend().Select(i => i.UserID).ToList();
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
            var details = test.UserInTests.FilterValidMaxAttend();
            ScoreUserList = new List<ScoreUserItem>();
            details.ForEach(i =>
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
                var groupScoreList = test.UserInTests.FilterValidMaxAttend().Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var stdevScore = groupScoreList.StandardDeviation();
                var averageScore = groupScoreList.Average();
                var userScoreList = test.UserInTests.FilterValidMaxAttend().Where(k => checkIds.Contains(k.UserID)).Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var selectionAverageScore = userScoreList.Average();

                Overall.SelectionAVGScore = new ScoreStatistic(selectionAverageScore, totalScore);
                Overall.GroupAVGScore = new ScoreStatistic(averageScore, totalScore);
                Overall.DifferenceScore = new ScoreStatistic(selectionAverageScore - averageScore, 1);
                Overall.GroupSTDEVScore = new ScoreStatistic(stdevScore, 1);
                Overall.ScoreList = userScoreList.ToList();

                UsersScores = test.UserInTests.FilterValidMaxAttend().Where(k => checkIds.Contains(k.UserID)).Select(i =>
                {
                    var userScore = new ScoreOnUser();
                    userScore.Name = i.User.FirstName ?? i.User.LastName ?? i.User.UserMail;
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
                var columnOverall= dt.Columns.Add(fObj.Overall.Name);
                var otherColumns = new Dictionary<DataColumn, String>();
                fObj.Statistics.ForEach(i => {
                    var column=dt.Columns.Add(i.Name);
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
                        var obj=i.Statistics.FirstOrDefault(k => k.Name == item.Value);
                        if (obj != null)
                        {
                            row[item.Key] = obj.Percent.RoundTwo();
                        }
                        else {
                            row[item.Key] = 0;
                        }
                    }
                    dt.Rows.Add(row);
                });
                wsScore.Cells["A2"].LoadFromDataTable(dt, true, TableStyles.Medium9);
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
        public string UserLabel { get; set; }
        public string UserPercent { get; set; }
        public int UserID { get; set; }
        public ScoreUserItemStatistic Overall { get; set; }
        public List<ScoreUserItemStatistic> Statistics { get; set; }
        public ScoreUserItem(List<Tag> tags, UserInTest inTest, decimal? totalScoreOfTest)
        {
            UserLabel = inTest.User.FirstName ?? inTest.User.LastName ?? inTest.User.UserMail;
            var percent = (decimal)inTest.Score / totalScoreOfTest;
            UserPercent = (percent).ToPercent();
            UserID = inTest.User.UserID;
            Overall = new ScoreUserItemStatistic() { Name = "Overall Score", Percent = percent.RoundTwo() ?? 0 };
            Statistics = new List<ScoreUserItemStatistic>();
            tags.ForEach(i =>
            {
                var item = new ScoreUserItemStatistic(i, inTest, totalScoreOfTest);
                Statistics.Add(item);
            });
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
}