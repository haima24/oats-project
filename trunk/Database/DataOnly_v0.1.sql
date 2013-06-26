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
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (1, N'12345678', N'pk_lilo777@yahoo.com', N'01285591685', N'Huynh', N'Tu', N'Binh Duong', NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (8, N'12345', N'abc@abc.com', N'43234234', N'Last Name of Teacher 1', N'First Name of Teacher 1', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (9, N'12345', N'xyz@xyz.com', N'242342342', N'Last Name of Teacher 2', N'First Name of Teacherrer34rer', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (10, N'12345', N'hmt@hmt.com', N'3414234', N'abc', N'lkasjd', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (12, N'12345', N'lll@lll.lll', N'324234234', N'qtret', N'sdaad', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (13, N'12345', N'aaa@aaa.aaa', N'5432342', N'ewrtw', N'qweqe', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (14, N'12345', N'opais@asai.opfid', N'453453453', N'wtret', N'asd', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (1013, N'12345', N'ueow@iore.com', N'4534535', N'retyt', N'ert', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2013, N'12345', N'abc@abc.com', N'2543535', N'rtet45', N'dasd', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2014, N'12345', N'ioreu@doifs.com', N'345345435', N'twerr', N'hth', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2015, N'12345', N'sdfs@oirw.com', N'134523523', N'tere', N'dggd', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2016, N'12345', N'erw732@oiud.com', N'4523523', N'wetr', N'sdfg', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2017, N'12345', N'rwiru@siorfu.com', NULL, N'hfghr', N'sdg', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2018, N'12345', N'abc@abc.com', NULL, NULL, N'Test Teacher', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2020, N'123456789', N'akai999@gmail.com', N'091902392039', N'Dang', N'Ha', N'Phan Thiet', NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2021, N'123456789', N'aaaa@aaaa.com', N'09139029032', N'Dang', N'Ha', N'Phan Thiet', NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2023, N'123456789', N'akai111@gmail.com', N'092039230', N'Huynh', N'Tu', N'Binh Duong', NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2024, N'12345678', N'aka@jhds.com', N'23232', N'Nguyen', N'Tan', N'Da Nang', NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (2025, N'12345678', N'tiwtiger@gmail.com', NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] ON 

GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (1, N'Default Setting')
GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (3, N'SettingForTest_14')
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3, N'Sample Test Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (4, N'Test History of Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (12, N'', 1, CAST(0x0000A1C800FF4913 AS DateTime), CAST(0x0000A1C800FF4913 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (13, N'', 1, CAST(0x0000A1C80100B0CA AS DateTime), CAST(0x0000A1C80100B0CA AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (14, N'Lession 123', 1, CAST(0x0000A1C80102E985 AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E300993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (15, N'demo', 1, CAST(0x0000A1D000993D9A AS DateTime), CAST(0x0000A1D000993D9A AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (16, N'Test Demos', 1, CAST(0x0000A1D0009F4759 AS DateTime), CAST(0x0000A1D0009F4759 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (17, N'', 1, CAST(0x0000A1D201390D47 AS DateTime), CAST(0x0000A1D201390D47 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (18, N'Bai test 3', 1, CAST(0x0000A1D30110972F AS DateTime), CAST(0x0000A1D30110972F AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (19, N'test 20', 1, CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (20, N'bai test 20', 1, CAST(0x0000A1D30121A97E AS DateTime), CAST(0x0000A1D40121A97E AS DateTime), CAST(0x0000A1ED0121A97E AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (21, N'Test 21', 1, CAST(0x0000A1D30121E013 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D401139A86 AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (22, N'Test so 22', 1, CAST(0x0000A1DD016B650D AS DateTime), CAST(0x0000A1DD016B650D AS DateTime), CAST(0x0000A1E9016B650D AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (23, N'', 1, CAST(0x0000A1DD01715DE6 AS DateTime), CAST(0x0000A1DD01715DE6 AS DateTime), NULL, NULL, 1, 0)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (24, N'', 1, CAST(0x0000A1DD01732A91 AS DateTime), CAST(0x0000A1DD01732A91 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (25, N'', 1, CAST(0x0000A1DD0176EBE7 AS DateTime), CAST(0x0000A1DD0176EBE7 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (26, N'sdfsdf', 1, CAST(0x0000A1DD0178C304 AS DateTime), CAST(0x0000A1DD0178C304 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (1022, N'', 1, CAST(0x0000A1DE009FED69 AS DateTime), CAST(0x0000A1DE009FED69 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (1023, N'', 1, CAST(0x0000A1E700BB886E AS DateTime), CAST(0x0000A1E700BB886E AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (2023, N'COPY: Lession 123', 1, CAST(0x0000A1E801614B0C AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E500993D9A AS DateTime), NULL, 3, 1)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Invitation] ON 

GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (101, 23, 2023, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (102, 23, 8, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (103, 24, 2025, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3102, 14, 10, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3103, 14, 12, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3104, 14, 13, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3105, 14, 14, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3106, 14, 1013, 2, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3107, 14, 8, 3, NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime]) VALUES (3108, 14, 9, 3, NULL)
GO
SET IDENTITY_INSERT [dbo].[Invitation] OFF
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
SET IDENTITY_INSERT [dbo].[Question] ON 

GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2201, N'Demo radio 1', 14, 1, NULL, NULL, 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2202, N'Demo Multiple 1', 14, 2, NULL, NULL, 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2203, N'abc', 18, 1, NULL, NULL, 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2223, N'', 21, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2224, N'Demo Essay', 14, 3, NULL, N'Demo Essay Detail', 5, N'4. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2225, N'Short Answer', 14, 4, NULL, N'Demo Short answer detial', 4, N'3. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2226, N'Demo Text', 14, 5, NULL, NULL, 7, N'B. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3264, N'', 14, 6, N'~/Resource/Images/HS-di-xe-may.jpg', NULL, 6, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3265, N'abs', 22, 1, NULL, NULL, 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3266, N'abc', 22, 2, NULL, NULL, 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3267, N'', 24, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3268, N'', 25, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3269, N'asdafdsfdsf', 26, 1, NULL, NULL, 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3270, N'Demo radio', 2023, 1, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3271, N'Demo Multiple 1', 2023, 2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3272, N'Demo Essay', 2023, 3, NULL, N'Demo Essay Detail', 5, N'4. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3273, N'Short Answer', 2023, 4, NULL, N'Demo Short answer detial', 4, N'3. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3274, N'Demo Text', 2023, 5, NULL, NULL, 6, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3275, N'', 2023, 6, N'~/Resource/Images/HS-di-xe-may.jpg', NULL, 5, N'B. ', NULL)
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
SET IDENTITY_INSERT [dbo].[TagInTest] ON 

GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1, 1, 14, 0)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (16, 4, 14, 1)
GO
SET IDENTITY_INSERT [dbo].[TagInTest] OFF
GO
SET IDENTITY_INSERT [dbo].[TagInQuestion] ON 

GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1, 1, 2201, NULL)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (2, 1, 2202, NULL)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (3, 1, 3270, NULL)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (4, 1, 3271, NULL)
GO
SET IDENTITY_INSERT [dbo].[TagInQuestion] OFF
GO
SET IDENTITY_INSERT [dbo].[UserInTest] ON 

GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (1, 14, 10, 4, CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (2, 14, 8, 5, CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (3, 14, 9, 6, CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[UserInTest] OFF
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2201, NULL, N'3264', CAST(1.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2202, NULL, N'3265,3266', CAST(2.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2225, N'xyz', NULL, NULL, NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2201, NULL, N'3263', CAST(0.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2202, NULL, N'3266', CAST(1.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2225, N'xyz', NULL, NULL, CAST(1.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2201, NULL, N'3263', CAST(0.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2202, NULL, N'3265,3266', CAST(2.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2225, N'xyz', NULL, NULL, CAST(1.00 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3263, N'Ans Radio 12', 2201, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3264, N'Ans Radio 2', 2201, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3265, N'Multiple 1', 2202, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3266, N'Multiple 2', 2202, 1, 1, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3268, N'xyz', 2203, 1, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3269, N'lhk', 2203, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3277, N'abc', 2202, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4357, N'kjshsd', 3265, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4358, N'kasjd', 3265, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4359, N'woeiqowue', 3266, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4360, N'lhk', 3266, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4361, N'', 3267, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4362, N'', 3267, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4363, N'', 3268, 0, NULL, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4364, N'', 3268, 0, NULL, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4365, N'sfdfsdfs', 3269, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4366, N'', 3269, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6357, N'Ans Radio 12', 3270, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6358, N'Ans Radio 2', 3270, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6359, N'Multiple 1', 3271, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6360, N'Multiple 2', 3271, 1, 1, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6361, N'abc', 3271, 0, 0, 1)
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
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (9, 3, 2, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (10, 3, 3, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (11, 3, 5, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (12, 3, 6, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (13, 3, 7, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (14, 3, 8, NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] OFF
GO
