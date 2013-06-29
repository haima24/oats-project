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
            if (CheckedUserIds.Count != 0)
            {
                CheckedUserIds.ForEach(t =>
                {
                    var user = db.Users.FirstOrDefault(i => i.UserID == t);
                    if (user != null)
                    { 
                    }
                });
                var names = ScoreUserList.Select(k => k.UserLabel).Aggregate((i, o) => i + "," + o);
                if (names != null) { ScoreUsersNameLabel = names; }
            }
            var tags = test.Questions.SelectMany(k => k.TagInQuestions.Select(i => i.Tag)).ToList();
            AvailableTags = new List<ScoreTag>();
            tags.ForEach(i =>
                {
                    if (i != null)
                    {
                        var scoreTag = new ScoreTag(i);
                        AvailableTags.Add(scoreTag);
                    }
                });
        }
    }
    public class ScoreTag
    {
        public Tag Tag { get; set; }
        public ScoreTag(Tag tag)
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