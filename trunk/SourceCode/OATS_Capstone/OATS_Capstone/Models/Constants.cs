﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OATS_Capstone.Models
{
    public class Constants
    {
        public const string DefaultProblemMessage = "Sorry, your operation failed, try again later.";
        public const string DefaultSuccessMessage = "Successful";
        public const string DefaultExceptionMessage = "There was a problem in server, please try gain later.";
        public const string DefaultSignUpSuccessMessage = "Signup was successful.";
        public const string DefaultSubmitTestSuccessMessage = "Submit test was successful.";
        public const string DefaultDuplicateEmail = "This email already exist, please choose another.";
        public const string DefaultTestInInvalidDateTime = "This test currently is not in range of start time    and end time.";
        public const string DefaultTestNotRunningOrActive = "This test currently is not marked as actived running test or contents not completed.";
        public const string DefaultTestNotExist = "This test is not exist.";
        public const string DefaultNoPermission = "Test content is not available or you have no permission to access content of this test.";
        public const string DefaultCannotExportFile = "Sorry, we could not export your requested file due to server problem. Try gain later.";
        public const string DefaultTestContainedQuestion = "Your current edit question exist in this test. Make content different.";
        public const string DefaultExcelPercentFormat = "#0%";
        public const string DefaultMaxOfAttemp = "Max Of Attemp";
        public const int DefaultSettingConfigId = 1;
        public const int DefaultTestDuration = 10;
    }
    public class AttempTypes {
        public const string Recent = "Recent";
        public const string Average = "Average";
        public const string Maximum = "Maximum";
    }
}