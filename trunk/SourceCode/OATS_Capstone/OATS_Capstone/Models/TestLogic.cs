using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class TestLogic
    {
        public string CurrentUserName { get; set; }
        public string TestTitle { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public string Introduction { get; set; }
        public int TestID { get; set; }
        public TestLogic(Test test)
        {
            if (test != null)
            {
                TestID = test.TestID;
                TestTitle = test.TestTitle;
                Introduction = test.Introduction;
                var authen = AuthenticationSessionModel.Instance();
                var user = authen.User;
                if (user != null)
                {
                    CurrentUserName = user.FirstName ?? user.LastName ?? user.UserMail;
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