USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[GlobalSetting]    Script Date: 04/07/2013 06:13:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[QA_Automation_Test].[GlobalSetting]') AND type in (N'U'))
DROP TABLE [QA_Automation_Test].[GlobalSetting]
GO

USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[GlobalSetting]    Script Date: 04/07/2013 06:13:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [QA_Automation_Test].[GlobalSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [varchar](100) NULL,
 CONSTRAINT [PK_GlobalSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


