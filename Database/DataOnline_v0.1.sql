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
INSERT [dbo].[User] ([UserID], [Password], [UserMail], [UserPhone], [LastName], [FirstName], [UserCountry], [RoleID]) VALUES (2, N'11111', N'anhan@yahoo.com', N'01203243364', N'Anh', N'An', N'Dong Nai', 3)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Test] ON 

GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (3, N'Sample Test Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60)
GO
INSERT [dbo].[Test] ([TestID], [TestTitle], [CreatedUserID], [CreatedDateTime], [StartDateTime], [EndDateTime], [Duration]) VALUES (4, N'Test History of Viet Nam', 1, CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1C100000000 AS DateTime), CAST(0x0000A1CD00000000 AS DateTime), 60)
GO
SET IDENTITY_INSERT [dbo].[Test] OFF
GO
SET IDENTITY_INSERT [dbo].[Invitation] ON 

GO
INSERT [dbo].[Invitation] ([InvitationID], [TestID], [UserID], [InvitationDateTime]) VALUES (2, 3, 2, CAST(0x0000A1C400000000 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Invitation] OFF
GO
