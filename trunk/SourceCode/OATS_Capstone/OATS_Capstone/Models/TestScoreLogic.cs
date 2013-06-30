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
        public ScoreTest(Test test, List<int> checkIds)
        {
            var db=SingletonDb.Instance();
            CheckedUserIds = checkIds;
            TotalScoreOfTest = test.Questions.Sum(i =>
            {
                return i.NoneChoiceScore + i.Answers.Sum(k => k.Score);
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
          
            var tags = test.Questions.SelectMany(k => k.TagInQuestions.Select(i => i.Tag)).ToList();
            AvailableTags = new List<ScoreTag>();
            tags.ForEach(i =>
                {
                    if (i != null)
                    {
                        var scoreTag = new ScoreTag(i,test.TestID,checkIds);
                        AvailableTags.Add(scoreTag);
                    }
                });
            if (CheckedUserIds.Count != 0)
            {
                var names = ScoreUserList.Where(t=>CheckedUserIds.Contains(t.UserID)).Select(k => k.UserLabel).Aggregate((i, o) => i + "," + o);
                if (names != null) { ScoreUsersNameLabel = names; }
            }
        }
    }
    public class ScoreTag
    {
        public Tag Tag { get; set; }
        public ScoreTag(Tag tag,int testid,List<int> checkIds)
        {
            Tag = tag;
        }
    }
    public class ScoreUserItem
    {
        public string UserLabel { get; set; }
        public string UserPercent { get; set; }
        public int UserID { get; set; }
    }
}