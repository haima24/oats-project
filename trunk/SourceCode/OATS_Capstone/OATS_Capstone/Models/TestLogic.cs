using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class TestLogic
    {
        public bool IsRunning { get; set; }
        public bool IsActive { get; set; }
        public bool IsDateTimeNotValid { get; set; }
        public SettingConfigDetail RequireAccessCodeSetting{get;set;}
        public string CurrentUserName { get; set; }
        public string TestTitle { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public string Introduction { get; set; }
        public int TestID { get; set; }
        public bool IsInInvitationRoleStudent { get; set; }
        public TestLogic(Test test)
        {
            if (test != null)
            {
                RequireAccessCodeSetting = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "RTC");

                var now = DateTime.Now;
                IsDateTimeNotValid = false;
                var end = test.EndDateTime;
                var start = test.StartDateTime;
                if (now.CompareTo(start) < 0)
                {
                    IsDateTimeNotValid = true;
                }
                if (end.HasValue)
                {
                    if (end.Value.CompareTo(now) < 0)
                    {
                        IsDateTimeNotValid = true;
                    }
                }

                IsActive = test.IsActive;
                IsRunning=test.IsRunning;
                TestID = test.TestID;
                TestTitle = test.TestTitle;
                Introduction = test.Introduction;
                var authen = AuthenticationSessionModel.Instance();
                var user = authen.User;
                var userId = authen.UserId;
                if (user != null)
                {
                    CurrentUserName = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserMail;
                }

                var invitation = test.Invitations.FirstOrDefault(i => i.UserID == userId);
                if (invitation != null) {
                    if (invitation.Role.RoleDescription == "Student") {
                        IsInInvitationRoleStudent = true;
                    }
                }

                Questions = test.Questions.OrderBy(i => i.SerialOrder);
                var settingDetail = test.SettingConfig.SettingConfigDetails.FirstOrDefault(i => i.SettingType.SettingTypeKey == "RAO");
                if (settingDetail.IsActive)
                {
                    Questions = Questions.RandomAnswers();
                }

                Questions = Questions.RandomQuestion();
            }
        }
    }
}