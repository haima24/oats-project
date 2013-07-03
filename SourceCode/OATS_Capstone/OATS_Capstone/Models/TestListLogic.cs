using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class TestList
    {
        private List<TestListItem> recentTests;

        public List<TestListItem> RecentTests
        {
            get { return recentTests; }
        }
        private List<TestListItem> runningTests;

        public List<TestListItem> RunningTests
        {
            get { return runningTests; }
        }

        private List<TestListItem> upComingTests;

        public List<TestListItem> UpComingTests
        {
            get { return upComingTests; }
        }

        public TestList(IEnumerable<Test> tests)
        {
            runningTests = tests.Where(i => i.UserInTests.Count == 0).Select(k => {
                var testListItem = new TestListItem(k);
                return testListItem;
            }).ToList();
            recentTests = tests.Where(i => i.UserInTests.Count > 0).Select(k =>
            {
                var testListItem = new TestListItem(k);
                return testListItem;
            }).ToList();
        }
    }
    public class TestListItem
    {
        public int TestID { get; set; }
        public string TestTitle { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        private int allInvitedCount = 0;
        private int resultedOnInvitedCount = 0;

        public int ResultedOnInvitedCount
        {
            get { return resultedOnInvitedCount; }
        }

        public int AllInvitedCount
        {
            get { return allInvitedCount; }
        }
        private decimal? _Average=null;

        public decimal? Average
        {
            get { return _Average; }
        }
        public string AverageString { get { return Average.HasValue ?Average.ToString(): "-"; } }
        public string STDEVString { get { return STDEV.HasValue ? STDEV.ToString() : "-"; } }
        private decimal? _STDEV=null;

        public decimal? STDEV
        {
            get { return _STDEV; }
        }
        public string Rate { get {
            var r = "N/A";
            if (allInvitedCount!=0&&resultedOnInvitedCount!=0) {
                r = string.Format("{0} out of {1}", resultedOnInvitedCount, allInvitedCount);
            }
            return r;
        } }
        public TestListItem(Test test)
        {
            TestID = test.TestID;
            TestTitle = test.TestTitle;
            Start = test.StartDateTime.ToDateDefaultFormat();
            End = test.EndDateTime.ToDateDefaultFormat() ;
            if (test.UserInTests.Count > 0)
            {
                var groupScoreList = test.UserInTests.Select(i =>
                {
                    return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
                });
                _STDEV = groupScoreList.StandardDeviation();
                _Average = groupScoreList.Average();
            }
            var allInvited = test.Invitations.Where(i => i.Role.RoleDescription == "Student");
            allInvitedCount = allInvited.Count();
            resultedOnInvitedCount = allInvited.Count(i =>
            {
                var userInTest = test.UserInTests.FirstOrDefault(k => k.UserID == i.UserID);
                return userInTest != null;
            });
        }
    }
}