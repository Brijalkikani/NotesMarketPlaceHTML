USE [master]
GO
/****** Object:  Database [notemarketplace]    Script Date: 10-04-2021 13:38:01 ******/
CREATE DATABASE [notemarketplace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'notemarketplace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\notemarketplace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'notemarketplace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\notemarketplace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [notemarketplace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [notemarketplace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [notemarketplace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [notemarketplace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [notemarketplace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [notemarketplace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [notemarketplace] SET ARITHABORT OFF 
GO
ALTER DATABASE [notemarketplace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [notemarketplace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [notemarketplace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [notemarketplace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [notemarketplace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [notemarketplace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [notemarketplace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [notemarketplace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [notemarketplace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [notemarketplace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [notemarketplace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [notemarketplace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [notemarketplace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [notemarketplace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [notemarketplace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [notemarketplace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [notemarketplace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [notemarketplace] SET RECOVERY FULL 
GO
ALTER DATABASE [notemarketplace] SET  MULTI_USER 
GO
ALTER DATABASE [notemarketplace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [notemarketplace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [notemarketplace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [notemarketplace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [notemarketplace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [notemarketplace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'notemarketplace', N'ON'
GO
ALTER DATABASE [notemarketplace] SET QUERY_STORE = OFF
GO
USE [notemarketplace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[CountryCode] [varchar](100) NULL,
	[Createddate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoteId] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[downloader] [int] NOT NULL,
	[isSellerhasAllowedDownloaded] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[Ispaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[Createddate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Createddate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[Modifiedby] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Referencedata]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Referencedata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[value] [varchar](100) NOT NULL,
	[DataValue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[Createddate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Referencedata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[Publisheddate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachements]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[Createddate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesAttachements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoteId] [int] NOT NULL,
	[ReportedByid] [int] NOT NULL,
	[againstDownloadId] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoteId] [int] NOT NULL,
	[ReviewedByid] [int] NOT NULL,
	[AgainstDownloadsId] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[Phonenumbercountrycode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Modifieddate] [datetime] NULL,
	[modifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10-04-2021 13:38:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailId] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (4, N'INDIA', N'91', CAST(N'2021-03-07T11:58:00.000' AS DateTime), 89, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (5, N'CANADA', N'1', CAST(N'2021-03-08T11:58:00.000' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (6, N'JAPAN', N'81', CAST(N'2021-03-09T11:58:00.000' AS DateTime), 89, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (7, N'NEPAL', N'977', CAST(N'2021-04-02T11:58:00.000' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (8, N'Rasia', N'48', CAST(N'2021-04-07T14:39:08.950' AS DateTime), 90, CAST(N'2021-04-07T14:46:28.030' AS DateTime), 90, 0)
INSERT [dbo].[Countries] ([ID], [name], [CountryCode], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (9, N'iran', N'76', CAST(N'2021-04-07T14:41:27.640' AS DateTime), 90, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (1, 44, 85, 85, 1, N'/UploadFiles/85/44/Attachment/Attachment1_04132021.pdf', 1, CAST(N'2021-03-22T15:29:28.273' AS DateTime), 0, NULL, N'data science', N'IT', CAST(N'2021-03-22T15:29:28.340' AS DateTime), 85, NULL, 85, 0)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (2, 47, 85, 85, 1, N'/UploadFiles/85/47/Attachment/Attachment1_04132021.pdf', 0, CAST(N'2021-03-22T15:50:26.350' AS DateTime), 1, CAST(23 AS Decimal(18, 0)), N'book of maths', N'IT', CAST(N'2021-03-22T15:50:26.350' AS DateTime), 85, CAST(N'2021-04-09T11:09:38.170' AS DateTime), 85, 0)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (4, 58, 85, 85, 1, N'/UploadFiles/85/58/Attachment/Attachment1_04132021.pdf', 1, CAST(N'2021-03-22T16:00:00.753' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'viiirta', N'IT', CAST(N'2021-03-22T16:00:00.937' AS DateTime), 85, CAST(N'2021-03-29T17:39:43.780' AS DateTime), 85, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (6, 56, 88, 85, 1, N'/UploadFiles/88/56/Attachment/Attachment1_04132021.pdf', 1, CAST(N'2021-03-22T17:46:30.930' AS DateTime), 0, NULL, N'Science', N'CA', CAST(N'2021-03-22T17:46:30.930' AS DateTime), 85, NULL, 85, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (8, 46, 85, 85, 1, N'/UploadFiles/85/46/Attachment/Attachment1_04132021.pdf', 0, CAST(N'2021-03-22T18:15:55.673' AS DateTime), 1, CAST(2377 AS Decimal(18, 0)), N'm,m', N'IT', CAST(N'2021-03-22T18:15:55.673' AS DateTime), 85, NULL, 85, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (18, 54, 88, 88, 1, N'/UploadFiles/88/54/Attachment/Attachment1_04132021.pdf', 0, CAST(N'2021-03-23T14:57:10.043' AS DateTime), 1, CAST(266 AS Decimal(18, 0)), N'Vijeta', N'CA', CAST(N'2021-03-23T14:57:10.047' AS DateTime), 88, NULL, 88, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (27, 54, 88, 85, 1, N'/UploadFiles/88/54/Attachment/Attachment1_04132021.pdf', 0, CAST(N'2021-03-26T10:27:01.450' AS DateTime), 1, CAST(266 AS Decimal(18, 0)), N'Vijeta', N'CA', CAST(N'2021-03-26T10:27:01.453' AS DateTime), 85, CAST(N'2021-04-09T11:10:41.767' AS DateTime), 85, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (29, 47, 85, 88, 1, N'/UploadFiles/85/47/Attachment/Attachment1_04132021.pdf', 0, CAST(N'2021-03-26T10:44:29.060' AS DateTime), 1, CAST(23 AS Decimal(18, 0)), N'book of maths', N'IT', CAST(N'2021-03-26T10:44:29.060' AS DateTime), 88, CAST(N'2021-03-29T19:00:25.580' AS DateTime), 88, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (32, 46, 85, 88, 0, NULL, 0, CAST(N'2021-03-26T11:27:56.470' AS DateTime), 1, CAST(2377 AS Decimal(18, 0)), N'm,m', N'IT', CAST(N'2021-03-26T11:27:56.470' AS DateTime), 88, NULL, 88, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (34, 69, 85, 85, 1, N'/UploadFiles/85/69/Attachment/Attachment1_30222021.pdf', 0, CAST(N'2021-03-30T08:26:47.797' AS DateTime), 1, CAST(234 AS Decimal(18, 0)), N'Data science', N'MCA', CAST(N'2021-03-30T08:26:47.797' AS DateTime), 85, NULL, 85, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (35, 71, 85, 95, 1, N'/UploadFiles/85/71/Attachment/Attachment1_31112021.pdf', 1, CAST(N'2021-04-09T11:21:45.890' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'account', N'MBA', CAST(N'2021-04-09T11:21:45.893' AS DateTime), 95, NULL, NULL, 1)
INSERT [dbo].[Downloads] ([Id], [NoteId], [Seller], [downloader], [isSellerhasAllowedDownloaded], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [Ispaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (36, 57, 85, 95, 1, N'/UploadFiles/85/57/Attachment/Attachment1_04132021.pdf', 1, CAST(N'2021-04-09T11:23:27.177' AS DateTime), 0, NULL, N'bbn', N'IT', CAST(N'2021-04-09T11:23:27.177' AS DateTime), 95, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([Id], [name], [Description], [Createddate], [CreatedBy], [ModifiedDate], [Modifiedby], [isActive]) VALUES (1, N'IT', N'information technology', CAST(N'2021-03-04T14:14:59.000' AS DateTime), 90, NULL, 90, 1)
INSERT [dbo].[NoteCategories] ([Id], [name], [Description], [Createddate], [CreatedBy], [ModifiedDate], [Modifiedby], [isActive]) VALUES (2, N'MCA', N'master of computer app', CAST(N'2021-03-05T14:14:59.000' AS DateTime), 90, NULL, 90, 1)
INSERT [dbo].[NoteCategories] ([Id], [name], [Description], [Createddate], [CreatedBy], [ModifiedDate], [Modifiedby], [isActive]) VALUES (3, N'MBA', N'this is mba', CAST(N'2021-03-08T14:14:59.000' AS DateTime), 90, NULL, 90, 1)
INSERT [dbo].[NoteCategories] ([Id], [name], [Description], [Createddate], [CreatedBy], [ModifiedDate], [Modifiedby], [isActive]) VALUES (4, N'CA', N'this is CA', CAST(N'2021-03-08T14:12:59.000' AS DateTime), 90, NULL, 90, 1)
INSERT [dbo].[NoteCategories] ([Id], [name], [Description], [Createddate], [CreatedBy], [ModifiedDate], [Modifiedby], [isActive]) VALUES (5, N'History', N'this is history', CAST(N'2021-04-07T10:36:50.040' AS DateTime), 90, CAST(N'2021-04-07T10:58:56.910' AS DateTime), 90, 0)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (1, N'Handwritten', N'this is handwriitten', CAST(N'2021-03-04T14:14:59.000' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (2, N'Story books', N'this is story books', CAST(N'2021-03-05T14:14:59.000' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (3, N'University Notes', N'this university books', CAST(N'2021-03-08T14:14:59.000' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([Id], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (4, N'Universitybook', N'important notes', CAST(N'2021-04-07T11:58:00.317' AS DateTime), 90, CAST(N'2021-04-07T11:59:31.333' AS DateTime), 90, 0)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Referencedata] ON 

INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (1, N'draft', N'draft', N'note status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (2, N'submitted for review', N'submited for review', N'note status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (3, N'in review', N'in review', N'note status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (4, N'published', N'approved', N'note status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (5, N'rejected', N'rejected', N'note status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (6, N'removed', N'removed', N'note status', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (7, N'Male', N'M', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (8, N'Female', N'F', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Referencedata] ([Id], [value], [DataValue], [RefCategory], [Createddate], [CreatedBy], [Modifieddate], [ModifiedBy], [isActive]) VALUES (9, N'Other', N'other', N'Gender', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Referencedata] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (39, 85, 4, 89, NULL, CAST(N'2021-03-15T15:34:57.000' AS DateTime), N'hbhhj', 4, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'vhhm', N'gec rajkot', 4, NULL, NULL, NULL, 0, NULL, N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T09:21:07.130' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (40, 85, 4, 89, NULL, CAST(N'2021-03-17T15:33:08.000' AS DateTime), N'hbhhj', 4, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'nnn', N'gec rajkot', 4, NULL, NULL, NULL, 0, NULL, N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T09:24:50.467' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (41, 85, 4, 89, NULL, CAST(N'2021-03-19T15:33:08.000' AS DateTime), N'hbhhj', 4, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'nnn', N'gec rajkot', 4, NULL, NULL, NULL, 0, NULL, N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T09:28:17.970' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (43, 85, 1, NULL, NULL, NULL, N'jggg', 4, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'hhhh', NULL, 4, NULL, NULL, NULL, 1, CAST(66 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T09:37:22.457' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (44, 85, 4, 89, NULL, CAST(N'2021-03-23T17:33:08.000' AS DateTime), N'data science', 1, N'/UploadFiles/85/44/DP_210239646.jpg', 1, 3445, N'this is my own book', N'gec bvn', 7, N'science', N'243', N'k p kandoriya', 0, NULL, N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T15:02:39.183' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (45, 85, 5, 89, N'not good', NULL, N'kbgl', 1, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'hh', NULL, 5, NULL, NULL, NULL, 1, CAST(7767 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T15:12:04.167' AS DateTime), NULL, CAST(N'2021-04-05T11:09:39.607' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (46, 85, 5, 89, N'not helpful', CAST(N'2021-03-16T12:03:06.000' AS DateTime), N'm,m', 1, N'/System Configuration/DefaultImage/DN.jpg', 2, 456, N'bbb', N'gec bvn', 5, N'science', N'45666', N'kpk', 1, CAST(2377 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-06T15:54:21.843' AS DateTime), NULL, CAST(N'2021-04-05T12:14:17.583' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (47, 85, 4, 89, NULL, CAST(N'2021-03-16T11:08:06.000' AS DateTime), N'book of maths', 1, N'/System Configuration/DefaultImage/DN.jpg', 1, 3445, N'vhcnhbvhcbvbjmn', N'bvbnn', 4, N'science', N'123', N'k p kandoriya', 1, CAST(23 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-07T10:41:29.000' AS DateTime), NULL, CAST(N'2021-03-14T11:18:44.440' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (48, 85, 4, 89, NULL, CAST(N'2021-03-31T19:06:10.057' AS DateTime), N'hbhhj', 4, N'/System Configuration/DefaultImage/DN.jpg', 1, 3445, N'bbb', N'bvbnn', 5, N'nmmm', N'243', N'bb b ', 1, CAST(344 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-14T10:58:08.963' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (49, 0, 2, NULL, NULL, NULL, N'brijal', 1, N'/System Configuration/DefaultImage/DN.jpg', 1, 3445, N'hhhh', N'bvbnn', 4, N'nmmm', N'243', N'bb', 1, CAST(34 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-14T10:29:08.693' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (50, 85, 6, 89, N'it need to change', CAST(N'2021-04-04T18:58:43.337' AS DateTime), N'jnmcm', 1, N'/System Configuration/DefaultImage/DN.jpg', 2, 3445, N' n n      nnnn', N'bvbnn', 5, N'vigyan', N'1234', N'bvk', 1, CAST(35 AS Decimal(18, 0)), N'/UploadFiles/88/55/Preview_213309067.pdf', CAST(N'2021-03-07T10:41:29.000' AS DateTime), NULL, CAST(N'2021-04-05T10:25:35.890' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (51, 85, 4, 89, NULL, NULL, N'ajnmcm', 1, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N' n n      nnnn', NULL, 5, NULL, NULL, NULL, 1, CAST(22 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-07T10:41:29.723' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (52, 85, 1, 89, N'this note is inappropriate', NULL, N'fjnmcm', 1, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N' n n      nnnn', NULL, 7, NULL, NULL, NULL, 1, CAST(22 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-07T10:41:29.723' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (53, 0, 2, NULL, NULL, NULL, N'vjnmcm', 1, NULL, NULL, NULL, N' n n      nnnn', NULL, 7, NULL, NULL, NULL, 1, CAST(22 AS Decimal(18, 0)), N'/UploadFiles/88/55/Preview_213309067.pdf', CAST(N'2021-03-13T10:55:43.090' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (54, 88, 4, 89, NULL, CAST(N'2021-03-28T17:05:06.000' AS DateTime), N'Vijeta', 4, N'/System Configuration/DefaultImage/DN.jpg', 1, 204, N'This is my own book', N'Gec bvn', 4, N'Science', N'256', N'ashish nimavat', 1, CAST(266 AS Decimal(18, 0)), N'/UploadFiles/88/55/Preview_213309067.pdf', CAST(N'2021-03-15T15:31:21.180' AS DateTime), NULL, CAST(N'2021-04-05T12:04:14.520' AS DateTime), 89, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (55, 88, 6, 89, N'this is not good', CAST(N'2021-04-02T09:59:11.357' AS DateTime), N'Tatvasoft', 1, N'/System Configuration/DefaultImage/DN.jpg', 1, 456, N'this is wellknown ', N'gec rajkot', 7, N'vigyan', N'1234', N'k p kandoriya', 1, CAST(344 AS Decimal(18, 0)), N'/UploadFiles/88/55/Preview_213309067.pdf', CAST(N'2021-03-15T15:33:08.930' AS DateTime), NULL, CAST(N'2021-04-05T10:19:46.467' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (56, 88, 4, 89, N'not good', CAST(N'2021-03-14T12:03:06.000' AS DateTime), N'Science', 4, N'/System Configuration/DefaultImage/DN.jpg', 2, 167, N'this is knownnnn', N'gec bvn', 7, N'science', N'243', N'bvk', 0, NULL, N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-15T15:34:57.173' AS DateTime), NULL, CAST(N'2021-04-05T12:04:17.160' AS DateTime), 89, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (57, 85, 4, 89, NULL, CAST(N'2021-04-02T09:58:36.583' AS DateTime), N'bbn', 1, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'hhh', N'gec rajkot', 7, N'Science', NULL, NULL, 0, NULL, N'/UploadFiles/88/55/Preview_213309067.pdf', CAST(N'2021-03-15T18:15:15.567' AS DateTime), NULL, CAST(N'2021-03-15T22:13:51.163' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (58, 85, 4, 89, NULL, CAST(N'2021-03-16T12:25:06.000' AS DateTime), N'viiirta', 1, N'/System Configuration/DefaultImage/DN.jpg', 1, 3445, N'hhhh', N'Gec bvn', 4, N'science', N'243', N'k p kandoriya', 0, CAST(0 AS Decimal(18, 0)), N'/UploadFiles/85/50/Preview_214129982.pdf', CAST(N'2021-03-16T12:03:06.473' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (69, 85, 4, 89, NULL, CAST(N'2021-03-16T12:35:06.000' AS DateTime), N'Data science', 2, N'/System Configuration/DefaultImage/DN.jpg', NULL, 566, N'this is note bokk', N'Rk university', 7, N'science', N'234456', N'Sanjay patel', 1, CAST(234 AS Decimal(18, 0)), N'/UploadFiles/85/69/Preview_212236043.pdf', CAST(N'2021-03-30T08:22:35.780' AS DateTime), NULL, CAST(N'2021-03-30T08:23:00.273' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (70, 85, 3, NULL, NULL, NULL, N'account', 1, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'this is note bokk', NULL, 7, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-31T14:10:51.303' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (71, 85, 4, 90, NULL, CAST(N'2021-04-09T09:42:25.160' AS DateTime), N'account', 3, N'/System Configuration/DefaultImage/DN.jpg', NULL, NULL, N'this is note bokk', NULL, 7, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), NULL, CAST(N'2021-03-31T14:11:53.443' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (72, 88, 2, NULL, NULL, NULL, N'Software Devolpment', 1, N'/UploadFiles/88/72/DP_210729121.jpg', NULL, 54, N'This is well known book', N'Rk university', 8, N'Science', N'67667', N'Sanjay patel', 1, CAST(655 AS Decimal(18, 0)), N'/UploadFiles/88/72/Preview_210729258.pdf', CAST(N'2021-04-08T18:07:28.880' AS DateTime), NULL, CAST(N'2021-04-08T18:07:56.080' AS DateTime), NULL, 1)
INSERT [dbo].[SellerNotes] ([Id], [SellerID], [Status], [ActionedBy], [AdminRemarks], [Publisheddate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (73, 95, 1, NULL, NULL, NULL, N'social science', 3, N'/UploadFiles/95/73/DP_215620740.jpg', 1, 566, N'important notes', N'Rk university', 7, N'social', N'234456', N'haresh shah', 1, CAST(78 AS Decimal(18, 0)), N'/UploadFiles/95/73/Preview_215622758.pdf', CAST(N'2021-04-08T18:56:19.797' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] ON 

INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, 39, N'Attachment1_06212021.pdf;', N'/UploadFiles/85/39/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T09:21:11.963' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, 40, N'Attachment1_06242021.pdf;', N'/UploadFiles/85/40/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T09:24:57.370' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, 41, N'Attachment1_06282021.pdf;', N'/UploadFiles/85/41/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T09:28:18.050' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, 43, N'Attachment1_06372021.pdf;', N'/UploadFiles/85/43/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T09:37:23.460' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (15, 44, N'Attachment1_0622021.pdf;', N'/UploadFiles/85/44/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T15:02:46.857' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 45, N'Attachment1_06122021.pdf;', N'/UploadFiles/85/45/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T15:12:08.517' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (17, 46, N'Attachment1_06542021.pdf;', N'/UploadFiles/85/46/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-06T15:54:24.413' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (18, 47, N'Attachment1_07182021.pdf;', N'/UploadFiles/85/47/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-07T10:18:36.563' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (19, 48, N'Attachment1_07222021.pdf;', N'/UploadFiles/85/48/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-07T10:22:18.177' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (20, 49, N'Attachment1_07272021.pdf;', N'/UploadFiles/85/49/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-07T10:27:44.107' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (21, 50, N'Attachment1_07412021.pdf;', N'/UploadFiles/85/50/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-07T10:41:29.943' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (22, 54, N'Attachment1_15312021.pdf;', N'/UploadFiles/88/54/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-15T15:31:21.893' AS DateTime), 88, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (23, 55, N'Attachment1_15332021.pdf;', N'/UploadFiles/88/55/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-15T15:33:09.017' AS DateTime), 88, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (24, 56, N'Attachment1_15342021.pdf;', N'/UploadFiles/88/56/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-15T15:34:57.257' AS DateTime), 88, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (25, 57, N'Attachment1_15162021.pdf;', N'/UploadFiles/85/57/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-15T18:16:52.437' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 58, N'Attachment1_1632021.pdf;', N'/UploadFiles/85/58/Attachment/Attachment1_04132021.pdf', CAST(N'2021-03-16T12:03:06.857' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (27, 69, N'Attachment1_30222021.pdf;', N'/UploadFiles/85/69/Attachment/Attachment1_30222021.pdf', CAST(N'2021-03-30T08:22:36.020' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (28, 71, N'Attachment1_31112021.pdf;', N'/UploadFiles/85/71/Attachment/Attachment1_31112021.pdf', CAST(N'2021-03-31T14:11:53.523' AS DateTime), 85, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (29, 72, N'Attachment1_0872021.pdf;', N'/UploadFiles/88/72/Attachment/Attachment1_0872021.pdf', CAST(N'2021-04-08T18:07:29.173' AS DateTime), 88, NULL, NULL, 1)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [Createddate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (30, 73, N'Attachment1_08562021.pdf;', N'/UploadFiles/95/73/Attachment/Attachment1_08562021.pdf', CAST(N'2021-04-08T18:56:21.807' AS DateTime), 95, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] ON 

INSERT [dbo].[SellerNotesReportedIssues] ([Id], [NoteId], [ReportedByid], [againstDownloadId], [Remarks], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (2, 54, 85, 18, N'not appropriate', CAST(N'2021-04-07T16:32:47.417' AS DateTime), 88, CAST(N'2021-04-08T17:45:12.417' AS DateTime), 85, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] ON 

INSERT [dbo].[SellerNotesReviews] ([Id], [NoteId], [ReviewedByid], [AgainstDownloadsId], [Ratings], [Comments], [CreatedDate], [CreatedBy], [Modifieddate], [ModifiedBy], [IsActive]) VALUES (2, 46, 85, 8, CAST(2 AS Decimal(18, 0)), N'goood', CAST(N'2021-03-25T21:41:01.400' AS DateTime), 85, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (64, N'SupportEmail', N'kikanibrijal23@gmail.com', CAST(N'2021-04-08T16:48:14.167' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.167' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (65, N'Password', N'mtnapkudprykaqpe', CAST(N'2021-04-08T16:48:14.313' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.313' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (66, N'SupportPhoneNumber', N'6352174896', CAST(N'2021-04-08T16:48:14.363' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.363' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (67, N'EmailAddress', N'rekhakikani79@gmail.com', CAST(N'2021-04-08T16:48:14.390' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.390' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (68, N'Facebook URL', N'www.facebook.com', CAST(N'2021-04-08T16:48:14.417' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.417' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (69, N'Twitter URL', N'www.twitter.com', CAST(N'2021-04-08T16:48:14.420' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.420' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (70, N'Linkedin URL', N'www.linkedin.com', CAST(N'2021-04-08T16:48:14.427' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.427' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (71, N'DefaultImageforNote', N'/System Configuration/DefaultImage/DN.jpg', CAST(N'2021-04-08T16:48:14.433' AS DateTime), 90, CAST(N'2021-04-08T16:48:14.433' AS DateTime), NULL, 1)
INSERT [dbo].[SystemConfigurations] ([Id], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [isActive]) VALUES (72, N'DefaultProfilePicture', N'/System Configuration/DefaultImage/DP.jpg', CAST(N'2021-04-08T16:48:16.493' AS DateTime), 90, CAST(N'2021-04-08T16:48:16.493' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ID], [UserId], [DOB], [Gender], [SecondaryEmailAddress], [Phonenumbercountrycode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, 85, CAST(N'2017-03-03T00:00:00.000' AS DateTime), 8, NULL, N'4', N'6352174896', N'/UploadFiles/85/DP_210525676.jpg', N'bagsara', N'amreli', N'sudavad', N'gujarat', N'666666', N'4', N'gtu', N'gec bvn', CAST(N'2021-03-15T10:34:43.437' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[UserProfile] ([ID], [UserId], [DOB], [Gender], [SecondaryEmailAddress], [Phonenumbercountrycode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, 88, CAST(N'2016-03-03T00:00:00.000' AS DateTime), 8, NULL, N'4', N'8128008515', N'/System Configuration/DefaultImage/DP.jpg', N'bagsara', N'amreli', N'sudavad', N'gujarat', N'666666', N'4', N'gtu', N'gec bvn', CAST(N'2021-03-15T11:06:30.677' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[UserProfile] ([ID], [UserId], [DOB], [Gender], [SecondaryEmailAddress], [Phonenumbercountrycode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, 89, NULL, NULL, N'brijkikani23@gmail.com', N'5', N'9898954612', N'/System Configuration/DefaultImage/DP.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-04T14:14:59.373' AS DateTime), 89, CAST(N'2021-04-04T14:25:48.947' AS DateTime), 89, 1)
INSERT [dbo].[UserProfile] ([ID], [UserId], [DOB], [Gender], [SecondaryEmailAddress], [Phonenumbercountrycode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, 90, NULL, NULL, N'brijkikani23@gmail.com', N'5', N'9879680483', N'/UploadFiles/90/DP_210525676.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-06T16:05:25.673' AS DateTime), 90, NULL, NULL, 1)
INSERT [dbo].[UserProfile] ([ID], [UserId], [DOB], [Gender], [SecondaryEmailAddress], [Phonenumbercountrycode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, 93, NULL, NULL, NULL, N'6', N'6565454332', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-06T17:48:01.317' AS DateTime), 90, CAST(N'2021-04-06T18:59:40.800' AS DateTime), 90, 0)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [modifiedBy], [IsActive]) VALUES (1, N'Member', N'this is simple member', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [modifiedBy], [IsActive]) VALUES (2, N'Admin', N'this is admin..', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [Modifieddate], [modifiedBy], [IsActive]) VALUES (3, N'SuperAdmin', N'this is super admin', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (85, 1, N'Brijal', N'Kikani', N'kikanibrijal23@gmail.com', N'wDedEp2VV1Yk4sgvlEHaI+aLRSS1fqJMvFjBVKjgALI=', 1, CAST(N'2021-03-05T16:14:13.857' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (88, 1, N'Rushi', N'Patel', N'brijalkikani2000@gmail.com', N'GBS48iGd8QqaFZamu0BCzkqdwFwuLtYy72jzT2SwFCU=', 1, CAST(N'2021-03-15T10:53:32.170' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (89, 2, N'Rekhaben', N'Kikani', N'rekhakikani79@gmail.com', N'nZZ5Lbv7CBdtn0KQH8WECGdcN4yR+fM69ywLVSjKcuU=', 1, CAST(N'2021-03-30T10:32:14.030' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (90, 3, N'Vipul', N'Patel', N'kikanibrij@gmail.com', N'1vTyVV0gf8x1hzPq6j/HBO+Y8xhg46kNWmpLx0e17S0=', 1, CAST(N'2021-04-05T16:54:02.233' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (93, 2, N'vijya', N'kyada', N'kikanibrijal059@gmail.com', N'JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=', 1, CAST(N'2021-04-06T17:48:01.153' AS DateTime), 90, CAST(N'2021-04-06T18:59:40.800' AS DateTime), 90, 0)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailId], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (95, 1, N'Manthan', N'Kikani', N'mankikani3@gmail.com', N'0MZBmG1hzEbm2MvteUK4/n9l4NoOQqfZFPWomLAZ7IM=', 1, CAST(N'2021-04-08T18:51:07.307' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Countries]  WITH CHECK ADD  CONSTRAINT [FK_Countries_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Countries] CHECK CONSTRAINT [FK_Countries_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_SellerNotes] FOREIGN KEY([NoteId])
REFERENCES [dbo].[SellerNotes] ([Id])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_SellerNotes]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users1] FOREIGN KEY([downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users1]
GO
ALTER TABLE [dbo].[NoteCategories]  WITH CHECK ADD  CONSTRAINT [FK_NoteCategories_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteCategories] CHECK CONSTRAINT [FK_NoteCategories_Users]
GO
ALTER TABLE [dbo].[NoteTypes]  WITH CHECK ADD  CONSTRAINT [FK_NoteTypes_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteTypes] CHECK CONSTRAINT [FK_NoteTypes_Users]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Countries]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([Id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteCategories]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([Id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteTypes]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Referencedata] FOREIGN KEY([Status])
REFERENCES [dbo].[Referencedata] ([Id])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Referencedata]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH NOCHECK ADD  CONSTRAINT [FK_SellerNotes_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users1]
GO
ALTER TABLE [dbo].[SellerNotesAttachements]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttachements_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([Id])
GO
ALTER TABLE [dbo].[SellerNotesAttachements] CHECK CONSTRAINT [FK_SellerNotesAttachements_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Downloads] FOREIGN KEY([againstDownloadId])
REFERENCES [dbo].[Downloads] ([Id])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes] FOREIGN KEY([NoteId])
REFERENCES [dbo].[SellerNotes] ([Id])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users] FOREIGN KEY([ReportedByid])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Downloads] FOREIGN KEY([NoteId])
REFERENCES [dbo].[SellerNotes] ([Id])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_SellerNotes] FOREIGN KEY([NoteId])
REFERENCES [dbo].[SellerNotes] ([Id])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Users] FOREIGN KEY([ReviewedByid])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Users]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Referencedata] FOREIGN KEY([Gender])
REFERENCES [dbo].[Referencedata] ([Id])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Referencedata]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserRoles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserRoles]
GO
USE [master]
GO
ALTER DATABASE [notemarketplace] SET  READ_WRITE 
GO
