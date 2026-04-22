USE [master]
GO
/****** Object:  Database [RuleEngine-Dev]    Script Date: 4/22/2026 11:14:02 PM ******/
CREATE DATABASE [RuleEngine-Dev]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RuleEngine-Dev', FILENAME = N'E:\MyProjects\RuleEngine\Dev\Data\RuleEngine-Dev.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RuleEngine-Dev_log', FILENAME = N'E:\MyProjects\RuleEngine\Dev\Data\RuleEngine-Dev_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RuleEngine-Dev] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RuleEngine-Dev].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RuleEngine-Dev] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ARITHABORT OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RuleEngine-Dev] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RuleEngine-Dev] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RuleEngine-Dev] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RuleEngine-Dev] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RuleEngine-Dev] SET  MULTI_USER 
GO
ALTER DATABASE [RuleEngine-Dev] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RuleEngine-Dev] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RuleEngine-Dev] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RuleEngine-Dev] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RuleEngine-Dev] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [RuleEngine-Dev] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RuleEngine-Dev] SET QUERY_STORE = ON
GO
ALTER DATABASE [RuleEngine-Dev] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RuleEngine-Dev]
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rule](
	[RuleId] [varchar](64) NOT NULL,
	[RuleName] [nvarchar](128) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[RuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleTarget]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleTarget](
	[RuleTargetId] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[TargetType] [varchar](64) NULL,
	[TargetCode] [varchar](64) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RuleTarget] PRIMARY KEY CLUSTERED 
(
	[RuleTargetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[ApplicationKey] [nvarchar](128) NOT NULL,
	[ApplicationName] [nvarchar](128) NOT NULL,
	[Version] [varchar](16) NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleAssignment]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleAssignment](
	[RuleAssignmentId] [uniqueidentifier] NOT NULL,
	[RuleId] [varchar](64) NOT NULL,
	[RuleTargetId] [uniqueidentifier] NOT NULL,
	[Priority] [int] NOT NULL,
	[StopOnFirstFail] [bit] NOT NULL,
	[EnableLog] [bit] NOT NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RuleAssignment] PRIMARY KEY CLUSTERED 
