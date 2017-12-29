USE [master]
GO
/****** Object:  Database [Licenta]    Script Date: 29.12.2017 22:01:34 ******/
DROP DATABASE IF EXISTS [Licenta]
GO
/****** Object:  Database [Licenta]    Script Date: 29.12.2017 22:01:34 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Licenta')
BEGIN
CREATE DATABASE [Licenta]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Licenta', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\Licenta.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Licenta_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQLEXPRESS\MSSQL\DATA\Licenta_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
END
GO
ALTER DATABASE [Licenta] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Licenta].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Licenta] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Licenta] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Licenta] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Licenta] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Licenta] SET ARITHABORT OFF 
GO
ALTER DATABASE [Licenta] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Licenta] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Licenta] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Licenta] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Licenta] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Licenta] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Licenta] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Licenta] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Licenta] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Licenta] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Licenta] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Licenta] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Licenta] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Licenta] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Licenta] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Licenta] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Licenta] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Licenta] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Licenta] SET  MULTI_USER 
GO
ALTER DATABASE [Licenta] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Licenta] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Licenta] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Licenta] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Licenta] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Licenta] SET QUERY_STORE = OFF
GO
USE [Licenta]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Licenta]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 29.12.2017 22:01:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Person]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Person](
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[CardNo] [varbinary](max) NULL,
	[DateOfBirth] [datetime] NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
USE [master]
GO
ALTER DATABASE [Licenta] SET  READ_WRITE 
GO
