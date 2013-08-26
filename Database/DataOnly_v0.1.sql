USE [OATSDB]
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
INSERT [dbo].[QuestionType] ([QuestionTypeID], [Type]) VALUES (7, N'Matching')
GO
SET IDENTITY_INSERT [dbo].[QuestionType] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] ON 

GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (1, N'Default Setting')
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [Name], [UserCountry], [LastLogin], [IsRegistered], [AccessToken]) VALUES (1, N'25-D5-5A-D2-83-AA-40-0A-F4-64-C7-6D-71-3C-07-AD', N'anhan60223@fpt.edu.vn', N'01203243364', N'an ngoc anh', N'viet nam', NULL, 1, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [Name], [UserCountry], [LastLogin], [IsRegistered], [AccessToken]) VALUES (2, N'25-D5-5A-D2-83-AA-40-0A-F4-64-C7-6D-71-3C-07-AD', N'thibt00721@fpt.edu.vn', N'0975454508', N'bui tuong thi', NULL, NULL, 1, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [Name], [UserCountry], [LastLogin], [IsRegistered], [AccessToken]) VALUES (3, N'25-D5-5A-D2-83-AA-40-0A-F4-64-C7-6D-71-3C-07-AD', N'akai777@gmail.com', NULL, N'huynh minh tu', NULL, NULL, 1, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [Name], [UserCountry], [LastLogin], [IsRegistered], [AccessToken]) VALUES (4, N'25-D5-5A-D2-83-AA-40-0A-F4-64-C7-6D-71-3C-07-AD', N'pk_lilo777@yahoo.com', NULL, N'nguyen duc tan', NULL, NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [SettingConfigID], [IsActive], [IsRunning], [IsComplete]) VALUES (1, N'Demo Test', NULL, 3, CAST(0x0000A22601609689 AS DateTime), CAST(0x0000A22601609689 AS DateTime), CAST(0x0000A22D01609689 AS DateTime), 1, 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Question] ON 

GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (1, N'Biet danh cua Vu Xuan Tien', 1, 1, NULL, N'', 1, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2, N'What animal?', 1, 2, NULL, N'', 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3, N'Phan do vui', 1, 5, NULL, N'', 0, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4, N'Cam xuc cua ban luc nay ???', 1, 3, NULL, N'Essay', 4, N'3. ', CAST(10.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (5, N'Who ''s Goden Ball 2012', 1, 4, NULL, N'Messi', 6, N'5. ', CAST(3.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (6, N'Matching', 1, 7, NULL, N'', 5, N'4. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (7, N'', 1, 6, N'~/Resource/Images/elefant_635131490996474542.jpg', N'', 2, N'B. ', NULL)
GO
SET IDENTITY_INSERT [dbo].[Question] OFF
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (1, N'bad man', 1, 0, CAST(0.00 AS Decimal(18, 2)), 0, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (2, N'iron man', 1, 1, CAST(1.00 AS Decimal(18, 2)), 1, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (3, N'elephant', 2, 1, CAST(1.00 AS Decimal(18, 2)), 0, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (4, N'cat', 2, 0, CAST(-0.50 AS Decimal(18, 2)), 1, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (5, N'cat', 2, 0, CAST(-0.50 AS Decimal(18, 2)), 2, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (6, N'Messi', 6, 1, CAST(1.00 AS Decimal(18, 2)), 0, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (7, N'C.Ronaldo', 6, 1, CAST(1.00 AS Decimal(18, 2)), 1, NULL)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (8, N'Argentina', 6, 0, NULL, NULL, 6)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (9, N'Potugal', 6, 0, NULL, NULL, 7)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder], [DependencyAnswerID]) VALUES (10, N'spider man', 1, 0, CAST(0.00 AS Decimal(18, 2)), 2, NULL)
GO
SET IDENTITY_INSERT [dbo].[Answer] OFF
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
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (6, N'Test')
GO
SET IDENTITY_INSERT [dbo].[Tag] OFF
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
SET IDENTITY_INSERT [dbo].[SettingTypes] ON 

GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (1, N'Test Access Control', 3, 0, N'RTC', N'Require test access code')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (2, N'Test Access Control', 3, 1, N'OSM', N'Attemp Limit')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (3, N'Test Formatting', 2, 0, N'ROQ', N'Shuffle question orders')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (4, N'Test Formatting', 2, 1, N'SQP', N'Show question point value')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (5, N'Results & Reporting', 1, 0, N'SAR', N'Students can access their own score reports')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (6, N'General Setting', 0, 0, N'MTP', N'Max Test Point')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (7, N'Test Formatting', 2, 2, N'NPP', N'Number Question Per Page')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (8, N'Test Formatting', 2, 2, N'RAO', N'Shuffle answer orders')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingGroupOrder], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (9, N'General Setting', 0, 1, N'DRL', N'Duration Limit')
GO
SET IDENTITY_INSERT [dbo].[SettingTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] ON 

GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1, 1, 1, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (2, 1, 2, 1, N'Recent', 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (3, 1, 3, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (4, 1, 4, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (5, 1, 5, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (6, 1, 6, 10, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (7, 1, 7, 5, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (8, 1, 8, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (9, 1, 9, 10, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] OFF
GO
