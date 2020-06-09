USE [master]
GO
/****** Object:  Database [SalaryManagement]    Script Date: 29/5/2020 1:22:48 AM ******/
CREATE DATABASE [SalaryManagement] ON  PRIMARY 
( NAME = N'SalaryManagement', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\SalaryManagement.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SalaryManagement_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\SalaryManagement_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SalaryManagement] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SalaryManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SalaryManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SalaryManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SalaryManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SalaryManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SalaryManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [SalaryManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SalaryManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SalaryManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SalaryManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SalaryManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SalaryManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SalaryManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SalaryManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SalaryManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SalaryManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SalaryManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SalaryManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SalaryManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SalaryManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SalaryManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SalaryManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SalaryManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SalaryManagement] SET  MULTI_USER 
GO
ALTER DATABASE [SalaryManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SalaryManagement] SET DB_CHAINING OFF 
GO
USE [SalaryManagement]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 29/5/2020 1:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [nvarchar](50) NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Salary] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0001', N'hpotter', N'Harry Potter', CAST(1235.01 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0002', N'rwesley', N'Ron Weasley', CAST(19234.50 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0003', N'ssnape', N'Severus Snape', CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0004', N'rhagrid', N'Rubeus Hagrid', CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0005', N'voldemort', N'Lord Voldemort', CAST(523.40 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0006', N'gwesley', N'Ginny Weasley', CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[Employees] ([EmployeeID], [LoginName], [Name], [Salary]) VALUES (N'e0007', N'hgranger', N'Hermione Granger', CAST(0.01 AS Decimal(18, 2)))
USE [master]
GO
ALTER DATABASE [SalaryManagement] SET  READ_WRITE 
GO
