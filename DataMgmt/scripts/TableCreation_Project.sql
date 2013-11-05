USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[Project]    Script Date: 03/25/2013 14:32:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[QA_Automation_Test].[Project]') AND type in (N'U'))
DROP TABLE [QA_Automation_Test].[Project]
GO

USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[Project]    Script Date: 03/25/2013 14:32:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [QA_Automation_Test].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](50) NOT NULL,
	[Platform] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


