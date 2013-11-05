USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[ControlProperty]    Script Date: 03/01/2013 22:44:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[QA_Automation_Test].[ControlProperty]') AND type in (N'U'))
DROP TABLE [QA_Automation_Test].[ControlProperty]
GO

USE [QA_Automation_Test_POC]
GO

/****** Object:  Table [QA_Automation_Test].[ControlProperty]    Script Date: 03/01/2013 22:44:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [QA_Automation_Test].[ControlProperty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](20) NOT NULL,
	[Property] [varchar](20) NOT NULL,
 CONSTRAINT [PK_ControlProperty] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


