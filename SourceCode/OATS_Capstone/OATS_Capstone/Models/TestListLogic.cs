﻿using System;
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
            var authen = AuthenticationSessionModel.Instance();
            var invitedTest = tests.Where(i => i.Invitations.Select(t => t.UserID).Contains(authen.UserId));
            var ownTests = tests.Where(i => i.CreatedUserID == authen.UserId);
            recentTests = invitedTest.FilterByRecents().Select(i => new TestListItem(i) { IsRunning = i.IsRunning }).Concat(ownTests.FilterByRecents().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning })).ToList();
            runningTests = invitedTest.FilterByRuning().Select(i => new TestListItem(i) { IsRunning = i.IsRunning }).Concat(ownTests.FilterByRuning().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning })).ToList();
            upComingTests = invitedTest.FilterByUpcoming().Select(i => new TestListItem(i) { IsRunning = i.IsRunning }).Concat(ownTests.FilterByUpcoming().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning })).ToList();
        }
    }
    public class TestListItem
    {
        private bool isRunning = true;
        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }
        private bool isCurrentUserOwnTest = false;
        public string Introduction { get; set; }
        public bool IsCurrentUserOwnTest
        {
            get { return isCurrentUserOwnTest; }
            set { isCurrentUserOwnTest = value; }
        }
        public int TestID { get; set; }
        public string TestTitle { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        private int allInvitedCount = 0;
        private int resultedOnInvitedCount = 0;
        public int NumOfStudent { get; set; }
        public int NumOfTeacher { get; set; }

        public int ResultedOnInvitedCount
        {
            get { return resultedOnInvitedCount; }
        }

        public int AllInvitedCount
        {
            get { return allInvitedCount; }
        }
        private decimal? _Average = null;

        public decimal? Average
        {
            get { return _Average; }
        }
        public string AverageString { get { return Average.HasValue ? string.Format("{0:#0.#}", Average) : "-"; } }
        public string STDEVString { get { return STDEV.HasValue ? string.Format("{0:#0.#}", STDEV) : "-"; } }
        private decimal? _STDEV = null;

        public decimal? STDEV
        {
            get { return _STDEV; }
        }
        public string Rate
        {
            get
            {
                var r = "N/A";
                if (allInvitedCount != 0 && resultedOnInvitedCount != 0)
                {
                    r = string.Format("{0} out of {1}", resultedOnInvitedCount, allInvitedCount);
                }
                return r;
            }
        }
        public TestListItem(Test test)
        {
            TestID = test.TestID;
            TestTitle = test.TestTitle;
            Introduction = test.Introduction;
            Start = test.StartDateTime.ToDateDefaultFormat();
            End = test.EndDateTime.ToDateDefaultFormat();
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
            var allInvitations = test.Invitations;
            NumOfStudent = allInvitations.Count(k => k.Role.RoleDescription == "Student");
            NumOfTeacher = allInvitations.Count(k => k.Role.RoleDescription == "Teacher");
        }
    }
}