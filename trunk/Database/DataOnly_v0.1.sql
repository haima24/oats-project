USE [OATSDB]
GO
SET IDENTITY_INSERT [dbo].[Role] ON 

GO
INSERT [dbo].[Role] ([RoleID], [RoleDescription]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Role] ([RoleID], [RoleDescription]) VALUES (2, N'Student')
GO
INSERT [dbo].[Role] ([RoleID], [RoleDescription]) VALUES (3, N'Teacher')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (1, N'12345', N'akai777@gmail.com', N'01285591685', N'Huynh', N'Tu', N'Binh Duong', 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (8, NULL, N'abc@abc.com', NULL, N'Last Name of Teacher 1', N'First Name of Teacher 1', NULL, 3, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (9, NULL, N'xyz@xyz.com', NULL, N'Last Name of Teacher 2', N'First Name of Teacher 2', NULL, 3, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (10, NULL, N'hmt@hmt.com', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (12, NULL, N'', NULL, NULL, NULL, NULL, 3, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (13, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (14, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (1013, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (2013, NULL, N'abc@abc.com', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (2014, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (2015, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (2016, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (2017, NULL, N'', NULL, NULL, NULL, NULL, 2, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[QuestionType] ON 

GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (1, N'Radio')
GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (2, N'Multiple')
GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (3, N'Essay')
GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (4, N'ShortAnswer')
GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (5, N'Text')
GO
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (6, N'Image')
GO
SET IDENTITY_INSERT [dbo].[QuestionType] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] ON 

GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (1, N'Default Setting')
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (3, N'Sample Test Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (4, N'Test History of Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (12, N'', 1, CAST(0x0000A1C800FF4913 AS DateTime), CAST(0x0000A1C800FF4913 AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (13, N'', 1, CAST(0x0000A1C80100B0CA AS DateTime), CAST(0x0000A1C80100B0CA AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (14, N'Lession 123', 1, CAST(0x0000A1C80102E985 AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E500993D9A AS DateTime), NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (15, N'demo', 1, CAST(0x0000A1D000993D9A AS DateTime), CAST(0x0000A1D000993D9A AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (16, N'Test Demos', 1, CAST(0x0000A1D0009F4759 AS DateTime), CAST(0x0000A1D0009F4759 AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (17, N'', 1, CAST(0x0000A1D201390D47 AS DateTime), CAST(0x0000A1D201390D47 AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (18, N'Bai test 3', 1, CAST(0x0000A1D30110972F AS DateTime), CAST(0x0000A1D30110972F AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (19, N'test 20', 1, CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (20, N'bai test 20', 1, CAST(0x0000A1D30121A97E AS DateTime), CAST(0x0000A1D40121A97E AS DateTime), CAST(0x0000A1ED0121A97E AS DateTime), NULL, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID]) VALUES (21, N'Test 21', 1, CAST(0x0000A1D30121E013 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D401139A86 AS DateTime), NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Question] ON 

GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2201, N'Demo radio 1', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 0, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2202, N'Demo Multiple 1', 14, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2203, N'abc', 18, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2223, N'', 21, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2224, N'Demo Essay', 14, 3, NULL, N'Demo Essay Detail', CAST(0.00 AS Decimal(18, 2)), 2, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2225, N'Short Answer', 14, 4, NULL, N'Demo Short answer detial', CAST(0.00 AS Decimal(18, 2)), 3, N'4. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2226, N'Demo Text', 14, 5, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 4, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2227, N'', 14, 6, N'~/Resource/Images/kien.jpg', NULL, CAST(0.00 AS Decimal(18, 2)), 5, N'B. ', NULL)
GO
SET IDENTITY_INSERT [dbo].[Question] OFF
GO
SET IDENTITY_INSERT [dbo].[Tag] ON 

GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1, N'Template')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (2, N'Sample')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (3, N'History')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (4, N'Math')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (5, N'IT')
GO
SET IDENTITY_INSERT [dbo].[Tag] OFF
GO
INSERT [dbo].[TagInQuestion] ([TagID], [QuestionID]) VALUES (1, 2201)
GO
INSERT [dbo].[TagInQuestion] ([TagID], [QuestionID]) VALUES (1, 2202)
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3263, N'Ans Radio 1', 2201, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3264, N'Ans Radio 2', 2201, 1, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3265, N'Multiple 1', 2202, 1, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3266, N'Multiple 2', 2202, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3268, N'xyz', 2203, 1, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3269, N'lhk', 2203, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3277, N'', 2202, 0, NULL, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3313, N'', 2223, 0, NULL, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3314, N'', 2223, 0, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[Answer] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingTypes] ON 

GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (2, N'Test Access Control', 0, N'RTC', N'Require test access code')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (3, N'Test Access Control', 1, N'OSM', N'One Submission Only - Time Limit')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (5, N'Test Formatting', 0, N'ROQ', N'Randomize the order of questions')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (6, N'Test Formatting', 1, N'SQP', N'Show question point value')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (7, N'Results & Reporting', 0, N'SAR', N'Students can access their own score reports')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (8, N'Results & Reporting', 1, N'ISA', N'Immediate self-assessment')
GO
SET IDENTITY_INSERT [dbo].[SettingTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] ON 

GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (2, 1, 2, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (3, 1, 3, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (5, 1, 5, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (6, 1, 6, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (7, 1, 7, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (8, 1, 8, NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Invitation] ON 

GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [InvitationDateTime]) VALUES (1, 3, 1013, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [InvitationDateTime]) VALUES (2, 3, 2013, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [InvitationDateTime]) VALUES (3, 4, 2013, NULL)
GO
SET IDENTITY_INSERT [dbo].[Invitation] OFF
GO
