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
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (1, N'12345', N'akai777@gmail.com', N'01285591685', N'Huynh', N'Tu', N'Binh Duong', 2)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (8, NULL, N'abc@abc.com', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (9, NULL, N'xyz@xyz.com', NULL, NULL, NULL, NULL, 3)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (10, NULL, N'hmt@hmt.com', NULL, NULL, NULL, NULL, 2)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (12, NULL, N'', NULL, NULL, NULL, NULL, 3)
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
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Question] ON 

GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (51, N'one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 0, N'1. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (53, N'abc', 14, 3, NULL, N'123', CAST(0.00 AS Decimal(18, 2)), 2, N'3. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (54, N'abc', 14, 4, NULL, N'456', CAST(0.00 AS Decimal(18, 2)), 5, N'6. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (55, N'343slkd', 14, 5, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 7, N'B. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (56, N'', 14, 6, N'~/Resource/Images/kien.jpg', NULL, CAST(0.00 AS Decimal(18, 2)), 6, N'A. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (58, N'one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 3, N'4. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (59, N'one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 4, N'5. ')
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [QuestionScore], [SerialOrder], [LabelOrder]) VALUES (62, N'one', 14, 1, NULL, NULL, CAST(0.00 AS Decimal(18, 2)), 1, N'2. ')
GO
SET IDENTITY_INSERT [dbo].[Question] OFF
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (120, N'mot', 51, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (121, N'1', 51, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (122, N'ba', 51, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (130, N'mot', 58, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (131, N'1', 58, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (132, N'ba', 58, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (133, N'mot', 59, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (134, N'1', 59, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (135, N'ba', 59, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (142, N'mot', 62, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (143, N'1', 62, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight]) VALUES (144, N'ba', 62, 0)
GO
SET IDENTITY_INSERT [dbo].[Answer] OFF
GO
