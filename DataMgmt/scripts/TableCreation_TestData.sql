USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[TestData]    Script Date: 03/01/2013 04:19:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[QA_Automation_Test].[TestData]') AND type in (N'U'))
DROP TABLE [QA_Automation_Test].[TestData]
GO

USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[TestData]    Script Date: 03/01/2013 04:19:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [QA_Automation_Test].[TestData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[TestCaseId] [varchar](10) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


