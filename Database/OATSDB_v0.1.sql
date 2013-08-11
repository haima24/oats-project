use master
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'OATSDB')
DROP DATABASE [OATSDB]
GO

CREATE DATABASE [OATSDB]
GO

USE [OATSDB]
GO
/****** Object:  Table [dbo].[Answer]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answer](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[AnswerContent] [nvarchar](1024) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[IsRight] [bit] NOT NULL,
	[Score] [int] NULL,
	[SerialOrder] [int] NULL,
	[DependencyAnswerID] [int] NULL,
 CONSTRAINT [PK_Answer_1] PRIMARY KEY CLUSTERED 
(
	[AnswerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeedBack]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedBack](
	[FeedBackID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[TestID] [int] NOT NULL,
	[FeedBackDetail] [nvarchar](1024) NULL,
	[FeedBackDateTime] [datetime] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_FeedBack] PRIMARY KEY CLUSTERED 
(
	[FeedBackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Invitation]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invitation](
	[InvitationID] [int] IDENTITY(1,1) NOT NULL,
	[TestID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NULL,
	[InvitationDateTime] [datetime] NULL,
	[IsMailSent] [bit] NOT NULL,
	[AccessToken] [varchar](128) NULL,
 CONSTRAINT [PK_Invitation] PRIMARY KEY CLUSTERED 
(
	[InvitationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Question]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Question](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionTitle] [nvarchar](1024) NOT NULL,
	[TestID] [int] NOT NULL,
	[QuestionTypeID] [int] NOT NULL,
	[ImageUrl] [varchar](1024) NULL,
	[TextDescription] [nvarchar](1024) NULL,
	[SerialOrder] [int] NULL,
	[LabelOrder] [varchar](10) NULL,
	[NoneChoiceScore] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[QuestionType]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QuestionType](
	[QuestionTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NOT NULL,
 CONSTRAINT [PK_QuestionType] PRIMARY KEY CLUSTERED 
(
	[QuestionTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleDescription] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettingConfig]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingConfig](
	[SettingConfigID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_SettingConfig] PRIMARY KEY CLUSTERED 
(
	[SettingConfigID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettingConfigDetail]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingConfigDetail](
	[SettingConfigDetailID] [int] IDENTITY(1,1) NOT NULL,
	[SettingConfigID] [int] NOT NULL,
	[SettingTypeID] [int] NOT NULL,
	[NumberValue] [int] NULL,
	[TextValue] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SettingConfigDetail] PRIMARY KEY CLUSTERED 
(
	[SettingConfigDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettingTypes]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SettingTypes](
	[SettingTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SettingGroupName] [nvarchar](512) NULL,
	[SettingGroupOrder] [int] NULL,
	[SettingInGroupOrder] [int] NULL,
	[SettingTypeKey] [varchar](10) NOT NULL,
	[SettingTypeDescription] [nvarchar](512) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[SettingTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TagInQuestion]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagInQuestion](
	[TagInQuestionID] [int] IDENTITY(1,1) NOT NULL,
	[TagID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[SerialOrder] [int] NULL,
 CONSTRAINT [PK_TagInQuestion] PRIMARY KEY CLUSTERED 
(
	[TagInQuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TagInTest]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagInTest](
	[TagInTestID] [int] IDENTITY(1,1) NOT NULL,
	[TagID] [int] NOT NULL,
	[TestID] [int] NOT NULL,
	[SerialOrder] [int] NULL,
 CONSTRAINT [PK_TagInTest_1] PRIMARY KEY CLUSTERED 
(
	[TagInTestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Test]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Test](
	[TestID] [int] IDENTITY(1,1) NOT NULL,
	[TestTitle] [nvarchar](512) NOT NULL,
	[Introduction] [nvarchar](1024) NULL,
	[CreatedUserID] [int] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NULL,
	[SettingConfigID] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsRunning] [bit] NOT NULL,
	[IsComplete] [bit] NULL,
 CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
(
	[TestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Password] [varchar](128) NULL,
	[UserMail] [varchar](256) NOT NULL,
	[UserPhone] [varchar](128) NULL,
	[Name] [nvarchar](128) NOT NULL,
	[UserCountry] [nvarchar](128) NULL,
	[LastLogin] [datetime] NULL,
	[IsRegistered] [bit] NOT NULL,
	[AccessToken] [varchar](128) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserInTest]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInTest](
	[UserInTestID] [int] IDENTITY(1,1) NOT NULL,
	[TestID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Score] [decimal](18, 2) NULL,
	[TestTakenDate] [datetime] NULL,
	[NumberOfAttend] [int] NOT NULL,
 CONSTRAINT [PK_UserInTest] PRIMARY KEY CLUSTERED 
(
	[UserInTestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserInTestDetail]    Script Date: 11/08/2013 9:43:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserInTestDetail](
	[UserInTestID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[AnswerContent] [nvarchar](1024) NULL,
	[AnswerIDs] [varchar](256) NULL,
	[ChoiceScore] [decimal](18, 2) NULL,
	[NonChoiceScore] [decimal](18, 2) NULL,
 CONSTRAINT [PK_UserInTestDetail] PRIMARY KEY CLUSTERED 
(
	[UserInTestID] ASC,
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[UserInTest] ADD  CONSTRAINT [DF_UserInTest_score]  DEFAULT ((0)) FOR [Score]
GO
ALTER TABLE [dbo].[UserInTest] ADD  CONSTRAINT [DF_UserInTest_numberOfAttend]  DEFAULT ((0)) FOR [NumberOfAttend]
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_Answer] FOREIGN KEY([DependencyAnswerID])
REFERENCES [dbo].[Answer] ([AnswerID])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_Answer]
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_Question] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Question] ([QuestionID])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_Question]
GO
ALTER TABLE [dbo].[FeedBack]  WITH CHECK ADD  CONSTRAINT [FK_FeedBack_FeedBack] FOREIGN KEY([ParentID])
REFERENCES [dbo].[FeedBack] ([FeedBackID])
GO
ALTER TABLE [dbo].[FeedBack] CHECK CONSTRAINT [FK_FeedBack_FeedBack]
GO
ALTER TABLE [dbo].[FeedBack]  WITH CHECK ADD  CONSTRAINT [FK_FeedBack_Test] FOREIGN KEY([TestID])
REFERENCES [dbo].[Test] ([TestID])
GO
ALTER TABLE [dbo].[FeedBack] CHECK CONSTRAINT [FK_FeedBack_Test]
GO
ALTER TABLE [dbo].[FeedBack]  WITH CHECK ADD  CONSTRAINT [FK_FeedBack_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[FeedBack] CHECK CONSTRAINT [FK_FeedBack_User]
GO
ALTER TABLE [dbo].[Invitation]  WITH CHECK ADD  CONSTRAINT [FK_Invitation_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
GO
ALTER TABLE [dbo].[Invitation] CHECK CONSTRAINT [FK_Invitation_Role]
GO
ALTER TABLE [dbo].[Invitation]  WITH CHECK ADD  CONSTRAINT [FK_Invitation_Test] FOREIGN KEY([TestID])
REFERENCES [dbo].[Test] ([TestID])
GO
ALTER TABLE [dbo].[Invitation] CHECK CONSTRAINT [FK_Invitation_Test]
GO
ALTER TABLE [dbo].[Invitation]  WITH CHECK ADD  CONSTRAINT [FK_Invitation_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Invitation] CHECK CONSTRAINT [FK_Invitation_User]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_QuestionType] FOREIGN KEY([QuestionTypeID])
REFERENCES [dbo].[QuestionType] ([QuestionTypeID])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_QuestionType]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Test] FOREIGN KEY([TestID])
REFERENCES [dbo].[Test] ([TestID])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Test]
GO
ALTER TABLE [dbo].[SettingConfigDetail]  WITH CHECK ADD  CONSTRAINT [FK_SettingConfigDetail_SettingConfig] FOREIGN KEY([SettingConfigID])
REFERENCES [dbo].[SettingConfig] ([SettingConfigID])
GO
ALTER TABLE [dbo].[SettingConfigDetail] CHECK CONSTRAINT [FK_SettingConfigDetail_SettingConfig]
GO
ALTER TABLE [dbo].[SettingConfigDetail]  WITH CHECK ADD  CONSTRAINT [FK_SettingConfigDetail_SettingTypes] FOREIGN KEY([SettingTypeID])
REFERENCES [dbo].[SettingTypes] ([SettingTypeID])
GO
ALTER TABLE [dbo].[SettingConfigDetail] CHECK CONSTRAINT [FK_SettingConfigDetail_SettingTypes]
GO
ALTER TABLE [dbo].[TagInQuestion]  WITH CHECK ADD  CONSTRAINT [FK_TagInQuestion_Question] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Question] ([QuestionID])
GO
ALTER TABLE [dbo].[TagInQuestion] CHECK CONSTRAINT [FK_TagInQuestion_Question]
GO
ALTER TABLE [dbo].[TagInQuestion]  WITH CHECK ADD  CONSTRAINT [FK_TagInQuestion_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
ALTER TABLE [dbo].[TagInQuestion] CHECK CONSTRAINT [FK_TagInQuestion_Tag]
GO
ALTER TABLE [dbo].[TagInTest]  WITH CHECK ADD  CONSTRAINT [FK_TagInTest_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
ALTER TABLE [dbo].[TagInTest] CHECK CONSTRAINT [FK_TagInTest_Tag]
GO
ALTER TABLE [dbo].[TagInTest]  WITH CHECK ADD  CONSTRAINT [FK_TagInTest_Test] FOREIGN KEY([TestID])
REFERENCES [dbo].[Test] ([TestID])
GO
ALTER TABLE [dbo].[TagInTest] CHECK CONSTRAINT [FK_TagInTest_Test]
GO
ALTER TABLE [dbo].[Test]  WITH CHECK ADD  CONSTRAINT [FK_Test_SettingConfig] FOREIGN KEY([SettingConfigID])
REFERENCES [dbo].[SettingConfig] ([SettingConfigID])
GO
ALTER TABLE [dbo].[Test] CHECK CONSTRAINT [FK_Test_SettingConfig]
GO
ALTER TABLE [dbo].[Test]  WITH CHECK ADD  CONSTRAINT [FK_Test_User] FOREIGN KEY([CreatedUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Test] CHECK CONSTRAINT [FK_Test_User]
GO
ALTER TABLE [dbo].[UserInTest]  WITH CHECK ADD  CONSTRAINT [FK_UserInTest_Test] FOREIGN KEY([TestID])
REFERENCES [dbo].[Test] ([TestID])
GO
ALTER TABLE [dbo].[UserInTest] CHECK CONSTRAINT [FK_UserInTest_Test]
GO
ALTER TABLE [dbo].[UserInTest]  WITH CHECK ADD  CONSTRAINT [FK_UserInTest_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[UserInTest] CHECK CONSTRAINT [FK_UserInTest_User]
GO
ALTER TABLE [dbo].[UserInTestDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserInTestDetail_Question] FOREIGN KEY([QuestionID])
REFERENCES [dbo].[Question] ([QuestionID])
GO
ALTER TABLE [dbo].[UserInTestDetail] CHECK CONSTRAINT [FK_UserInTestDetail_Question]
GO
ALTER TABLE [dbo].[UserInTestDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserInTestDetail_UserInTest] FOREIGN KEY([UserInTestID])
REFERENCES [dbo].[UserInTest] ([UserInTestID])
GO
ALTER TABLE [dbo].[UserInTestDetail] CHECK CONSTRAINT [FK_UserInTestDetail_UserInTest]
GO
