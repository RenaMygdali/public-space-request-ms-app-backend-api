USE [master]
GO
/****** Object:  Database [PublicSpaceMaintenanceRequestMSDb]    Script Date: 27/8/2024 10:06:24 μμ ******/
CREATE DATABASE [PublicSpaceMaintenanceRequestMSDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PublicSpaceMaintenanceRequestMSDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PublicSpaceMaintenanceRequestMSDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PublicSpaceMaintenanceRequestMSDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PublicSpaceMaintenanceRequestMSDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PublicSpaceMaintenanceRequestMSDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET  MULTI_USER 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PublicSpaceMaintenanceRequestMSDb]
GO
/****** Object:  User [requestMSuser]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE USER [requestMSuser] FOR LOGIN [requestMSuser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [requestMSuser]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Admins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citizens]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citizens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Citizens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Completes]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Completes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
 CONSTRAINT [PK_Completes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InProgresses]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InProgresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
 CONSTRAINT [PK_InProgresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Officers]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Officers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Officers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pendings]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pendings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
 CONSTRAINT [PK_Pendings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requests]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](1000) NULL,
	[RequestStatus] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[CitizenId] [int] NOT NULL,
	[AssignedDepartmentId] [int] NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/8/2024 10:06:25 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](60) NULL,
	[Firstname] [nvarchar](50) NULL,
	[Lastname] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Phonenumber] [nvarchar](50) NULL,
	[Role] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_Admins_UserId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Admins_UserId] ON [dbo].[Admins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Citizens_UserId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Citizens_UserId] ON [dbo].[Citizens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Completes_RequestId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Completes_RequestId] ON [dbo].[Completes]
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Name]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Name] ON [dbo].[Departments]
(
	[Title] ASC
)
WHERE ([Title] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InProgresses_RequestId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_InProgresses_RequestId] ON [dbo].[InProgresses]
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Officers_DepartmentId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_Officers_DepartmentId] ON [dbo].[Officers]
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Officers_UserId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Officers_UserId] ON [dbo].[Officers]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pendings_RequestId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Pendings_RequestId] ON [dbo].[Pendings]
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CreateDate]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_CreateDate] ON [dbo].[Requests]
(
	[CreateDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Requests_AssignedDepartmentId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_Requests_AssignedDepartmentId] ON [dbo].[Requests]
(
	[AssignedDepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Requests_CitizenId]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_Requests_CitizenId] ON [dbo].[Requests]
(
	[CitizenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Status]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_Status] ON [dbo].[Requests]
(
	[RequestStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_LASTNAME]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE NONCLUSTERED INDEX [IX_LASTNAME] ON [dbo].[Users]
(
	[Lastname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_EMAIL]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_EMAIL] ON [dbo].[Users]
(
	[Email] ASC
)
WHERE ([Email] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_USERNAME]    Script Date: 27/8/2024 10:06:25 μμ ******/
CREATE UNIQUE NONCLUSTERED INDEX [UQ_USERNAME] ON [dbo].[Users]
(
	[Username] ASC
)
WHERE ([Username] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Requests] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Requests] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[Admins]  WITH CHECK ADD  CONSTRAINT [FK_Admins_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Admins] CHECK CONSTRAINT [FK_Admins_Users]
GO
ALTER TABLE [dbo].[Citizens]  WITH CHECK ADD  CONSTRAINT [FK_Citizens_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Citizens] CHECK CONSTRAINT [FK_Citizens_Users]
GO
ALTER TABLE [dbo].[Completes]  WITH CHECK ADD  CONSTRAINT [FK_Completes_Requests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Requests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Completes] CHECK CONSTRAINT [FK_Completes_Requests_RequestId]
GO
ALTER TABLE [dbo].[InProgresses]  WITH CHECK ADD  CONSTRAINT [FK_InProgresses_Requests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Requests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InProgresses] CHECK CONSTRAINT [FK_InProgresses_Requests_RequestId]
GO
ALTER TABLE [dbo].[Officers]  WITH CHECK ADD  CONSTRAINT [FK_Officers_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Officers] CHECK CONSTRAINT [FK_Officers_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Officers]  WITH CHECK ADD  CONSTRAINT [FK_Officers_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Officers] CHECK CONSTRAINT [FK_Officers_Users]
GO
ALTER TABLE [dbo].[Pendings]  WITH CHECK ADD  CONSTRAINT [FK_Pendings_Requests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Requests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pendings] CHECK CONSTRAINT [FK_Pendings_Requests_RequestId]
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_Citizens] FOREIGN KEY([CitizenId])
REFERENCES [dbo].[Citizens] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_Citizens]
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_Departments] FOREIGN KEY([AssignedDepartmentId])
REFERENCES [dbo].[Departments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_Departments]
GO
USE [master]
GO
ALTER DATABASE [PublicSpaceMaintenanceRequestMSDb] SET  READ_WRITE 
GO
