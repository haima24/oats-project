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

        private void Initialize(IEnumerable<Test> tests, string role)
        {
            var authen = AuthenticationSessionModel.Instance();
            var curUserId = authen.UserId;
            var studentInvited = tests.Where(i =>
            {
                var condition = false;
                var invitation = i.Invitations.FirstOrDefault(t => t.UserID == curUserId);
                if (invitation != null)
                {
                    if (invitation.Role.RoleDescription == "Student")
                    {
                        condition = true;
                    }
                }
                return condition;
            });
            var teacherInvited = tests.Where(i =>
            {
                var condition = false;
                var invitation = i.Invitations.FirstOrDefault(t => t.UserID == curUserId);
                if (invitation != null)
                {
                    if (invitation.Role.RoleDescription == "Teacher")
                    {
                        condition = true;
                    }
                }
                return condition;
            });
            var ownTests = tests.Where(i => i.CreatedUserID == authen.UserId).Concat(teacherInvited);
            switch (role)
            {
                case "Do":
                    recentTests = studentInvited.FilterByRecents().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = false, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    runningTests = studentInvited.FilterByRuning().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = false, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    upComingTests = studentInvited.FilterByUpcoming().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = false, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    break;
                case "Review":
                    recentTests = ownTests.FilterByRecents().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    runningTests = ownTests.FilterByRuning().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    upComingTests = ownTests.FilterByUpcoming().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete }).ToList();
                    break;
                default:
                    recentTests = studentInvited.FilterByRecents().Select(i => new TestListItem(i) { IsCurrentUserOwnTest = false, IsRunning = i.IsRunning, IsComplete = i.IsComplete }).Concat(ownTests.FilterByRecents().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete })).ToList();
                    runningTests = studentInvited.FilterByRuning().Select(i => new TestListItem(i) { IsCurrentUserOwnTest = false, IsRunning = i.IsRunning, IsComplete = i.IsComplete }).Concat(ownTests.FilterByRuning().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete })).ToList();
                    upComingTests = studentInvited.FilterByUpcoming().Select(i => new TestListItem(i) { IsCurrentUserOwnTest = false, IsRunning = i.IsRunning, IsComplete = i.IsComplete }).Concat(ownTests.FilterByUpcoming().Select(k => new TestListItem(k) { IsCurrentUserOwnTest = true, IsRunning = k.IsRunning, IsComplete = k.IsComplete })).ToList();
                    break;
            }
            recentTests = recentTests.OrderByDescending(i => i.CreatedDate).ToList();
            runningTests = runningTests.OrderByDescending(i => i.CreatedDate).ToList();
            upComingTests = upComingTests.OrderByDescending(i => i.CreatedDate).ToList();
        }

        public TestList(IEnumerable<Test> tests)
        {
            Initialize(tests,null);
        }

        public TestList(IEnumerable<Test> tests, string role)
        {
            Initialize(tests, role);
        }
    }
    public class TestListItem
    {
        public bool CanViewHistory { get; set; }
        private int? remainAttemp = null;
        public int? RemainAttemp
        {
            get { return remainAttemp; }
            set { remainAttemp = value; }
        }
        public bool IsComplete { get; set; }
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
        public DateTime CreatedDate { get; set; }
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
            CreatedDate = test.CreatedDateTime;
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
            var settingDetails = test.SettingConfig.SettingConfigDetails;
            var osmSetting = settingDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "OSM");
            if (osmSetting != null)
            {
                if (osmSetting.IsActive)
                {
                    var authen = AuthenticationSessionModel.Instance();
                    var userId = authen.UserId;
                    var inTests = test.UserInTests.Where(i => i.UserID == userId);
                    if (inTests.Count() > 0)
                    {
                        var maxAttemp = osmSetting.NumberValue ?? 0;
                        var curAttemp = inTests.Max(i => i.NumberOfAttend);
                        var delta = maxAttemp - curAttemp;
                        remainAttemp = delta < 0 ? 0 : delta;
                    }
                }
            }
            var canViewHistorySetting = settingDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "SAR");
            if (canViewHistorySetting != null)
            {
                if (canViewHistorySetting.IsActive)
                {
                    CanViewHistory = true;
                }
            }
        }
    }
}