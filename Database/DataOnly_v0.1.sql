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
SET IDENTITY_INSERT [dbo].[SettingConfig] ON 

GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (1, N'Default Setting')
GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (3, N'SettingForTest_14')
GO
INSERT [dbo].[SettingConfig] ([SettingConfigID], [Description]) VALUES (4, N'SettingForTest_2025')
GO
SET IDENTITY_INSERT [dbo].[SettingConfig] OFF
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
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3024, NULL, N'akai777@gmail.com', NULL, NULL, N'HMT', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3025, NULL, N'simon77@gmail.com', NULL, NULL, N'HMT', NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3026, NULL, N'', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3027, NULL, N'', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3028, NULL, N'', NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [LastLogin]) VALUES (3029, N'01647373064', N'dangtrinhnhatha@gmail.com', N'01647373064', N'Nhat Ha', N'Dang Trinh', N'Viet Nam', NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3, N'Sample Test Viet Nam', NULL, 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (4, N'Test History of Viet Nam', NULL, 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (14, N'







Lession 123                    ', N'gi', 1, CAST(0x0000A1C80102E985 AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E400993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (15, N'demo', NULL, 1, CAST(0x0000A1D000993D9A AS DateTime), CAST(0x0000A1D000993D9A AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (16, N'Test Demos', NULL, 1, CAST(0x0000A1D0009F4759 AS DateTime), CAST(0x0000A1D0009F4759 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (18, N'Bai test 3', NULL, 1, CAST(0x0000A1D30110972F AS DateTime), CAST(0x0000A1D30110972F AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (19, N'test 20', NULL, 1, CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (20, N'bai test 20', NULL, 1, CAST(0x0000A1D30121A97E AS DateTime), CAST(0x0000A1D40121A97E AS DateTime), CAST(0x0000A1ED0121A97E AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (21, N'Test 21', NULL, 1, CAST(0x0000A1D30121E013 AS DateTime), CAST(0x0000A1D301139A86 AS DateTime), CAST(0x0000A1D401139A86 AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (22, N'Test so 22', NULL, 1, CAST(0x0000A1DD016B650D AS DateTime), CAST(0x0000A1DD016B650D AS DateTime), CAST(0x0000A1E9016B650D AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (23, N'bai test 23', NULL, 1, CAST(0x0000A1DD01715DE6 AS DateTime), CAST(0x0000A1DD01715DE6 AS DateTime), NULL, NULL, 1, 0)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (24, N'bai test 24', NULL, 1, CAST(0x0000A1DD01732A91 AS DateTime), CAST(0x0000A1DD01732A91 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (25, N'bai test 25', NULL, 1, CAST(0x0000A1DD0176EBE7 AS DateTime), CAST(0x0000A1DD0176EBE7 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (26, N'sdfsdf', NULL, 1, CAST(0x0000A1DD0178C304 AS DateTime), CAST(0x0000A1DD0178C304 AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (2023, N'
test 321', NULL, 1, CAST(0x0000A1E801614B0C AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E500993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (2025, N'Demo Test', NULL, 1, CAST(0x0000A1EB0117CB33 AS DateTime), CAST(0x0000A1EB0117CB33 AS DateTime), NULL, NULL, 4, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (2026, N'Lession 456', NULL, 1, CAST(0x0000A1EF015D7DA5 AS DateTime), CAST(0x0000A1EF015D7DA5 AS DateTime), CAST(0x0000A1F4015D7DA5 AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3026, N'test 456', NULL, 1, CAST(0x0000A1F0007B0D08 AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E300993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3027, N'





Lession 999                        ', N'summary introduction of test Lession 999', 1, CAST(0x0000A1F100B7B7D4 AS DateTime), CAST(0x0000A1F100B7B7D4 AS DateTime), CAST(0x0000A1FF00B7B7D4 AS DateTime), NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3028, N'
test 123', NULL, 1, CAST(0x0000A1F201678B29 AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E400993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3029, N'COPY: COPY: 

Lession 321', NULL, 1, CAST(0x0000A1F201678FDC AS DateTime), CAST(0x0000A1DE00993D9A AS DateTime), CAST(0x0000A1E400993D9A AS DateTime), NULL, 3, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3030, N'test 777', NULL, 1, CAST(0x0000A1F3009E96FC AS DateTime), CAST(0x0000A1F3009E96FC AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3031, N'test 1000', NULL, 1, CAST(0x0000A1F400CBD3DE AS DateTime), CAST(0x0000A1F400CBD3DE AS DateTime), NULL, NULL, 1, 1)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [Introduction], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration], [SettingConfigID], [IsActive]) VALUES (3032, N'Test 888', NULL, 1, CAST(0x0000A1F501035F3E AS DateTime), CAST(0x0000A1F501035F3E AS DateTime), NULL, NULL, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Invitation] ON 

GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (101, 23, 2023, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (102, 23, 8, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (103, 24, 2025, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3102, 14, 10, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3103, 14, 12, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3104, 14, 13, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3105, 14, 14, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3106, 14, 1013, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3107, 14, 8, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (3108, 14, 9, 3, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4100, 14, 3025, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4101, 18, 3025, 3, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4102, 20, 3025, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4103, 2025, 10, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4104, 2025, 12, 2, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4105, 2025, 8, 3, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (4106, 2025, 9, 3, CAST(0x0000A1F000000000 AS DateTime), NULL)
GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [RoleID], [InvitationDateTime], [AccessToken]) VALUES (5102, 3031, 1, 2, CAST(0x0000A1F400CC8279 AS DateTime), NULL)
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
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2201, N'Demo hmt radio', 14, 1, NULL, NULL, 2, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2202, N'Demo Multiple 1', 14, 2, NULL, NULL, 3, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2203, N'abc', 18, 1, NULL, NULL, 2, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2223, N'', 21, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2224, N'Demo Essay', 14, 3, NULL, N'Demo Essay Detail', 4, N'4. ', CAST(6.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (2225, N'Short Answer', 14, 4, NULL, N'Demo Short answer detial', 5, N'5. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3265, N'abs', 22, 1, NULL, NULL, 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3266, N'abc', 22, 2, NULL, NULL, 2, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3267, N'', 24, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3268, N'abcded', 25, 1, NULL, NULL, 0, NULL, NULL)
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
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3318, N'radió', 2025, 1, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3319, N'mul', 2025, 2, NULL, NULL, 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3320, N'esse', 2025, 3, NULL, N'essay detail', 4, N'3. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3321, N'short', 2025, 4, NULL, N'shortans', 5, N'4. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3322, N'', 2025, 6, N'~/Resource/Images/HS-di-xe-may.jpg', NULL, 6, N'A. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (3361, N'abc', 2026, 4, N'~/Resource/Images/kien.jpg', N'fdsf', 2, N'1. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4277, N'Demo radio 1', 3026, 1, NULL, NULL, 3, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4278, N'Demo Multiple 1', 3026, 2, NULL, NULL, 2, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4279, N'Demo Essay', 3026, 3, NULL, N'Demo Essay Detail', 5, N'4. ', CAST(6.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4280, N'Short Answer', 3026, 4, NULL, N'Demo Short answer detial', 4, N'3. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4452, N'Demo radio 1', 3028, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4453, N'Demo Multiple 1', 3028, 2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4454, N'Demo Essay', 3028, 3, NULL, N'Demo Essay Detail', 3, N'3. ', CAST(6.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4455, N'Short Answer', 3028, 4, NULL, N'Demo Short answer detial', 0, NULL, CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4456, N'Demo radio 1', 3029, 1, NULL, NULL, 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4457, N'Demo Multiple 1', 3029, 2, NULL, NULL, 0, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4458, N'Demo Essay', 3029, 3, NULL, N'Demo Essay Detail', 3, N'3. ', CAST(6.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4459, N'Short Answer', 3029, 4, NULL, N'Demo Short answer detial', 4, N'4. ', CAST(5.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4461, N'Demo abcradio', 14, 2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4472, N'Demo xyzradio', 14, 2, NULL, NULL, 1, NULL, NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4473, N'radio choice', 3030, 1, NULL, N'', 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4474, N'mul', 3030, 2, NULL, N'', 1, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4475, N'cau hoi 1
', 3031, 1, NULL, N'', 0, N'1. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4476, N'cau hoi 2
', 3031, 1, NULL, N'', 2, N'2. ', NULL)
GO
INSERT [dbo].[Question] ([QuestionID], [QuestionTitle], [TestID], [QuestionTypeID], [ImageUrl], [TextDescription], [SerialOrder], [LabelOrder], [NoneChoiceScore]) VALUES (4477, N'', 3031, 6, N'~/Resource/Images/303484_338113126255253_100001697060431_888654_907280013_n.jpg', N'', 1, N'A. ', NULL)
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
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1002, N'Test')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1003, N'Temp')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1004, N'super')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1005, N'kingkong')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1006, N'jimmy')
GO
INSERT [dbo].[Tag] ([TagID], [TagName]) VALUES (1007, N'demo')
GO
SET IDENTITY_INSERT [dbo].[Tag] OFF
GO
SET IDENTITY_INSERT [dbo].[TagInTest] ON 

GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1, 1, 14, 1)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (17, 1, 2025, 1)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (18, 3, 2025, 0)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1006, 1, 3026, 0)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1007, 4, 3026, 1)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1008, 4, 14, 2)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1009, 1, 3028, 1)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1010, 4, 3028, 2)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1011, 1, 3029, 1)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1012, 4, 3029, 2)
GO
INSERT [dbo].[TagInTest] ([TagInTestID], [TagID], [TestID], [SerialOrder]) VALUES (1015, 1004, 14, 3)
GO
SET IDENTITY_INSERT [dbo].[TagInTest] OFF
GO
SET IDENTITY_INSERT [dbo].[TagInQuestion] ON 

GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (10, 1, 3318, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (11, 1, 2202, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (54, 4, 2201, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (59, 4, 2225, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (60, 4, 2224, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1005, 2, 2202, 1)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1039, 1007, 2201, 1)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1042, 4, 4461, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1043, 1007, 4461, 1)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1044, 4, 4472, 0)
GO
INSERT [dbo].[TagInQuestion] ([TagInQuestionID], [TagID], [QuestionID], [SerialOrder]) VALUES (1045, 1007, 4472, 1)
GO
SET IDENTITY_INSERT [dbo].[TagInQuestion] OFF
GO
SET IDENTITY_INSERT [dbo].[UserInTest] ON 

GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (1, 14, 10, CAST(4.00 AS Decimal(18, 2)), CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (2, 14, 8, CAST(5.00 AS Decimal(18, 2)), CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
INSERT [dbo].[UserInTest] ([UserInTestID], [TestID], [UserID], [Score], [TestTakenDate], [NumberOfAttend]) VALUES (3, 14, 9, CAST(6.00 AS Decimal(18, 2)), CAST(0x0000A1DC00000000 AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[UserInTest] OFF
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2201, NULL, N'3264', CAST(0.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2202, NULL, N'3265,3266', CAST(2.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (1, 2225, N'xyz', NULL, NULL, NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2201, NULL, N'3263', CAST(1.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2202, NULL, N'3266', CAST(1.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (2, 2225, N'xyz', NULL, NULL, CAST(1.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2201, NULL, N'3263', CAST(1.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2202, NULL, N'3265,3266', CAST(2.00 AS Decimal(18, 2)), NULL)
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2224, N'abc', NULL, NULL, CAST(2.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[UserInTestDetail] ([UserInTestID], [QuestionID], [AnswerContent], [AnswerIDs], [ChoiceScore], [NonChoiceScore]) VALUES (3, 2225, N'xyz', NULL, NULL, CAST(1.00 AS Decimal(18, 2)))
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
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (10, N'General Setting', 0, N'MXP', N'Max Test Point')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (11, N'General Setting', 1, N'NPP', N'Number Question Per Page')
GO
INSERT [dbo].[SettingTypes] ([SettingTypeID], [SettingGroupName], [SettingInGroupOrder], [SettingTypeKey], [SettingTypeDescription]) VALUES (12, N'Test Formatting', 2, N'RAO', N'Random answer orders')
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
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (9, 3, 2, NULL, N'dFB6dhdi', 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (10, 3, 3, 15, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (11, 3, 5, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (12, 3, 6, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (13, 3, 7, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (14, 3, 8, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (15, 4, 2, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (16, 4, 3, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (17, 4, 5, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (18, 4, 6, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (19, 4, 7, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (20, 4, 8, NULL, NULL, 0)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1015, 1, 10, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1016, 3, 10, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1017, 4, 10, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1018, 1, 11, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1019, 3, 11, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1020, 4, 11, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1021, 1, 12, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1022, 3, 12, NULL, NULL, 1)
GO
INSERT [dbo].[SettingConfigDetail] ([SettingConfigDetailID], [SettingConfigID], [SettingTypeID], [NumberValue], [TextValue], [IsActive]) VALUES (1023, 4, 12, NULL, NULL, 1)
GO
SET IDENTITY_INSERT [dbo].[SettingConfigDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3263, N'Ans Radio 12', 2201, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3264, N'Ans Radio 2', 2201, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3265, N'Multiple 1', 2202, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3266, N'Multiple 2', 2202, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3268, N'xyz', 2203, 1, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3269, N'lhk', 2203, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (3277, N'abc', 2202, 1, 1, 0)
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
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4363, N'radio1', 3268, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (4364, N'radio2', 3268, 0, 0, 1)
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
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6373, N'ans radio 3', 2201, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6440, N'radio1', 3318, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6441, N'radio2', 3318, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6442, N'mul1', 3319, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6443, N'mul2', 3319, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6444, N'radio3', 3318, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6445, N'mul3', 3319, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6565, N'xyz', 3361, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (6566, N'lk', 3361, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7357, N'Ans Radio 12', 4277, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7358, N'Ans Radio 2', 4277, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7359, N'ans radio 3', 4277, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7360, N'Multiple 1', 4278, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7361, N'Multiple 2', 4278, 1, 1, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7362, N'abc', 4278, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7820, N'Ans Radio 12', 4452, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7821, N'Ans Radio 2', 4452, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7822, N'ans radio 3', 4452, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7823, N'Multiple 1', 4453, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7824, N'Multiple 2', 4453, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7825, N'abc', 4453, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7826, N'Ans Radio 12', 4456, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7827, N'Ans Radio 2', 4456, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7828, N'ans radio 3', 4456, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7829, N'Multiple 1', 4457, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7830, N'Multiple 2', 4457, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7831, N'abc', 4457, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7835, N'Ans Radio 12', 4461, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7836, N'Ans Radio 2', 4461, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7837, N'ans radio 3', 4461, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7850, N'Ans Radio 12', 4472, 1, 1, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7851, N'Ans Radio 2', 4472, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7852, N'ans radio 3', 4472, 0, 0, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7853, N'radio1', 4473, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7854, N'radio2', 4473, 1, 5, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7855, N'mul1', 4474, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7856, N'mul2', 4474, 0, -2, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7857, N'mul3', 4474, 1, 1, 2)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7858, N'dap an 1
', 4475, 1, 1, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7859, N'dap an 2', 4475, 0, 0, 1)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7860, N'dap an 1
', 4476, 0, 0, 0)
GO
INSERT [dbo].[Answer] ([AnswerID], [AnswerContent], [QuestionID], [IsRight], [Score], [SerialOrder]) VALUES (7861, N'dap an 2', 4476, 1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Answer] OFF
GO
SET IDENTITY_INSERT [dbo].[FeedBack] ON 

GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1, 1, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1ED00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (2, 8, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1DA00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (3, 9, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1E400000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (4, 10, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1DC00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (5, 12, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1DD00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (6, 13, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1DE00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (7, 14, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1DF00000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (8, 8, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1E000000000 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (9, 8, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1E100000000 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (10, 8, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1E200000000 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (11, 8, 14, N'this a negative lookahead. Like all lookarounds, it''s a zero-width assertion, which just means it matches a position between characters rather than a pattern of characters themselves. So this matches "a position that is not followed by Country". This means if you had a country named CountryTown, it wouldn''t get matched by the full regex', CAST(0x0000A1E300000000 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1002, 8, 14, N'abc', CAST(0x0000A1F3000534F6 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1003, 8, 14, N'abc', CAST(0x0000A1F300058EFE AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1004, 8, 14, N'lkh', CAST(0x0000A1F3000682AF AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1005, 8, 14, N'lkh', CAST(0x0000A1F30006DC9F AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1006, 8, 14, N'lkh', CAST(0x0000A1F30007D517 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1007, 8, 14, N'lkh', CAST(0x0000A1F3000AAA1B AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1009, 8, 14, N'lkh', CAST(0x0000A1F3000DE485 AS DateTime), 8)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1010, 8, 14, N'akai', CAST(0x0000A1F30010E051 AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1011, 8, 14, N'sida', CAST(0x0000A1F3001158BE AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1012, 8, 14, N'sida', CAST(0x0000A1F300115BDB AS DateTime), NULL)
GO
INSERT [dbo].[FeedBack] ([FeedBackID], [UserID], [TestID], [FeedBackDetail], [FeedBackDateTime], [ParentID]) VALUES (1013, 1, 14, N'abc', CAST(0x0000A1F300973196 AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[FeedBack] OFF
GO
