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
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (8, NULL, N'abc@abc.com', NULL, NULL, NULL, NULL, 3, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID], [LastLogin]) VALUES (9, NULL, N'xyz@xyz.com', NULL, NULL, NULL, NULL, 3, NULL)
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
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (3, N'Sample Test Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (4, N'Test History of Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (12, N'', 1, CAST(0x0000A1C800FF4913 AS DateTime), CAST(0x0000A1C800FF4913 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (13, N'', 1, CAST(0x0000A1C80100B0CA AS DateTime), CAST(0x0000A1C80100B0CA AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (14, N'Lession 123', 1, CAST(0x0000A1C80102E985 AS DateTime), CAST(0x0000A1C80102E985 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (15, N'demo', 1, CAST(0x0000A1D000993D9A AS DateTime), CAST(0x0000A1D000993D9A AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (16, N'Test Demo', 1, CAST(0x0000A1D0009F4759 AS DateTime), CAST(0x0000A1D0009F4759 AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Question] ON 

GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1077, N'One plus one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1078, N'Five plus five', 14, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 5, N'4. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1080, N'Demo Essay Title', 14, 3, NULL, N'Essay Detail', CAST(0.00 AS Decimal(18, 2)), 4, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1081, N'Demo ShortAnswer', 14, 4, NULL, N'Short Answer', CAST(0.00 AS Decimal(18, 2)), 6, N'5. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1082, N'Demo Text', 14, 5, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 7, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1083, N'', 14, 6, N'~/Resource/Images/kien.jpg', NULL, CAST(0.00 AS Decimal(18, 2)), 8, N'B. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1098, N'Demo ShortAnswer', 14, 4, NULL, N'Short Answer', CAST(0.00 AS Decimal(18, 2)), 9, N'6. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1099, N'Demo Essay Title', 14, 3, NULL, N'Essay Detail', CAST(0.00 AS Decimal(18, 2)), 10, N'7. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2063, N'', 15, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2064, N'', 15, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 2, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2065, N'<i>Enter Question</i>', 15, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2066, N'One plus one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 11, N'8. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2067, N'Five plus five', 14, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 12, N'9. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2068, N'One plus one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2069, N'1+1', 16, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2070, N'3+1', 16, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 2, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2071, N'Essay demo', 16, 3, NULL, N'Detail', CAST(0.00 AS Decimal(18, 2)), 3, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2072, N'shortanswe', 16, 4, NULL, N'hello', CAST(0.00 AS Decimal(18, 2)), 5, N'4. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2073, N'dajkshdjsadahdas assa', 16, 5, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 6, N'B. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2074, N'', 16, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 7, N'5. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2075, N'', 16, 6, N'~/Resource/Images/HS-di-xe-may.jpg', NULL, CAST(0.00 AS Decimal(18, 2)), 4, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2076, N'shortanswe', 16, 4, NULL, N'hello', CAST(0.00 AS Decimal(18, 2)), 8, N'6. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2077, N'3+1', 16, 2, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 9, N'7. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2078, N'hdjkashd', 16, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 10, N'8. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2079, N'dayuiqwer', 16, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 11, N'9. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2080, N'qeiowueoiqwueoiqweiouqoiwue', 16, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 12, N'10. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2081, N'abcdjkashjdksahdjkashdas?', 16, 5, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 9, N'C. ', NULL)
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
INSERT [dbo].[TagInQuestion] ([TagID], [QuestionID]) VALUES (1, 1077)
GO
INSERT [dbo].[TagInQuestion] ([TagID], [QuestionID]) VALUES (1, 1078)
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2035, N'1', 1077, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2036, N'2', 1077, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2037, N'3', 1077, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2038, N'Ten', 1078, 1, 1, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2039, N'10', 1078, 1, 1, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2040, N'Seven', 1078, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (2041, N'eight', 1078, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3006, N'', 2063, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3007, N'', 2063, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3008, N'', 2064, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3009, N'', 2064, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3010, N'<i>Enter Answer</i>', 2065, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3011, N'<i>Enter Answer</i>', 2065, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3012, N'1', 2066, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3013, N'2', 2066, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3014, N'3', 2066, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3015, N'Ten', 2067, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3016, N'10', 2067, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3017, N'Seven', 2067, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3018, N'eight', 2067, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3019, N'1', 2068, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3020, N'2', 2068, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3021, N'3', 2068, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3022, N'2', 2069, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3023, N'3', 2069, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3024, N'4', 2070, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3025, N'bon', 2070, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3026, N'nam', 2070, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3027, N'', 2074, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3028, N'', 2074, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3029, N'4', 2077, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3030, N'bon', 2077, 1, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3031, N'nam', 2077, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3032, N'askdjhasd', 2078, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3033, N'asdjaksd', 2078, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3034, N'dshjkasd', 2078, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3035, N'fdshfi', 2079, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3036, N'roieworwe', 2079, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3037, N'reoqiuoiqwe', 2079, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3038, N'rwieo', 2079, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3039, N'rweioruwoier', 2079, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3040, N'roewiurowieurweiw', 2080, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3041, N'eqweiowuqeoiquwe', 2080, 0, NULL, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3042, N'lksdflksdjflksjdf', 2080, 0, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Answer] OFF
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
