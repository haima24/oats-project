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
        public string TestTitle { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
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
        public decimal? Average { get; }
        public decimal? STDEV { get; }
        public TestListItem(Test test)
        {
            TestTitle = test.TestTitle;
            Start = test.StartDateTime;
            End = test.EndDateTime;

            var groupScoreList = test.UserInTests.Select(i =>
            {
                return i.UserInTestDetails.Sum(k => k.NonChoiceScore ?? 0 + k.ChoiceScore ?? 0);
            });
            var stdevScore = groupScoreList.StandardDeviation();
            var averageScore = groupScoreList.Average();
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