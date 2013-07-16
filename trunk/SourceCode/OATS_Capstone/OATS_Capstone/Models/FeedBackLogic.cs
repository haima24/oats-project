using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public abstract class AbsFeedBackUser
    {
        public abstract List<FeedBack> Parents { get; }
        public int FeedBackCount { get { return Parents.Count; } }
        public string TestTitle { get; set; }
        public int TestID { get; set; }
        public AbsFeedBackUser(Test test)
        {
            TestTitle = test.TestTitle;
            TestID = test.TestID;
        }
    }
    public class FeedBackStudent : AbsFeedBackUser
    {
        private List<FeedBack> parents;
        public FeedBackStudent(Test test)
            : base(test)
        {
            parents = test.FeedBacks.Where(i =>
            {

                var invitation = test.Invitations.FirstOrDefault(k => k.UserID == i.UserID);
                if (invitation != null)
                {
                    return invitation.Role.RoleDescription == "Student" && !i.ParentID.HasValue;
                }
                else
                {
                    return false;
                }
            }).ToList();

            //default order by date
            parents = parents.OrderByDescending(i => i.FeedBackDateTime).ToList();
            parents.ForEach(i => i.FeedBackChilds = i.FeedBackChilds.OrderBy(k => k.FeedBackDateTime).ToList());
        }
        public override List<FeedBack> Parents
        {
            get { return parents; }
        }
    }
    public class FeedBackTeacher : AbsFeedBackUser
    {
        private List<FeedBack> parents;
        public FeedBackTeacher(Test test)
            : base(test)
        {
            parents = test.FeedBacks.Where(i =>
            {
                if (test.CreatedUserID == i.UserID)
                {
                    return true;
                }
                else
                {
                    var invitation = test.Invitations.FirstOrDefault(k => k.UserID == i.UserID);
                    if (invitation != null)
                    {
                        return invitation.Role.RoleDescription == "Teacher" && !i.ParentID.HasValue;
                    }
                    else
                    {
                        return false;
                    }
                }
            }).ToList();

            //default order by date
            parents = parents.OrderByDescending(i => i.FeedBackDateTime).ToList();
            parents.ForEach(i => i.FeedBackChilds = i.FeedBackChilds.OrderBy(k => k.FeedBackDateTime).ToList());
        }
        public override List<FeedBack> Parents
        {
            get { return parents; }
        }
    }
}