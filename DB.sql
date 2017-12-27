CREATE DATABASE Licenta
GO

USE [Licenta]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 27.12.2017 18:42:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[CardNo] [nvarchar](50) NULL,
	[DateOfBirth] [datetime] NULL,
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Person] ([FirstName], [LastName], [CardNo], [DateOfBirth], [Id]) VALUES (N'Jon', N'Snow', N'1234', '1999-01-01', 1)
GO
INSERT [dbo].[Person] ([FirstName], [LastName], [CardNo], [DateOfBirth], [Id]) VALUES (N'Mary', N'Jay', N'1234', '1996-11-04', 2)
GO
INSERT [dbo].[Person] ([FirstName], [LastName], [CardNo], [DateOfBirth], [Id]) VALUES (N'Mark', N'Twain', N'5432', '2000-02-03', 3)
GO