(
	[RuleAssignmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[RuleAssignmentView]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RuleAssignmentView]
AS
	SELECT RA.[RuleAssignmentId]
			,AP.[ApplicationId]
			,AP.[ApplicationName]
			,AP.[Version]
			,RT.[RuleTargetId]
			,RT.[TargetType]
			,RT.[TargetCode]
			,RL.[RuleId]
			,RL.[RuleName]
			,RL.[Description]		
			,RA.[Priority]
			,RA.[StopOnFirstFail]
			,RA.[EnableLog]
			,RA.[IsActived]
			,RA.[IsDeleted]
			,RA.[CreatedDate]
			,RA.[CreatedUserId]
			,RA.[LastModifiedDate]
			,RA.[LastModifiedUserId]
		FROM [Rule] RL 
			INNER JOIN [RuleAssignment] RA ON RL.RuleId = RA.RuleId
			INNER JOIN [RuleTarget] RT ON RT.[RuleTargetId] = RA.[RuleTargetId]
			INNER JOIN [Application] AP ON AP.[ApplicationId] = RT.[ApplicationId]
GO
/****** Object:  Table [dbo].[RuleAction]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleAction](
	[RuleActionId] [varchar](64) NOT NULL,
	[RuleId] [varchar](64) NOT NULL,
	[ActionType] [varchar](64) NOT NULL,
	[ActionOrder] [int] NOT NULL,
	[ActionJson] [nvarchar](max) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RuleAction] PRIMARY KEY CLUSTERED 
(
	[RuleActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleCondition]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleCondition](
	[RuleConditionId] [varchar](64) NOT NULL,
	[RuleId] [varchar](64) NOT NULL,
	[ConditionJson] [nvarchar](max) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RuleCondition] PRIMARY KEY CLUSTERED 
(
	[RuleConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleExecutionLog]    Script Date: 4/22/2026 11:14:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleExecutionLog](
	[RuleExecutionLog] [uniqueidentifier] NOT NULL,
	[RuleAssignmentId] [uniqueidentifier] NOT NULL,
	[RequestId] [varchar](64) NULL,
	[Input] [nvarchar](max) NULL,
	[ConditionResult] [bit] NULL,
	[ActionsExecuted] [nvarchar](max) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
	[ExecutionTimeMs] [int] NULL,
	[IsActived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedUserId] [uniqueidentifier] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedUserId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_RuleExecutionLog] PRIMARY KEY CLUSTERED 
(
	[RuleExecutionLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Application] ([ApplicationId], [ApplicationKey], [ApplicationName], [Version], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'a1a1a1a1-b2b2-4c4c-b2b2-a1a1a1a1b2b2', N'3b8f1a6c4e9d2a7c5f0b3e1d8a6c9f2e4b7a1c5d8e3f0a2c6b9d4e7f1a3c8b5', N'Family Tree', N'0.0.1', 1, 0, CAST(N'2026-04-22T15:11:18.330' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Application] ([ApplicationId], [ApplicationKey], [ApplicationName], [Version], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'abcddcba-1221-4aa4-1221-abcddcba1221', N'e7c3a9f1d5b2c8a4f0e6d9b7a1c3e5f8d2b4a7c9e1f6d3b8a0c5e7f2d9a4b6c1', N'New App', N'0.0.1', 1, 0, CAST(N'2026-04-22T21:36:41.430' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Rule] ([RuleId], [RuleName], [Description], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'CHECK_SUBPRODUCTDETAILS_APPROVEDDATEOFSECLOAN_FORMAT', N'Validate approvedDateOfSecLoan date format', N'Reject if subProductDetails.approvedDateOfSecLoan does not match YYYY-MM-DD format', 1, 0, CAST(N'2026-04-22T15:11:18.330' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Rule] ([RuleId], [RuleName], [Description], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'RULE_CHECK_FRONTEND_APP_EXISTS', N'Reject duplicate FrontEnd application', N'Reject when new application has existing frontEndApplicationId', 1, 0, CAST(N'2026-04-22T15:11:18.313' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Rule] ([RuleId], [RuleName], [Description], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'RULE_UNSECOD_001', N'Validate Product Type (OD)', N'Reject if productDetails.product is not in the allowed OD product list', 1, 0, CAST(N'2026-04-22T15:11:18.317' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Rule] ([RuleId], [RuleName], [Description], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'RULE_UNSECOD_002', N'Required fields for ODBUNDLESECURED', N'When product is ODBUNDLESECURED, subProductDetails fields are required', 1, 0, CAST(N'2026-04-22T15:11:18.320' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[Rule] ([RuleId], [RuleName], [Description], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'RULE_UNSECOD_003', N'Calculate Applicant Age', N'Set age based on current date minus birthDate using DB function', 1, 0, CAST(N'2026-04-22T15:11:18.333' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_CHECK_APPROVED_DATE_FMT', N'CHECK_SUBPRODUCTDETAILS_APPROVEDDATEOFSECLOAN_FORMAT', N'REJECT', 1, N'{"errorCode":"02","message":"subProductDetails.approvedDateOfSecLoan"}', 1, 0, CAST(N'2026-04-22T15:11:18.333' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_CHECK_FRONTEND_APP_EXISTS', N'RULE_CHECK_FRONTEND_APP_EXISTS', N'REJECT', 1, N'{"errorCode":"11","message":"Cannot create application: duplicated FrontEnd application ID ${sourcingDetails.frontEndApplicationId}","metadataQuery":"dbo.FN_QUERY_JSON(''APPLICATION'',''FRONT_END_APP_ID:frontEndAppId,LOS_APP_ID:applicationId,APPLICATION_CREATOR:applicationCreator,APPLICATION_STATUS:appStatus,APPLICATION_SUB_STATUS:appSubStatus'',''FRONT_END_APP_ID = ''''${sourcingDetails.frontEndApplicationId}'''''',NULL, 1)"}', 1, 0, CAST(N'2026-04-22T15:11:18.317' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_001', N'RULE_UNSECOD_001', N'REJECT', 1, N'{"errorCode":"ERR_INVALID_PRODUCT","message":"productDetails.product is invalid"}', 1, 0, CAST(N'2026-04-22T15:11:18.320' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_01', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 1, N'{"errorCode":"01","field":"subProductDetails.approvedDateOfSecLoan","message":"subProductDetails.approvedDateOfSecLoan"}', 1, 0, CAST(N'2026-04-22T15:11:18.323' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_02', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 2, N'{"errorCode":"01","field":"subProductDetails.ltvOfSecLoan","message":"subProductDetails.ltvOfSecLoan"}', 1, 0, CAST(N'2026-04-22T15:11:18.323' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_03', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 3, N'{"errorCode":"01","field":"subProductDetails.secLoanLd","message":"subProductDetails.secLoanLd"}', 1, 0, CAST(N'2026-04-22T15:11:18.327' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_04', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 4, N'{"errorCode":"01","field":"subProductDetails.collateralValue","message":"subProductDetails.collateralValue"}', 1, 0, CAST(N'2026-04-22T15:11:18.327' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_05', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 5, N'{"errorCode":"01","field":"subProductDetails.collateralId","message":"subProductDetails.collateralID"}', 1, 0, CAST(N'2026-04-22T15:11:18.327' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_06', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 6, N'{"errorCode":"01","field":"subProductDetails.collateralType","message":"subProductDetails.collateralType"}', 1, 0, CAST(N'2026-04-22T15:11:18.330' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_002_07', N'RULE_UNSECOD_002', N'REQUIRE_FIELD', 7, N'{"errorCode":"01","field":"subProductDetails.secCustGroup","message":"subProductDetails.secCustGroup"}', 1, 0, CAST(N'2026-04-22T15:11:18.330' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAction] ([RuleActionId], [RuleId], [ActionType], [ActionOrder], [ActionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'ACT_UNSECOD_003', N'RULE_UNSECOD_003', N'SET_VALUE', 1, N'{"field":"primaryApplicant.personalDetails.age","expression":"(int)((DateTime.Now - DateTime.Parse(${primaryApplicant.personalDetails.birthDate})).TotalDays / 365.25)"}', 1, 0, CAST(N'2026-04-22T15:11:18.337' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAssignment] ([RuleAssignmentId], [RuleId], [RuleTargetId], [Priority], [StopOnFirstFail], [EnableLog], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'f488356c-49eb-48a1-84ed-627e612ecf99', N'RULE_UNSECOD_003', N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', 5, 0, 1, 1, 0, CAST(N'2026-04-22T15:11:18.337' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAssignment] ([RuleAssignmentId], [RuleId], [RuleTargetId], [Priority], [StopOnFirstFail], [EnableLog], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'a1f7d00f-a587-408d-b98e-86141f8546af', N'RULE_UNSECOD_001', N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', 2, 0, 1, 1, 0, CAST(N'2026-04-22T15:11:18.317' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAssignment] ([RuleAssignmentId], [RuleId], [RuleTargetId], [Priority], [StopOnFirstFail], [EnableLog], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'3d3737bb-6bb3-420f-ab49-8e4a05b79636', N'RULE_UNSECOD_002', N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', 3, 0, 1, 1, 0, CAST(N'2026-04-22T15:11:18.320' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAssignment] ([RuleAssignmentId], [RuleId], [RuleTargetId], [Priority], [StopOnFirstFail], [EnableLog], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'3ec13eb3-314c-442c-b7f1-daaa5b78df12', N'RULE_CHECK_FRONTEND_APP_EXISTS', N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', 1, 0, 1, 1, 0, CAST(N'2026-04-22T15:11:18.313' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleAssignment] ([RuleAssignmentId], [RuleId], [RuleTargetId], [Priority], [StopOnFirstFail], [EnableLog], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'28bf39d9-e12b-4f51-8e2c-ed2183e71b40', N'CHECK_SUBPRODUCTDETAILS_APPROVEDDATEOFSECLOAN_FORMAT', N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', 4, 0, 1, 1, 0, CAST(N'2026-04-22T15:11:18.330' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleCondition] ([RuleConditionId], [RuleId], [ConditionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'COND_CHECK_APPROVED_DATE_FMT', N'CHECK_SUBPRODUCTDETAILS_APPROVEDDATEOFSECLOAN_FORMAT', N'{"type":"AND","conditions":[{"field":"subProductDetails.approvedDateOfSecLoan","operator":"IS_NOT_NULL"},{"field":"subProductDetails.approvedDateOfSecLoan","operator":"IS_NOT_VALID_DATE","value":"yyyy-MM-dd"}]}', 1, 0, CAST(N'2026-04-22T15:11:18.333' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleCondition] ([RuleConditionId], [RuleId], [ConditionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'COND_CHECK_FRONTEND_APP_EXISTS', N'RULE_CHECK_FRONTEND_APP_EXISTS', N'{"type":"AND","conditions":[{"field":"isNewApplication","operator":"EQ","value":true},{"field":"sourcingDetails.frontEndApplicationId","operator":"IS_NOT_NULL"},{"field":"dbo.FN_FRONTEND_APP_EXISTS(${sourcingDetails.frontEndApplicationId})","operator":"DB_FUNCTION","compareOperator":"GT","value":0}]}', 1, 0, CAST(N'2026-04-22T15:11:18.313' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleCondition] ([RuleConditionId], [RuleId], [ConditionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'COND_UNSECOD_001', N'RULE_UNSECOD_001', N'{"type":"SIMPLE","field":"productDetails.product","operator":"NOT_IN","values":["ODXSELL","ODSTAFF","ODBUNDLESECURED"]}', 1, 0, CAST(N'2026-04-22T15:11:18.320' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleCondition] ([RuleConditionId], [RuleId], [ConditionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'COND_UNSECOD_002', N'RULE_UNSECOD_002', N'{"type":"AND","conditions":[{"field":"isNewApplication","operator":"EQ","value":true},{"field":"productDetails.product","operator":"EQ","value":"ODBUNDLESECURED"}]}', 1, 0, CAST(N'2026-04-22T15:11:18.323' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleCondition] ([RuleConditionId], [RuleId], [ConditionJson], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'COND_UNSECOD_003', N'RULE_UNSECOD_003', N'{"type":"SIMPLE","field":"primaryApplicant.personalDetails.birthDate","operator":"IS_NOT_NULL"}', 1, 0, CAST(N'2026-04-22T15:11:18.337' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
INSERT [dbo].[RuleTarget] ([RuleTargetId], [ApplicationId], [TargetType], [TargetCode], [IsActived], [IsDeleted], [CreatedDate], [CreatedUserId], [LastModifiedDate], [LastModifiedUserId]) VALUES (N'71af5cfa-92b7-40ea-8e2f-500e8ecfd6cd', N'abcddcba-1221-4aa4-1221-abcddcba1221', N'PRODUCT', N'UNSECODAUTO', 1, 0, CAST(N'2026-04-22T21:44:14.860' AS DateTime), N'00000000-0000-0000-0000-000000000000', NULL, NULL)
GO
/****** Object:  Index [IX_Application]    Script Date: 4/22/2026 11:14:02 PM ******/
CREATE NONCLUSTERED INDEX [IX_Application] ON [dbo].[Application]
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RuleTargetIdx01]    Script Date: 4/22/2026 11:14:02 PM ******/
CREATE NONCLUSTERED INDEX [RuleTargetIdx01] ON [dbo].[RuleTarget]
(
	[TargetType] ASC,
	[TargetCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Application] ADD  CONSTRAINT [DF_Application_IsActived]  DEFAULT ((1)) FOR [IsActived]
GO
ALTER TABLE [dbo].[Application] ADD  CONSTRAINT [DF_Application_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Application] ADD  CONSTRAINT [DF_Application_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Rule] ADD  CONSTRAINT [DF_Rule_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Rule] ADD  CONSTRAINT [DF_Rule_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RuleAction] ADD  CONSTRAINT [DF_RuleAction_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RuleAction] ADD  CONSTRAINT [DF_RuleAction_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RuleAssignment] ADD  CONSTRAINT [DF_RuleAssignment_RuleAssignmentId]  DEFAULT (newid()) FOR [RuleAssignmentId]
GO
ALTER TABLE [dbo].[RuleAssignment] ADD  CONSTRAINT [DF_RuleAssignment_StopOnFirstFail]  DEFAULT ((0)) FOR [StopOnFirstFail]
GO
ALTER TABLE [dbo].[RuleAssignment] ADD  CONSTRAINT [DF_RuleAssignment_EnableLog]  DEFAULT ((0)) FOR [EnableLog]
GO
ALTER TABLE [dbo].[RuleAssignment] ADD  CONSTRAINT [DF_RuleAssignment_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RuleAssignment] ADD  CONSTRAINT [DF_RuleAssignment_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RuleCondition] ADD  CONSTRAINT [DF_RuleCondition_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RuleCondition] ADD  CONSTRAINT [DF_RuleCondition_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RuleExecutionLog] ADD  CONSTRAINT [DF_RuleExecutionLog_RuleAssignmentId]  DEFAULT (newid()) FOR [RuleAssignmentId]
GO
ALTER TABLE [dbo].[RuleExecutionLog] ADD  CONSTRAINT [DF_RuleExecutionLog_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RuleExecutionLog] ADD  CONSTRAINT [DF_RuleExecutionLog_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RuleTarget] ADD  CONSTRAINT [DF_RuleTarget_RuleTargetId]  DEFAULT (newid()) FOR [RuleTargetId]
GO
ALTER TABLE [dbo].[RuleTarget] ADD  CONSTRAINT [DF_RuleTarget_IsActived]  DEFAULT ((1)) FOR [IsActived]
GO
ALTER TABLE [dbo].[RuleTarget] ADD  CONSTRAINT [DF_RuleTarget_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RuleTarget] ADD  CONSTRAINT [DF_RuleTarget_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PRODUCT, CHANNEL, REGION, CUSTOMER_SEGMENT, PERSON' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RuleTarget', @level2type=N'COLUMN',@level2name=N'TargetType'
GO
USE [master]
GO
ALTER DATABASE [RuleEngine-Dev] SET  READ_WRITE 
GO
