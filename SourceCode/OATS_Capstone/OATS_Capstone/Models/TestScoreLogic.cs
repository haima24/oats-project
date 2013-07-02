using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class ScoreTest
    {
        public List<ScoreUserItem> ScoreUserList { get; set; }
        public decimal? TotalScoreOfTest { get; set; }
        public List<int> CheckedUserIds { get; set; }
        public int CheckedUserIdsCount { get { return CheckedUserIds.Count; } }
        public string ScoreUsersNameLabel { get; set; }
        public List<ScoreTag> AvailableTags { get; set; }
        public ScoreStatistics Overall { get; set; }
        public List<ScoreOnUser> UsersScores { get; set; }
        public ScoreTest(Test test, List<int> checkIds)
        {
            var db = SingletonDb.Instance();
            CheckedUserIds = checkIds;
            TotalScoreOfTest = test.Questions.Sum(i =>
            {
                return i.NoneChoiceScore ?? 0 + i.Answers.Sum(k => k.Score ?? 0);
            });
            var details = test.UserInTests.ToList();
            ScoreUserList = new List<ScoreUserItem>();
            details.ForEach(i =>
            {
                if (i.User != null)
                {
                    var item = new ScoreUserItem();
                    item.UserLabel = i.User.FirstName ?? i.User.LastName ?? i.User.UserMail;
                    item.UserPercent = ((decimal)i.Score / TotalScoreOfTest).ToPercent();
                    item.UserID = i.User.UserID;
                    ScoreUserList.Add(item);
                }
            });
            if (CheckedUserIds.Count != 0)
            {
                var names = ScoreUserList.Where(t => CheckedUserIds.Contains(t.UserID)).Select(k => k.UserLabel).Aggregate((i, o) => i + "," + o);
                if (names != null) { ScoreUsersNameLabel = names; }
                var tags = test.Questions.SelectMany(k => k.TagInQuestions.Select(i => i.Tag)).ToList();
                var groupTags = from tag in tags
                                group tag by tag into GroupTags
                                select new ScoreTag(GroupTags.Key, test.TestID, checkIds);
                AvailableTags = groupTags.ToList();
                Overall = new ScoreStatistics();
                Overall.Name = "Overall";
                var totalScore = test.Questions.TotalScore();
                var groupScoreList = test.UserInTests.Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var stdevScore = groupScoreList.StandardDeviation();
                var averageScore = groupScoreList.Average();
                var userScoreList = test.UserInTests.Where(k => checkIds.Contains(k.UserID)).Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                var selectionAverageScore = userScoreList.Average();

                Overall.SelectionAVGScore = new ScoreStatistic(selectionAverageScore, totalScore);
                Overall.GroupAVGScore = new ScoreStatistic(averageScore, totalScore);
                Overall.DifferenceScore = new ScoreStatistic(selectionAverageScore - averageScore, 1);
                Overall.GroupSTDEVScore = new ScoreStatistic(stdevScore, 1);
                Overall.ScoreList = userScoreList.ToList();

                UsersScores = test.UserInTests.Where(k => checkIds.Contains(k.UserID)).Select(i =>
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
                                     select new {Score=GroupDetails.Sum(k=>k.NonChoiceScore??0+k.ChoiceScore??0)};
                var userDetails = groupDetails.Where(i => checkIds.Contains(i.UserInTest.UserID));
                var userCalculate = from g in userDetails
                                     group g by g.UserInTestID into UserDetails
                                     select new {Score=UserDetails.Sum(k=>k.NonChoiceScore??0+k.ChoiceScore??0)};
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
    }
}