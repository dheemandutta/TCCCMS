USE [TCCCMSDB]
GO
/****** Object:  FullTextCatalog [commonToall_catalog]    Script Date: 06-04-2021 10:22:54 PM ******/
CREATE FULLTEXT CATALOG [commonToall_catalog] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  FullTextCatalog [manual_catalog]    Script Date: 06-04-2021 10:22:55 PM ******/
CREATE FULLTEXT CATALOG [manual_catalog] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  FullTextCatalog [refMaterial_catalog]    Script Date: 06-04-2021 10:22:55 PM ******/
CREATE FULLTEXT CATALOG [refMaterial_catalog] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  FullTextCatalog [shipManual_catalog]    Script Date: 06-04-2021 10:22:55 PM ******/
CREATE FULLTEXT CATALOG [shipManual_catalog] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GenerateUserCode]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : udf_GenerateUserCode : Generate User Code
				  By Rank for ship User and normal for Company User
** Created By   : BINGSHU BAIDYA
** Created On   : 29/01/2021
*******************************************************/
CREATE FUNCTION [dbo].[udf_GenerateUserCode]
(
	@UserType INT,
	@ShipId INT,
	@RankId INT,
	@UserName VARCHAR(250)
)RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @Code VARCHAR(250),
			@Rank VARCHAR(250),
			@CodeCount INT

	IF(@UserType = 1)
		BEGIN
			SELECT @Rank = RankName FROM tblRank WHERE RankId = @RankId
			SELECT @CodeCount = COUNT(UserId) FROM tblUserMaster WHERE ShipId= @ShipId and UserType=@UserType and IsActive=1 and RankId=@RankId
			---select @CodeCount ,@Rank
			IF(@CodeCount = 0)
				BEGIN
					SET @Code = REPLACE(@Rank,' ','') + '1'
		 
				END
			ELSE
				BEGIN
					SET @CodeCount =@CodeCount +1
					SET @Code = REPLACE(@Rank,' ','') + CONVERT(VARCHAR(50), @CodeCount)
				END
		END
	ELSE IF(@UserType =2)
		BEGIN
			DECLARE @Name VARCHAR(10)
			SET @Name =LEFT(REPLACE(REPLACE(@UserName,' ',''),'.',''),4)
			SET @Code = 'tcc'+@Name
			--SELECT @CodeCount = COUNT(UserId) FROM tblUserMaster WHERE UserType=@UserType
			--IF(@CodeCount = 0)
			--	BEGIN
			--		SET @Code =  'TCC1'
		 
			--	END
			--ELSE
			--	BEGIN
			--		SET @CodeCount =@CodeCount +1
			--		SET @Code = 'TCC' + CONVERT(VARCHAR(50), @CodeCount)
			--	END
		END


	RETURN @Code

END





GO
/****** Object:  UserDefinedFunction [dbo].[ufn_CSVToTable]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[ufn_CSVToTable] ( @StringInput VARCHAR(8000), @Delimiter nvarchar(1))
RETURNS @OutputTable TABLE ( [String] VARCHAR(10) )
AS
BEGIN

    DECLARE @String    VARCHAR(10)

    WHILE LEN(@StringInput) > 0
    BEGIN
        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [String] )
        VALUES ( @String )
    END

    RETURN
END







































GO
/****** Object:  UserDefinedFunction [dbo].[ufunc_GetDateFormat]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[ufunc_GetDateFormat]()    
returns int     
Begin 
declare @x int 
select @x = convert(int,ConfigValue) from tblConfig Where KeyName = 'dateformat' 
return @x 
End

GO
/****** Object:  Table [dbo].[tblApprover]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblApprover](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Descripton] [varchar](100) NULL,
 CONSTRAINT [PK_tblApprover] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblApproverMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblApproverMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VesselIMONumber] [int] NOT NULL,
	[ShipId] [int] NOT NULL,
	[RankId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[ApproverId] [int] NULL,
	[IsActive] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_tblApproverMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCommonToAllManual]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCommonToAllManual](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varbinary](max) NOT NULL,
	[Content] [varbinary](max) NOT NULL,
	[Extension]  AS ('.html'),
	[BodyHeader] [varchar](500) NULL,
	[BodyText] [varchar](max) NULL,
	[BodyHtml] [varchar](max) NULL,
	[ControllerName] [varchar](250) NULL,
	[ActionName] [varchar](500) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [Pk_tblCommonToAllManual] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblForms]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblForms](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[FormName] [varchar](500) NOT NULL,
	[Description] [varchar](1000) NULL,
	[Path] [varchar](500) NULL,
	[IsActive] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[Version] [varchar](50) NULL,
 CONSTRAINT [PK_tblForms] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFormsArchiveLog]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFormsArchiveLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormId] [int] NOT NULL,
	[FormsVersion] [varchar](50) NULL,
	[Operation] [varchar](20) NULL,
	[ArchivedDate] [datetime] NULL,
	[ModifiedSection] [varchar](500) NULL,
 CONSTRAINT [PK_tblFormsArchiveLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFormsCategory]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFormsCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatecoryName] [varchar](250) NOT NULL,
	[Description] [varchar](500) NULL,
	[IsActive] [int] NULL,
 CONSTRAINT [PK_tblFormsCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFormsUpdateNotification]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFormsUpdateNotification](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormId] [int] NOT NULL,
	[ShipId] [int] NOT NULL,
	[Operation] [varchar](20) NULL,
	[UpdateNotifyDate] [datetime] NULL,
	[IsUpdate] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_tblFormsUpdateNotification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFormsUploaded]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFormsUploaded](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FormId] [int] NOT NULL,
	[ShipId] [int] NOT NULL,
	[FormsPath] [varchar](250) NULL,
	[FormsName] [varchar](500) NULL,
	[IsApprove] [int] NULL,
	[ApprovedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_tblFormsUploaded] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFormsUploadedApproverMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFormsUploadedApproverMapping](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UploadedFormId] [int] NOT NULL,
	[ApproverId] [int] NOT NULL,
	[IsApprove] [int] NULL,
	[ApprovedOn] [datetime] NULL,
	[CreatedOn] [datetime] NULL,
	[ApproverUserId] [int] NULL,
 CONSTRAINT [PK_tblFormsUploadedApproverMapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblGroupMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGroupMaster](
	[GroupId] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](200) NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tblGroupMaster] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblManual]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblManual](
	[ManualId] [int] IDENTITY(1,1) NOT NULL,
	[VolumeId] [int] NOT NULL,
	[ManualFileName] [varbinary](max) NOT NULL,
	[ManualHTML] [varbinary](max) NOT NULL,
	[ManualHeader] [varchar](500) NULL,
	[ManualBodyText] [varchar](max) NULL,
	[ActionName] [varchar](500) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Extension]  AS ('.html'),
	[ManualBodyHtml] [varchar](max) NULL,
	[ControllerName] [varchar](250) NULL,
 CONSTRAINT [Pk_tblManual] PRIMARY KEY CLUSTERED 
(
	[ManualId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblManualHistory]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblManualHistory](
	[ManualHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[ManualId] [int] NOT NULL,
	[ChapterName] [varchar](max) NULL,
	[Section] [varchar](max) NOT NULL,
	[ChangeComment] [varchar](500) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [Pk_tblManualHistory] PRIMARY KEY CLUSTERED 
(
	[ManualHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPermissionMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPermissionMaster](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [varchar](200) NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
 CONSTRAINT [PK_tblPermissionMaster] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPermissionRoleMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPermissionRoleMapping](
	[PermissionRoleId] [int] IDENTITY(1,1) NOT NULL,
	[PermissionId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRank]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRank](
	[RankId] [int] IDENTITY(1,1) NOT NULL,
	[RankName] [varchar](200) NOT NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [int] NULL,
	[Email] [varchar](200) NULL,
 CONSTRAINT [PK_tblRank] PRIMARY KEY CLUSTERED 
(
	[RankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReferenceMaterialManual]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReferenceMaterialManual](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varbinary](max) NOT NULL,
	[Content] [varbinary](max) NOT NULL,
	[Extension]  AS ('.html'),
	[BodyHeader] [varchar](500) NULL,
	[BodyText] [varchar](max) NULL,
	[BodyHtml] [varchar](max) NULL,
	[ControllerName] [varchar](250) NULL,
	[ActionName] [varchar](500) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [Pk_tblReferenceMaterialManual] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRevisionHeader]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRevisionHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RevisionNo] [varchar](200) NOT NULL,
	[RevisionDate] [varchar](50) NOT NULL,
	[CreatedAt] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_tblRevisionHeader] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRevisionHistory]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRevisionHistory](
	[RevisionHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[Chapter] [varchar](1000) NOT NULL,
	[Section] [varchar](2000) NOT NULL,
	[ChangeComment] [varchar](3000) NOT NULL,
	[ModificationDate] [varchar](50) NULL,
	[HeaderId] [int] NULL,
 CONSTRAINT [PK_RevisionHistory] PRIMARY KEY CLUSTERED 
(
	[RevisionHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRoleGroup]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRoleGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_tblRoleGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRoleMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRoleMaster](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](200) NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
 CONSTRAINT [PK_tblRoleMaster] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRoles]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRolGroupMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRolGroupMapping](
	[RoleGroupId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
 CONSTRAINT [PK_tblRolGroupMapping] PRIMARY KEY CLUSTERED 
(
	[RoleGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblShip]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblShip](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShipName] [nvarchar](21) NULL,
	[IMONumber] [int] NOT NULL,
	[FlagOfShip] [nvarchar](50) NULL,
	[Regime] [int] NULL,
	[TimeStamp] [timestamp] NULL,
	[LastSyncDate] [datetime] NULL,
	[CompanyID] [int] NULL,
	[ShipEmail] [varchar](200) NULL,
	[VesselTypeID] [int] NULL,
	[VesselSubTypeID] [int] NULL,
	[VesselSubSubTypeID] [int] NULL,
	[ShipEmail2] [varchar](100) NULL,
	[Voices1] [varchar](100) NULL,
	[Voices2] [varchar](100) NULL,
	[Fax1] [varchar](100) NULL,
	[Fax2] [varchar](100) NULL,
	[VOIP1] [varchar](100) NULL,
	[VOIP2] [varchar](100) NULL,
	[Mobile1] [varchar](100) NULL,
	[Mobile2] [varchar](100) NULL,
	[CommunicationsResources] [varchar](100) NULL,
	[HelicopterDeck] [int] NULL,
	[HelicopterWinchingArea] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_tblShip] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[IMONumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblShipsManual]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblShipsManual](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShipNo] [int] NULL,
	[Name] [varbinary](max) NOT NULL,
	[Content] [varbinary](max) NOT NULL,
	[Extension]  AS ('.html'),
	[BodyHeader] [varchar](500) NULL,
	[BodyText] [varchar](max) NULL,
	[BodyHtml] [varchar](max) NULL,
	[ControllerName] [varchar](250) NULL,
	[ActionName] [varchar](500) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [Pk_tblShipsManual] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblShipUpdateNotificationMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblShipUpdateNotificationMapping](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NotificationId] [int] NULL,
	[FormId] [int] NOT NULL,
	[ShipId] [int] NOT NULL,
	[FormsVersion] [varchar](50) NULL,
	[Operation] [varchar](20) NULL,
	[IsUpdate] [int] NULL,
	[UpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_tblShipUpdateNotificationMapping] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTest]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTest](
	[clmName] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTicket]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketNumber] [varchar](50) NULL,
	[Error] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[FilePath] [varchar](500) NULL,
	[Email] [varchar](500) NULL,
	[IsSolved] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
 CONSTRAINT [Pk_tblTicket] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserGroupMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserGroupMapping](
	[UserGroupId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
 CONSTRAINT [PK_tblUserGroupMapping] PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserMaster](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](200) NOT NULL,
	[Password] [varchar](200) NULL,
	[CreatedOn] [datetime] NULL,
	[IsActive] [int] NULL,
	[Email] [varchar](200) NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
	[Gender] [varchar](50) NULL,
	[VesselIMO] [varchar](50) NULL,
	[RankId] [int] NULL,
	[ShipId] [int] NULL,
	[UserCode] [varchar](500) NOT NULL,
	[UserType] [int] NOT NULL,
	[UploadPermission] [int] NULL,
	[JoinDate] [datetime] NULL,
	[ReleaseDate] [datetime] NULL,
	[IsAdmin] [int] NULL,
 CONSTRAINT [PK_tblUserMaster] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVesselSubSubType]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVesselSubSubType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VesselSubTypeID] [int] NOT NULL,
	[VesselSubSubTypeDecsription] [varchar](100) NULL,
 CONSTRAINT [PK_tblVesselSubSubType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVesselSubType]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVesselSubType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VesselTypeID] [int] NOT NULL,
	[SubTypeDescription] [varchar](100) NULL,
 CONSTRAINT [PK_tblVesselSubType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVesselType]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVesselType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tblVesselType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblVolumeMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVolumeMaster](
	[VolumeId] [int] IDENTITY(1,1) NOT NULL,
	[VolumeName] [varchar](200) NOT NULL,
	[VolumeMasterDesc] [varchar](500) NOT NULL,
	[ControllerName] [varchar](500) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [Pk_tblVolumeMaster] PRIMARY KEY CLUSTERED 
(
	[VolumeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblUserMaster] ADD  CONSTRAINT [DF_tblUserMaster_UploadPermission]  DEFAULT ((0)) FOR [UploadPermission]
GO
ALTER TABLE [dbo].[tblPermissionRoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblPermissionRoleMapping_tblPermissionMaster] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[tblPermissionMaster] ([PermissionId])
GO
ALTER TABLE [dbo].[tblPermissionRoleMapping] CHECK CONSTRAINT [FK_tblPermissionRoleMapping_tblPermissionMaster]
GO
ALTER TABLE [dbo].[tblPermissionRoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblPermissionRoleMapping_tblRoleMaster] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tblRoleMaster] ([RoleId])
GO
ALTER TABLE [dbo].[tblPermissionRoleMapping] CHECK CONSTRAINT [FK_tblPermissionRoleMapping_tblRoleMaster]
GO
ALTER TABLE [dbo].[tblRolGroupMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblRolGroupMapping_tblGroupMaster] FOREIGN KEY([GroupId])
REFERENCES [dbo].[tblGroupMaster] ([GroupId])
GO
ALTER TABLE [dbo].[tblRolGroupMapping] CHECK CONSTRAINT [FK_tblRolGroupMapping_tblGroupMaster]
GO
ALTER TABLE [dbo].[tblRolGroupMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblRolGroupMapping_tblRoleMaster] FOREIGN KEY([RoleId])
REFERENCES [dbo].[tblRoleMaster] ([RoleId])
GO
ALTER TABLE [dbo].[tblRolGroupMapping] CHECK CONSTRAINT [FK_tblRolGroupMapping_tblRoleMaster]
GO
ALTER TABLE [dbo].[tblShip]  WITH CHECK ADD  CONSTRAINT [FK_tblShip_tblVesselSubSubType] FOREIGN KEY([VesselSubSubTypeID])
REFERENCES [dbo].[tblVesselSubSubType] ([ID])
GO
ALTER TABLE [dbo].[tblShip] CHECK CONSTRAINT [FK_tblShip_tblVesselSubSubType]
GO
ALTER TABLE [dbo].[tblShip]  WITH CHECK ADD  CONSTRAINT [FK_tblShip_tblVesselSubType] FOREIGN KEY([VesselSubTypeID])
REFERENCES [dbo].[tblVesselSubType] ([ID])
GO
ALTER TABLE [dbo].[tblShip] CHECK CONSTRAINT [FK_tblShip_tblVesselSubType]
GO
ALTER TABLE [dbo].[tblShip]  WITH CHECK ADD  CONSTRAINT [FK_tblShip_tblVesselType] FOREIGN KEY([VesselTypeID])
REFERENCES [dbo].[tblVesselType] ([Id])
GO
ALTER TABLE [dbo].[tblShip] CHECK CONSTRAINT [FK_tblShip_tblVesselType]
GO
ALTER TABLE [dbo].[tblUserGroupMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblUserGroupMapping_tblGroupMaster] FOREIGN KEY([GroupId])
REFERENCES [dbo].[tblGroupMaster] ([GroupId])
GO
ALTER TABLE [dbo].[tblUserGroupMapping] CHECK CONSTRAINT [FK_tblUserGroupMapping_tblGroupMaster]
GO
ALTER TABLE [dbo].[tblUserGroupMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblUserGroupMapping_tblUserMaster] FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUserMaster] ([UserId])
GO
ALTER TABLE [dbo].[tblUserGroupMapping] CHECK CONSTRAINT [FK_tblUserGroupMapping_tblUserMaster]
GO
ALTER TABLE [dbo].[tblVesselSubSubType]  WITH CHECK ADD  CONSTRAINT [FK_tblVesselSubSubType_tblVesselSubType] FOREIGN KEY([VesselSubTypeID])
REFERENCES [dbo].[tblVesselSubType] ([ID])
GO
ALTER TABLE [dbo].[tblVesselSubSubType] CHECK CONSTRAINT [FK_tblVesselSubSubType_tblVesselSubType]
GO
ALTER TABLE [dbo].[tblVesselSubType]  WITH CHECK ADD  CONSTRAINT [FK_tblVesselSubType_tblVesselType] FOREIGN KEY([VesselTypeID])
REFERENCES [dbo].[tblVesselType] ([Id])
GO
ALTER TABLE [dbo].[tblVesselSubType] CHECK CONSTRAINT [FK_tblVesselSubType_tblVesselType]
GO
/****** Object:  StoredProcedure [dbo].[CheckCommonToAllFileExist]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : CheckCommonToAllFileExist :  Check File name exist or not 
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 27/03/2021
*******************************************************/

CREATE PROCEDURE [dbo].[CheckCommonToAllFileExist]
(
	@Name varchar(max),
	@ReturnMessage INT OUTPUT
)
AS
BEGIN
	Select @ReturnMessage = Count(*) from tblCommonToAllManual where Convert(varChar(max),[Name]) = @Name
END
GO
/****** Object:  StoredProcedure [dbo].[CheckReferenceMaterialFileExist]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : CheckReferenceMaterialFileExist :  Check File name exist or not 
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 27/03/2021
*******************************************************/

create PROCEDURE [dbo].[CheckReferenceMaterialFileExist]
(
	@Name varchar(max),
	@ReturnMessage INT OUTPUT
)
AS
BEGIN
	Select @ReturnMessage = Count(*) from tblReferenceMaterialManual where Convert(varChar(max),[Name]) = @Name
END
GO
/****** Object:  StoredProcedure [dbo].[CheckShipFileExist]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : CheckShipFileExist :  Check File name exist or not Ship wise
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 18/02/2021
*******************************************************/

CREATE PROCEDURE [dbo].[CheckShipFileExist]
(
	@Name varchar(max),
	@ShipNo int,
	@ReturnMessage INT OUTPUT
)
AS
BEGIN
	Select @ReturnMessage = Count(*) from tblShipsManual where Convert(varChar(max),[Name]) = @Name and ShipNo = @ShipNo
END
GO
/****** Object:  StoredProcedure [dbo].[CheckUserLogin]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : CheckUserLogin : Get User Login Details
				  By Email and Password with Ship Details
** Created By   : BINGSHU BAIDYA
** Created On   : 27/01/2021--Modified 31/03/2021
*******************************************************/

-- exec CheckUserLogin "12345", "user"

CREATE PROCEDURE [dbo].[CheckUserLogin]
(
	@Password VARCHAR(200),
	@UserCode VARCHAR(500)
)
AS
BEGIN
	DECLARE @ShipId int -- Added on 31/03/2021
	Select @ShipId = ShipId FROM tblUserMaster um WHERE UserCode = @UserCode AND Password = @Password AND IsActive =1-- Added on 31/03/2021
	SELECT um.UserId,um.UserCode,um.UserName,um.Email,um.UserType,  --,um.IsAdmin,
		   um.ShipId, VesselIMO
	FROM tblUserMaster um
	WHERE UserCode = @UserCode AND Password = @Password AND IsActive =1

	-- Added on 31/03/2021
	select Id, ShipName,IMONumber 
	from tblShip where Id=@ShipId
		
END
GO
/****** Object:  StoredProcedure [dbo].[CheckVolumeManualFileExist]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : CheckVolumeManualFileExist :  Check File name exist or not volume wise
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 03/04/2021
*******************************************************/

CREATE PROCEDURE [dbo].[CheckVolumeManualFileExist]
(
	@Name varchar(max),
	@VolId int,
	@ReturnMessage INT OUTPUT
)
AS
BEGIN
	Select @ReturnMessage = Count(*) from tblManual where Convert(varChar(max),[ManualFileName]) = @Name and VolumeId = @VolId
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteApprover]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : DeleteApprover : Delete Approver(soft Delete)
** Created By   : BINGSHU BAIDYA
** Created On   : 19/01/2021
*******************************************************/
CREATE procedure [dbo].[DeleteApprover] 
( 
	@ApproverMasterId int
) 
AS 
BEGIN 
	UPDATE tblApproverMaster 
			SET IsActive = 0
	WHERE ID = @ApproverMasterId
RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteForms]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteForms]
(
	@FormName VARCHAR(500)
)
AS
BEGIN
	UPDATE tblForms
		SET IsActive = 0
	WHERE FormName = @FormName

	
END
GO
/****** Object:  StoredProcedure [dbo].[GetActionByFileName]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetActionByFileName : Get Action 
				  By File Name
** Created By   : BINGSHU BAIDYA
** Created On   : 13/02/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetActionByFileName]
(
	@FileName varchar(max)
)
AS
BEGIN
	SELECT ManualId, ManualFileName,ActionName,ControllerName 
	FROM tblManual
	where CONVERT(VARCHAR(MAX),ManualFileName) = @FileName

END
GO
/****** Object:  StoredProcedure [dbo].[GetAllApproverListPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetAllApproverListPageWise : Get All 
				  Approver List PageWise
** Created By   : BINGSHU BAIDYA
** Created On   : 18/01/2021
*******************************************************/
create procedure [dbo].[GetAllApproverListPageWise]
(
	@PageIndex INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ROW_NUMBER() OVER
      (
            ORDER BY [UserName] ASC
      )AS RowNumber
	  ,am.ID
	  ,am.ShipId
	  ,s.ShipName
	  ,am.VesselIMONumber
	  ,am.UserId
	  ,um.UserName
	  ,am.RankId
	  ,r.RankName
	  ,am.ApproverId
	  ,a.Descripton as ApproverDescription


	INTO #Results

	FROM tblApproverMaster am
		inner join tblShip s
			on s.ID = am.ShipId
		inner join tblRank r
			on r.RankId = am.RankId
		inner join tblUserMaster um
			on um.UserId = am.UserId
		inner join tblApprover a
			on a.ID = am.ApproverId
	where am.IsActive = 1



	SELECT @TotalCount = COUNT(*)
    FROM #Results       
    SELECT * FROM #Results
    WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
    DROP TABLE #Results
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllShipDetailsPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllShipDetailsPageWise]
(
	@PageIndex INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [ShipName] ASC
      )AS RowNumber
	  ,ID
	  ,ShipName
	  ,IMONumber
	  ,FlagOfShip
	  ,Regime
	  ,ShipEmail
	  ,ShipEmail2
	  ,VesselTypeID
	  ,VesselSubTypeID
	  ,VesselSubSubTypeID
	  ,Voices1
	  ,Voices2
	  ,Fax1
	  ,Fax2
	  ,VOIP1
	  ,VOIP2
	  ,Mobile1
	  ,Mobile2
	  ,HelicopterDeck
	  ,HelicopterWinchingArea

	  INTO #Results

	  FROM dbo.tblShip

	   SELECT @TotalCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllUserForDropDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.1
** Procedure    : GetAllUserForDropDown : Get All 
				  User List for DropDown (Ship wise or all)
** Created By   : BINGSHU BAIDYA
** Created On   : 18/01/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetAllUserForDropDown]
(
	@ShipId int = null
)
AS

BEGIN
	if(@ShipId is null)
		BEGIN
			SELECT UserId as ID,
				   UserName as Name
			FROM tblUserMaster
			WHERE IsActive = 1 
		END
	ELSE
		BEGIN
			SELECT UserId as ID,
				   UserName as Name
			FROM tblUserMaster
			WHERE IsActive = 1 
				  AND ShipId= @ShipId 
				  AND UserId not in(select distinct UserId from tblApproverMaster  where ShipId= @ShipId)/*--Added on 19th Jan 2021--*/
		END

	
	
END

GO
/****** Object:  StoredProcedure [dbo].[GetApproverLevelForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetApproverLevelForDopDown : Get Approver 1, 2 ,3 ,4, 5, 6 
				  for DropDown only
** Created By   : BINGSHU BAIDYA
** Created On   : 21/01/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetApproverLevelForDopDown]

AS
BEGIN
	SELECT ID,
		   Descripton
	FROM tblApprover 
END
GO
/****** Object:  StoredProcedure [dbo].[GetApproverListByShipForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetApproverListByShipForDopDown : Get Approver 
				  User Ship wise
** Created By   : BINGSHU BAIDYA
** Created On   : 21/01/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetApproverListByShipForDopDown]
(
	@ShipId int
)
AS
BEGIN
	SELECT am.UserId as UserId,
		   um.UserCode,
		   um.UserName
	FROM tblApproverMaster am
		 INNER JOIN tblUserMaster um
		 ON UM.UserId= am.UserId
	WHERE am.ShipId = @ShipId
END
GO
/****** Object:  StoredProcedure [dbo].[GetCategoryList]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetCategoryList]
as
begin
	select ID,CatecoryName,[Description]
	from tblFormsCategory
end
GO
/****** Object:  StoredProcedure [dbo].[GetCommonToAllManualByControllerAction]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetCommonToAllManualByControllerAction : Get Common to All Manuals 
				  By Controller and Action Name
** Created By   : BINGSHU BAIDYA
** Created On   : 30/03/2021
*******************************************************/

CREATE PROCEDURE [dbo].[GetCommonToAllManualByControllerAction]
(
	@ControllerName VARCHAR(250),
	@ActionName VARCHAR(500)
)
AS
BEGIN
	SELECT Id
		  ,CONVERT(VARCHAR(MAX),[Name]) as [Name] 
		  ,CONVERT(VARCHAR(MAX),Content) as Content
		  ,BodyHeader
		  ,BodyText
		  ,BodyHtml
		  ,ActionName
		  ,ControllerName
	FROM tblCommonToAllManual
	where ControllerName=@ControllerName AND ActionName=@ActionName
END
GO
/****** Object:  StoredProcedure [dbo].[GetFormIdForModifiedSection]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- GetFormIdForModifiedSection
CREATE PROCEDURE [dbo].[GetFormIdForModifiedSection]
--(
--	@Id INT
--)
AS
BEGIN
SELECT ID AS FormId --,
       --FormName ,
       --Version  ,
	   --(select convert(varchar(10), UpdatedOn, 120)) as UpdatedOn
FROM tblForms
WHERE UpdatedOn > (select max(convert(varchar(10), UpdatedOn, 120)) as UpdatedOn FROM tblForms)

END
GO
/****** Object:  StoredProcedure [dbo].[GetFormsListByCategoryForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec GetFormsListByCategoryForDopDown '4'

CREATE procedure [dbo].[GetFormsListByCategoryForDopDown]
(
	@CategoryId int
)
as
begin
	select ID, FormName as Name
	from tblForms
	where IsActive = 1 and CategoryId = @CategoryId
end
GO
/****** Object:  StoredProcedure [dbo].[GetFormsListCategoryWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetFormsListCategoryWise]
(
	@CategoryId INT
)
AS
BEGIN
	SELECT ROW_NUMBER() OVER(  ORDER BY FormName ASC )AS RowNumber,  ID,FormName,[Description],[Path]
	FROM tblForms
	where IsActive =1 AND CategoryId=@CategoryId
END
GO
/****** Object:  StoredProcedure [dbo].[GetManualByControllerAction]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetManualByControllerAction : Get Manuals 
				  By Controller and Action Name
** Created By   : BINGSHU BAIDYA
** Created On   : 18/01/2021
*******************************************************/

CREATE PROCEDURE [dbo].[GetManualByControllerAction]
(
	@ControllerName VARCHAR(250),
	@ActionName VARCHAR(500)
)
AS
BEGIN
	SELECT ManualId
		  ,CONVERT(VARCHAR(MAX),[ManualFileName]) as ManualFileName 
		  ,CONVERT(VARCHAR(MAX),ManualHtml) as ManualHtml
		  ,ManualHeader
		  ,ManualBodyText
		  ,ManualBodyHtml
		  ,ActionName
		  ,ControllerName
	FROM tblManual
	where ControllerName=@ControllerName AND ActionName=@ActionName
END
GO
/****** Object:  StoredProcedure [dbo].[GetReferenceMaterialManualByControllerAction]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetReferenceMaterialManualByControllerAction : Get Reference Materials Manuals 
				  By Controller and Action Name
** Created By   : BINGSHU BAIDYA
** Created On   : 30/03/2021
*******************************************************/

create PROCEDURE [dbo].[GetReferenceMaterialManualByControllerAction]
(
	@ControllerName VARCHAR(250),
	@ActionName VARCHAR(500)
)
AS
BEGIN
	SELECT Id
		  ,CONVERT(VARCHAR(MAX),[Name]) as [Name] 
		  ,CONVERT(VARCHAR(MAX),Content) as Content
		  ,BodyHeader
		  ,BodyText
		  ,BodyHtml
		  ,ActionName
		  ,ControllerName
	FROM tblReferenceMaterialManual
	where ControllerName=@ControllerName AND ActionName=@ActionName
END
GO
/****** Object:  StoredProcedure [dbo].[GetRevisionDetails]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetRevisionDetails : Get Revision  details from 
				  Revision Header and History
** Created By   : BINGSHU BAIDYA
** Created On   : 03/04/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetRevisionDetails]

AS
BEGIN
	SELECT Id, [RevisionNo],[RevisionDate]
	FROM tblRevisionHeader

	SELECT RevisionHistoryId,Chapter ,Section,ChangeComment,ModificationDate,HeaderId

	FROM tblRevisionHistory
	where HeaderId in (SELECT Id FROM tblRevisionHeader)

END
GO
/****** Object:  StoredProcedure [dbo].[GetRevisionHeaderForDropDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetRevisionHeaderForDropDown : Get Revision  details fro DropDown from 
				  Revision Header
** Created By   : BINGSHU BAIDYA
** Created On   : 04/04/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetRevisionHeaderForDropDown]

AS
BEGIN
	SELECT Id, RevisionNo,RevisionDate
	FROM tblRevisionHeader
	order by Id desc


END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleByUserId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- GetRoleByUserId 1011
CREATE PROCEDURE [dbo].[GetRoleByUserId]
(
	@UserId int
)
AS
BEGIN
	SELECT R.RoleName
	FROM tblRoles R

	Inner JOIN tblRoleGroup RG
    ON RG.RoleId = R.Id

	Inner JOIN tblGroupMaster GM
    ON GM.GroupId = RG.GroupId

    Inner JOIN tblUserGroupMapping UGM
    ON UGM.GroupId = GM.GroupId

	Inner JOIN tblUserMaster UM
    ON UM.UserId = UGM.UserId

	where UM.UserId = @UserId

END
GO
/****** Object:  StoredProcedure [dbo].[GetShipDetailsById]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetShipDetailsById : Get Ship Details
				  By ID and Ship wise total Approvers Count
** Created By   : BINGSHU BAIDYA
** Created On   : 11/01/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetShipDetailsById]
(
	@ShipId INT
)
AS
BEGIN
	SELECT ID, ShipName,IMONumber,FlagOfShip as Flag,ShipEmail,ShipEmail2,Voices1,Voices2,Fax1,Fax2,VOIP1,VOIP2,Mobile1,Mobile2,
		   VesselTypeID,VesselSubTypeID,VesselSubSubTypeID
	FROM tblShip
	where ID = @ShipId

	SELECT COUNT(UserId) as ApproversCount FROM tblApproverMaster WHERE ShipId=@ShipId AND IsActive = 1
END
GO
/****** Object:  StoredProcedure [dbo].[GetShipManualByControllerAction]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetShipManualByControllerAction : Get Ship's Manuals 
				  By Controller and Action Name
** Created By   : BINGSHU BAIDYA
** Created On   : 19/02/2021
*******************************************************/

CREATE PROCEDURE [dbo].[GetShipManualByControllerAction]
(
	@ControllerName VARCHAR(250),
	@ActionName VARCHAR(500)
)
AS
BEGIN
	SELECT Id
		  ,CONVERT(VARCHAR(MAX),[Name]) as [Name] 
		  ,CONVERT(VARCHAR(MAX),Content) as Content
		  ,BodyHeader
		  ,BodyText
		  ,BodyHtml
		  ,ActionName
		  ,ControllerName
	FROM tblShipsManual
	where ControllerName=@ControllerName AND ActionName=@ActionName
END
GO
/****** Object:  StoredProcedure [dbo].[GetVesselSubSubTypeListBySubTypeForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetVesselSubSubTypeListBySubTypeForDopDown]
(
	@VesselSubTypeId int
)
as
begin
	select ID, [VesselSubSubTypeDecsription] as Name
	from tblVesselSubSubType
	where VesselSubTypeID between -1 and @VesselSubTypeId
end
GO
/****** Object:  StoredProcedure [dbo].[GetVesselSubTypeListByTypeForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetVesselSubTypeListByTypeForDopDown]
(
	@VesselTypeId int
)
as
begin
	select ID, [SubTypeDescription] as Name
	from tblVesselSubType
	where VesselTypeID between -1 and @VesselTypeId
end
GO
/****** Object:  StoredProcedure [dbo].[GetVesselTypeListForDopDown]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetVesselTypeListForDopDown]
as
begin
	select ID, [Description] as Name
	from tblVesselType
end
GO
/****** Object:  StoredProcedure [dbo].[GetVolumeById]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : GetVolumeById : Get Volume Details
				  By ID 
** Created By   : BINGSHU BAIDYA
** Created On   : 13/02/2021
*******************************************************/
CREATE PROCEDURE [dbo].[GetVolumeById]
(
	@VolumeId INT
)
AS
BEGIN
	SELECT VolumeId, VolumeName,VolumeMasterDesc,ControllerName 
	FROM tblVolumeMaster
	where VolumeId = @VolumeId

END
GO
/****** Object:  StoredProcedure [dbo].[SaveApprover]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveApprover : save approver
** Created By   : BINGSHU BAIDYA
** Created On   : 18/01/2021
*******************************************************/
CREATE PROCEDURE [dbo].[SaveApprover]
(
	@Id			INT,
	@ShipId		INT,
	@IMONumber	INT,
	@RankId		INT,
	@UserId		INT,
	@ApproverId INT,
	@CreatedBy	INT

)
AS
BEGIN
	SET NOCOUNT ON;
	IF (@Id IS NULL or @Id = 0)
		BEGIN
			INSERT INTO tblApproverMaster(VesselIMONumber,ShipId,RankId,UserId,ApproverId,IsActive,CreatedOn,CreatedBy)
									VALUES(@IMONumber,@ShipId,@RankId,@UserId,@ApproverId,1,GETDATE(),@CreatedBy)
		END

	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[SaveCommonToAllManualHtml]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveCommonToAllManualHtml : Save Common to All Manual Html 
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 77/02/2021
*******************************************************/
create procedure [dbo].[SaveCommonToAllManualHtml]
(
	@Name varchar(max),
	@Content varchar(max),
	@BodyHeader varchar(500),
	@BodyText varchar(max),
	@BodyHtml varchar(max),
	@ControllerName varchar(500),
	--@ActionName varchar(500),
	--@CreatedAt datetime,
	@CreatedBy int
)
as
	begin
		SET NOCOUNT ON;
		DECLARE @Count int

		Select @Count = Count(*) from tblCommonToAllManual where Convert(varChar(max),[Name]) = @Name

		if( @Count = 0)
			BEGIN
				insert into tblCommonToAllManual([Name],Content,BodyHeader,BodyText,BodyHtml
									  ,ControllerName,CreatedAt,CreatedBy)
						values(
								Convert(varbinary(max),@Name)
								,Convert(varbinary(max),@Content)
								,@BodyHeader
								,@BodyText
								,@BodyHtml
								,@ControllerName
								--,@ActionName
								--,@CreatedAt
								,GETDATE()
								,@CreatedBy
							)
			END
		ELSE
			BEGIN
				UPDATE tblCommonToAllManual
					SET Content		= Convert(varbinary(max),@Content),
						BodyText	= @BodyText,
						BodyHtml	= @BodyHtml,
						UpdatedAt	= GETDATE(),
						UpdatedBy	= @CreatedBy
				WHERE Convert(varChar(max),[Name]) = @Name
						

			END
		
	
		SET NOCOUNT OFF;
	end
GO
/****** Object:  StoredProcedure [dbo].[SaveFilledUpFormsForApproval]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveFilledUpFormsForApproval : Save 
				  Filled up Forms for Approval
** Created By   : BINGSHU BAIDYA
** Created On   : 19/01/2021
*******************************************************/

CREATE PROCEDURE [dbo].[SaveFilledUpFormsForApproval]
(
	--@Id				INT				= 0,
	@Name			VARCHAR(500)	= NULL,
	@Path			VARCHAR(250)	= NULL,
	@ShipId			INT				= 0,
	@xmlApprovers	ntext			= NULL,
	@OriginalForm	VARCHAR(500)	= NULL,
	@User			INT				= 1
)
AS
BEGIN
	 
	declare @hDoc1			int,
			@counter		int,
	        @rowcount		int,
			@formId			int,
			@uploadedFormId int

	DECLARE @approver_id	INT,
			@user_id		INT

	SELECT @formId = ID FROM tblForms WHERE FormName= @OriginalForm
	
	IF(@formId is not Null)
		BEGIN
			SET NOCOUNT ON

			BEGIN TRANSACTION
			IF(@xmlApprovers is not null) EXEC SP_XML_PREPAREDOCUMENT @hDoc1 OUTPUT,@xmlApprovers 
			
			INSERT INTO tblFormsUploaded(FormId,ShipId,FormsPath,FormsName,IsApprove,CreatedBy,CreatedOn)
								 VALUES(@formId,@ShipId,@Path,@Name,0,@User,GETDATE())

								 SELECT @uploadedFormId = SCOPE_IDENTITY();
								IF(@@rowcount=0)
									BEGIN
										ROLLBACK TRANSACTION
										IF(@xmlApprovers is not null) EXEC sp_xml_removedocument @hDoc1
										
										RETURN 0
									END

			IF(@xmlApprovers is not null)
				BEGIN
					SET @counter = 1
					SELECT  @rowcount=count(row_id)  
					FROM OPENXML(@hDoc1,'approver/row', 2)  
					WITH( row_id int )

					WHILE(@counter <= @rowcount)
						begin
							select  
								@approver_id    = approverId,
								@user_id		= userId

							from openxml(@hDoc1,'approver/row',2)
							with
							( 
								approverId int,
								userId int,
								row_id int
							) xmlTemp where xmlTemp.row_id = @counter  
			
							if(@approver_id = 0)
								begin
										---******----
										rollback transaction
										if(@xmlApprovers is not null) exec sp_xml_removedocument @hDoc1
										return 0

								end
							else
								begin
									INSERT INTO tblFormsUploadedApproverMapping(UploadedFormId,ApproverId,ApproverUserId,IsApprove,CreatedOn)
																	VALUES(@uploadedFormId,@approver_id,@user_id,0,GETDATE())
				    
									if(@@rowcount=0)
										begin
											rollback transaction
											if(@xmlApprovers is not null) exec sp_xml_removedocument @hDoc1
											return 0
										end
								end
							set @counter = @counter + 1
						end
				end
			
			COMMIT TRANSACTION
			SET NOCOUNT OFF

			RETURN 1
		END
	ELSE
		BEGIN
			RETURN 0
		END
		
END
GO
/****** Object:  StoredProcedure [dbo].[SaveFormsDetails]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveFormsDetails : save
                  institution 
** Created By   : BINGSHU BAIDYA
** Created On   : 08/01/2021
*******************************************************/

CREATE PROCEDURE [dbo].[SaveFormsDetails]
(
	@Name varchar(500) ,
	@Path varchar(500),
	@Description varchar(1000),
	@Version varchar(50),
	@CategoryId int,
	@User int

	,@ModifiedSection varchar(500) -- deep
)
AS
BEGIN
	declare @formId int,
			@oldVersion varchar(50),
			@counter int,
			@rowcount int,
			@shipId int
	create table #tmpShip(
		id int,
		rowNumber int
	);
	
	IF NOT EXISTS(select [FormName] FROM tblForms WHERE [FormName] = @Name AND CategoryId = @CategoryId)
		 BEGIN
			SET NOCOUNT ON;
			Insert into tblForms ([FormName],[CategoryId],[Description],[Version],[Path],[CreatedOn],CreatedBy)
			values (@Name,@CategoryId,@Description,@Version,@Path,GETDATE(),@User)
		END
	ELSE
		BEGIN
			select @formId = ID, @oldVersion = Version from tblForms where [FormName] =@Name AND CategoryId = @CategoryId
			update tblForms
				set [Version]	= @Version,
					[UpdatedOn] = GETDATE(),
					[UpdatedBy] = @User
			where[FormName] = @Name AND CategoryId = @CategoryId 

			if(@@rowcount = 1)

				begin
					INSERT INTO tblFormsArchiveLog([FormId],[FormsVersion],[Operation],[ArchivedDate],ModifiedSection)  -- deep
											VALUES(@formId,@oldVersion,'UPDATE',GETDATE(),@ModifiedSection)  -- deep
					
					INSERT INTO tblFormsUpdateNotification(FormId,[ShipId],Operation,UpdateNotifyDate)
						values(@formId,0,'UPDATE',GETDATE())
					DECLARE @UpdateNotifyId int = scope_identity()
					---may need to write some code in later time
					set @Counter = 1
					INSERT INTO #tmpShip(rowNumber,id)(SELECT ROW_NUMBER() OVER(  ORDER BY ID ASC )AS RowNumber,ID FROM tblShip)
					SELECT @rowcount = COUNT(id) FROM #tmpShip
					WHILE (@counter <= @rowcount)
						BEGIN
							SELECT @shipId = id FROM #tmpShip WHERE rowNumber = @counter
							INSERT INTO tblShipUpdateNotificationMapping(
																		  NotificationId,
																		  FormId,
																		  ShipId,IsUpdate
																		 )
																	VALUES(
																			@UpdateNotifyId,
																			@formId,
																			@shipId,
																			0
																		   )


							set @counter = @counter + 1

						END

				end

		END

	DROP TABLE #tmpShip
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[SaveManualHtml]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveManualHtml : Save Manual Html Search
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 02/02/2021
*******************************************************/
CREATE procedure [dbo].[SaveManualHtml]
(
	@Name varchar(max),
	@Content varchar(max),
	--@Name varbinary(max),
	--@Content varbinary(max),
	@BodyHeader varchar(500),
	@BodyText varchar(max),
	@BodyHtml varchar(max),
	@VolumeId int,
	@ControllerName varchar(500),
	--@ActionName varchar(500),
	--@CreatedAt datetime,
	@CreatedBy int
)
as
	begin
		SET NOCOUNT ON;
		DECLARE @Count int

		Select @Count = Count(*) from tblManual where Convert(varChar(max),ManualFileName) = @Name and VolumeId = @VolumeId

		if( @Count = 0)
			BEGIN
				insert into tblManual(ManualFileName,ManualHTML,ManualHeader,ManualBodyText,ManualBodyHtml,VolumeId
									  ,ControllerName,CreatedOn,CreatedBy)
						values(
								Convert(varbinary(max),@Name)
								,Convert(varbinary(max),@Content)
								--@Name
								--,@Content
								,@BodyHeader
								,@BodyText
								,@BodyHtml
								,@VolumeId
								,@ControllerName
								--,@ActionName
								--,@CreatedAt
								,GETDATE()
								,@CreatedBy
							)
			END
		ELSE
			BEGIN
				UPDATE tblManual
					SET ManualHTML		= Convert(varbinary(max),@Content),
						ManualBodyText	= @BodyText,
						ManualBodyHtml	= @BodyHtml
				WHERE Convert(varChar(max),ManualFileName) = @Name and VolumeId = @VolumeId

				--UPDATE tblManual
				--	SET ManualHTML		= @Content,
				--		ManualBodyText	= @BodyText,
				--		ManualBodyHtml	= @BodyHtml
				--WHERE ManualFileName = @Name and VolumeId = @VolumeId
						

			END
		
	
		SET NOCOUNT OFF;
	end
GO
/****** Object:  StoredProcedure [dbo].[SaveReferenceMaterialManualHtml]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveReferenceMaterialManualHtml : Save Reference Material Manual Html 
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 77/02/2021
*******************************************************/
create procedure [dbo].[SaveReferenceMaterialManualHtml]
(
	@Name varchar(max),
	@Content varchar(max),
	@BodyHeader varchar(500),
	@BodyText varchar(max),
	@BodyHtml varchar(max),
	@ControllerName varchar(500),
	--@ActionName varchar(500),
	--@CreatedAt datetime,
	@CreatedBy int
)
as
	begin
		SET NOCOUNT ON;
		DECLARE @Count int

		Select @Count = Count(*) from tblReferenceMaterialManual where Convert(varChar(max),[Name]) = @Name

		if( @Count = 0)
			BEGIN
				insert into tblReferenceMaterialManual([Name],Content,BodyHeader,BodyText,BodyHtml
									  ,ControllerName,CreatedAt,CreatedBy)
						values(
								Convert(varbinary(max),@Name)
								,Convert(varbinary(max),@Content)
								,@BodyHeader
								,@BodyText
								,@BodyHtml
								,@ControllerName
								--,@ActionName
								--,@CreatedAt
								,GETDATE()
								,@CreatedBy
							)
			END
		ELSE
			BEGIN
				UPDATE tblReferenceMaterialManual
					SET Content		= Convert(varbinary(max),@Content),
						BodyText	= @BodyText,
						BodyHtml	= @BodyHtml,
						UpdatedAt	= GETDATE(),
						UpdatedBy	= @CreatedBy
				WHERE Convert(varChar(max),[Name]) = @Name
						

			END
		
	
		SET NOCOUNT OFF;
	end
GO
/****** Object:  StoredProcedure [dbo].[SaveRevisionHeader]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveRevisionHeader : Save Revision Revision Header
** Created By   : BINGSHU BAIDYA
** Created On   : 04/04/2021
*******************************************************/

create PROCEDURE [dbo].[SaveRevisionHeader]
(
	@RevisionHeaderId int,
	@RevisionNo varchar(1000),
	@CreatedBy int,
	@RevisionDate varchar(50)
)
AS
BEGIN
	IF @RevisionHeaderId IS NULL
		 BEGIN
			Insert into tblRevisionHeader (RevisionNo,RevisionDate,CreatedAt,CreatedBy)
									values(@RevisionNo, @RevisionDate,CONVERT(varchar(50), GETDATE()), @CreatedBy)
		END
	ELSE
		BEGIN
			UPDATE [dbo].tblRevisionHeader 
				SET RevisionNo=@RevisionNo,
					RevisionDate=@RevisionDate
			WHERE Id=@RevisionHeaderId 
		END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveShipDetails]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveShipDetails]
(
	@Id int,
	@Name varchar(21) ,
	@IMONumber int,
	@Flag varchar(50),
	@CompanyId int,
	@VesselTypeId int,
	@VesselSubTypeId int,
	@VesselSubSubTypeId int,
	@ShipEmail1 varchar(200),
	@ShipEmail2 varchar(200),
	@Mobile1  varchar(100),
	@Mobile2 varchar(100),
	@Fax1 varchar(100),
	@Fax2 varchar(100),
	@User int
)
AS
BEGIN
	SET NOCOUNT ON;
	IF @Id IS NULL
		BEGIN

			Insert into tblShip(ShipName,IMONumber,FlagOfShip,ShipEmail,ShipEmail2,Fax1,Fax2,Mobile1,Mobile2,
								 CompanyID,VesselTypeID,VesselSubTypeID,VesselSubSubTypeID,CreatedOn,CreatedBy)
								values(@Name,@IMONumber,@Flag,@ShipEmail1,@ShipEmail2,@Fax1,@Fax2,@Mobile1,@Mobile2,
								@CompanyId,@VesselTypeId,@VesselSubTypeId,@VesselSubSubTypeId,GETDATE(),@User)
		END
	ELSE
		BEGIN
			UPDATE tblShip
				set 
					ShipName			= @Name
					,IMONumber			= @IMONumber
					,FlagOfShip			= @Flag
					,ShipEmail			= @ShipEmail1
					,ShipEmail2			= @ShipEmail2
					,Fax1				= @Fax1
					,Fax2				= @Fax2
					,Mobile1			= @Mobile1
					,Mobile2			= @Mobile2
					,CompanyID			= @CompanyId
					,VesselTypeID		= @VesselTypeId
					,VesselSubTypeID	= @VesselSubTypeId
					,VesselSubSubTypeID	= @VesselSubSubTypeId
					,UpdatedOn			= GETDATE()
					,UpdatedBy			= @User
			
			WHERE ID = @Id
		END
	
	SET NOCOUNT OFF;
END
GO
/****** Object:  StoredProcedure [dbo].[SaveShipManualHtml]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveShipManualHtml : Save Ship Manual Html
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 17/02/2021
*******************************************************/
CREATE procedure [dbo].[SaveShipManualHtml]
(
	@Name varchar(max),
	@Content varchar(max),
	@BodyHeader varchar(500),
	@BodyText varchar(max),
	@BodyHtml varchar(max),
	@ShipNo int,
	@ControllerName varchar(500),
	--@ActionName varchar(500),
	--@CreatedAt datetime,
	@CreatedBy int
)
as
	begin
		SET NOCOUNT ON;
		DECLARE @Count int

		Select @Count = Count(*) from tblShipsManual where Convert(varChar(max),[Name]) = @Name and ShipNo = @ShipNo

		if( @Count = 0)
			BEGIN
				insert into tblShipsManual([Name],Content,BodyHeader,BodyText,BodyHtml,ShipNo
									  ,ControllerName,CreatedAt,CreatedBy)
						values(
								Convert(varbinary(max),@Name)
								,Convert(varbinary(max),@Content)
								,@BodyHeader
								,@BodyText
								,@BodyHtml
								,@ShipNo
								,@ControllerName
								--,@ActionName
								--,@CreatedAt
								,GETDATE()
								,@CreatedBy
							)
			END
		ELSE
			BEGIN
				UPDATE tblShipsManual
					SET Content		= Convert(varbinary(max),@Content),
						BodyText	= @BodyText,
						BodyHtml	= @BodyHtml
				WHERE Convert(varChar(max),[Name]) = @Name and ShipNo = @ShipNo
						

			END
		
	
		SET NOCOUNT OFF;
	end
GO
/****** Object:  StoredProcedure [dbo].[SaveTicketDetails]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : SaveTicketDetails : Save tICKET dETAILS
** Description  : 
** Created By   : BINGSHU BAIDYA
** Created On   : 01/04/2021
*******************************************************/
Create procedure [dbo].[SaveTicketDetails]
(
	@Error varchar(max),
	@Description varchar(max),
	@FilePath varchar(500),
	@CreatedBy int,
	@TicketNumber int output
)
AS
	BEGIN
		SET NOCOUNT ON;
		insert into tblTicket(Error,[Description],FilePath,CreatedAt,CreatedBy)
						values(@Error
								,@Description
								,@FilePath
								,GETDATE()
								,@CreatedBy
							)
		set @TicketNumber = @@IDENTITY 

		return @TicketNumber

		SET NOCOUNT OFF;
	END
GO
/****** Object:  StoredProcedure [dbo].[SearchManuals]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 2.0.0.0
** Procedure    : SearchManuals : Search Manual Volume wise,
				  Ship wise and Both, Common to All Manual and Reference Material also
** Created By   : BINGSHU BAIDYA
** Created On   : 10/01/2021
** Modified On   : 15/02/2021;27/03/2021
*******************************************************/
--declare @x int Exec SearchManualsPagewise 1,10, @x out, 1, 'Business'

--declare @x int Exec SearchManuals 1, 'Business'
CREATE PROCEDURE [dbo].[SearchManuals]
(
	@VolumeId	int				= 0,
	@ShipId		int				= 0,
	@SearchText varchar(1000)
)
AS
	BEGIN
		set nocount on;
		declare @strSQL varchar(max),
				@strVolSQL varchar(max),--Added on 15th Feb 2021 @BK
				@strC2ASQL varchar(max),--Added on 27th Mar 2021 @BK
				@strRefMQL varchar(max)--Added on 27th Mar 2021 @BK

		set @strVolSQL='select ROW_NUMBER() OVER(  ORDER BY [ManualHeader] ASC )AS RowNumber,'
		--set @strSQL='select '
		set @strVolSQL=@strVolSQL +' ManualId, CONVERT(VARCHAR(MAX), [ManualFileName]) AS [ManualFileName],'
        set @strVolSQL=@strVolSQL +' CONVERT(VARCHAR(MAX), [ManualHTML]) AS [ManualHTML] ,[ControllerName],'
		set @strVolSQL=@strVolSQL +'[ActionName],[ManualBodyText],[ManualHeader] '
		set @strVolSQL=@strVolSQL +' FROM tblManual m '
		set @strVolSQL=@strVolSQL +' WHERE 1=1 '
		
		IF(isnull(@VolumeId,0 )<> 0)
			begin
				set @strVolSQL= @strVolSQL + ' AND VolumeId ='+ CONVERT(varchar,@VolumeId)
			end
		if(isnull(@SearchText,'') <>'')
			begin
				set @strVolSQL= @strVolSQL + ' AND FREETEXT(*,''' + @SearchText + ''') '
			end

	   -------------------Added on 15th Feb 2021----@BK-----------------------------------------------
	   declare @strShipSQL varchar(max)
	   set @strShipSQL='select ROW_NUMBER() OVER(  ORDER BY [BodyHeader] ASC )AS RowNumber,'
		--set @strSQL='select '
		set @strShipSQL=@strShipSQL +' Id, CONVERT(VARCHAR(MAX), [Name]) AS [ManualFileName],'
        set @strShipSQL=@strShipSQL +' CONVERT(VARCHAR(MAX), [Content]) AS [ManualHTML] ,[ControllerName],'
		set @strShipSQL=@strShipSQL +'[ActionName],[BodyText] as [ManualBodyText],BodyHeader as [ManualHeader] '
		set @strShipSQL=@strShipSQL +' FROM tblShipsManual'
		set @strShipSQL=@strShipSQL +' WHERE 1=1 '
		
		IF(isnull(@ShipId,0 )<> 0)
			begin
				set @strShipSQL= @strShipSQL + ' AND ShipNo ='+ CONVERT(varchar,@ShipId)
			end
		if(isnull(@SearchText,'') <>'')
			begin
				set @strShipSQL= @strShipSQL + ' AND FREETEXT(*,''' + @SearchText + ''') '
			end
		---------------End-----------Ship Sql -------------------------------------------------------------------------------

		-------------Common to all-----------------Added on 27th Mar 2021----@BK------------------------------
		
		set @strC2ASQL = N'select ROW_NUMBER() OVER(  ORDER BY [BodyHeader] ASC )AS RowNumber,'
		--set @strSQL='select '
		set @strC2ASQL = @strC2ASQL + N' Id, CONVERT(VARCHAR(MAX), [Name]) AS [ManualFileName],'
        set @strC2ASQL = @strC2ASQL + N' CONVERT(VARCHAR(MAX), [Content]) AS [ManualHTML] ,[ControllerName],'
		set @strC2ASQL = @strC2ASQL + N'[ActionName],[BodyText] as [ManualBodyText],BodyHeader as [ManualHeader] '
		set @strC2ASQL = @strC2ASQL + N' FROM tblCommonToAllManual'
		set @strC2ASQL = @strC2ASQL + N' WHERE 1=1 '
		set @strC2ASQL = @strC2ASQL + N' AND FREETEXT(*,''' + @SearchText + ''') '
		------------end ----Common to all----sql-----------------------------------------------------------------------------

		-------------Reference Material-----------------Added on 27th Mar 2021----@BK------------------------------
		
		set @strRefMQL = N'select ROW_NUMBER() OVER(  ORDER BY [BodyHeader] ASC )AS RowNumber,'
		--set @strSQL='select '
		set @strRefMQL = @strRefMQL + N' Id, CONVERT(VARCHAR(MAX), [Name]) AS [ManualFileName],'
        set @strRefMQL = @strRefMQL + N' CONVERT(VARCHAR(MAX), [Content]) AS [ManualHTML] ,[ControllerName],'
		set @strRefMQL = @strRefMQL + N'[ActionName],[BodyText] as [ManualBodyText],BodyHeader as [ManualHeader] '
		set @strRefMQL = @strRefMQL + N' FROM tblReferenceMaterialManual'
		set @strRefMQL = @strRefMQL + N' WHERE 1=1 '
		set @strRefMQL = @strRefMQL + N' AND FREETEXT(*,''' + @SearchText + ''') '
		------------end ----Reference Material----sql-----------------------------------------------------------------------------

		---------below lines -Added on 15th Feb 2021-----@BK-------------------------
		IF(isnull(@ShipId,0 )= 0 AND isnull(@VolumeId,0 )= 0)
			BEGIN
				set @strSQL = @strVolSQL +'  UNION ALL ' + @strShipSQL + ' UNION ALL '+ @strC2ASQL + ' UNION ALL '+ @strRefMQL
			END
		ELSE
			BEGIN
				IF(isnull(@ShipId,0 )<> 0)
					BEGIN
						set @strSQL = @strShipSQL
					END
				ELSE IF(isnull(@VolumeId,0 )<> 0)
					BEGIN
						set @strSQL = @strVolSQL
					END
			END
		------End-------------Added on 15th Feb 2021-----@BK----------------------------------------------
		exec(@strSQL)
		set nocount off;
	END
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteGroupMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- stpDeleteGroupMaster 1
create procedure [dbo].[stpDeleteGroupMaster] 
( 
@GroupId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0
BEGIN
UPDATE tblGroupMaster SET IsActive = 0
WHERE GroupId = @GroupId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpDeletePermissionRole]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpDeletePermissionRole 6
CREATE procedure [dbo].[stpDeletePermissionRole] 
( 
@PermissionRoleId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0



BEGIN
--UPDATE tblUserGroupMapping SET IsActive = 0
Delete from tblPermissionRoleMapping
WHERE PermissionRoleId = @PermissionRoleId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteRank]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpDeleteRank '21'

create procedure [dbo].[stpDeleteRank] 
( 
@RankId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0
BEGIN
UPDATE tblRank SET IsActive = 0
WHERE RankId = @RankId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteRoleMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpDeleteRoleMaster 2
CREATE procedure [dbo].[stpDeleteRoleMaster] 
( 
@RoleId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0
BEGIN
UPDATE tblRoleMaster SET IsActive = 0
WHERE RoleId = @RoleId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteUserGroup]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec stpDeleteUserGroup '1'

CREATE procedure [dbo].[stpDeleteUserGroup] 
( 
@UserGroupId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0



BEGIN
--UPDATE tblUserGroupMapping SET IsActive = 0
Delete from tblUserGroupMapping
WHERE UserGroupId = @UserGroupId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteUserMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpDeleteUserMaster '1013'

CREATE procedure [dbo].[stpDeleteUserMaster] 
( 
@UserId int
) 
AS 
BEGIN 

DECLARE @UserMasterCount int
SET @UserMasterCount =0

SELECT @UserMasterCount = IsActive FROM tblUserMaster 
WHERE UserId = @UserId

IF @UserMasterCount = 0
BEGIN
UPDATE tblUserMaster SET IsActive = 1
WHERE UserId = @UserId
 
END
ELSE 
BEGIN
UPDATE tblUserMaster SET IsActive = 0
WHERE UserId = @UserId
END

RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllGroupMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpGetAllGroupMaster]  

AS  
Begin  
	Select  GroupName,CreatedBy,ModifiedBy ,GroupId 
	FROM dbo.tblGroupMaster  
    WHERE IsActive=1  
End 
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllGroupMasterPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  declare @x int Exec stpGetAllGroupMasterPageWise 1,10, @x out
create PROCEDURE [dbo].[stpGetAllGroupMasterPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 10,
      @RecordCount INT OUTPUT
	  --,@VesselID int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [GroupName] ASC
      )AS RowNumber

	  ,GroupId
     ,GroupName
	 ,CreatedBy
	 ,ModifiedBy

 INTO #Results

    FROM dbo.tblGroupMaster 
    --where UserId=@UserId
	where IsActive=1

      SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetAllGroupsNotInRoles]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpGetAllGroupsNotInRoles]  

AS  
Begin  
	Select  GroupName,GroupId,CreatedBy,ModifiedBy
	FROM dbo.tblGroupMaster  
    WHERE IsActive=1
	AND GroupId NOT IN (SELECT DISTINCT GroupId FROM tblRoleGroup)
End 
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllPermissionRoleByPermissionId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetAllPermissionRoleByPermissionId  1
CREATE PROCEDURE [dbo].[stpGetAllPermissionRoleByPermissionId]
(
@PermissionId int

)
AS
Begin
Select  P.PermissionName,R.RoleName,PR.CreatedBy,PR.ModifiedBy,R.RoleId

FROM dbo.tblPermissionRoleMapping PR
LEFT OUTER JOIN tblPermissionMaster P
ON PR.PermissionId = P.PermissionId

LEFT OUTER JOIN tblRoleMaster R
ON PR.RoleId = R.RoleId

WHERE PR.PermissionId= @PermissionId 
AND PR.IsActive=1

--where UGM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetAllRankPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- declare @x int Exec stpGetAllRankPageWise 1,100, @x out

CREATE PROCEDURE [dbo].[stpGetAllRankPageWise]
(
	@PageIndex INT = 1,
    @PageSize INT = 10,
    @RecordCount INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [RankName] ASC
      )AS RowNumber
	  ,RankId
	  ,RankName
	  ,Description
	  ,Email

	  INTO #Results

	  FROM dbo.tblRank
	  where IsActive=1
	   SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllRole]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   exec stpGetAllRole
CREATE PROCEDURE [dbo].[stpGetAllRole]
AS
BEGIN
	SELECT Id,RoleName 
	FROM tblRoles
	WHERE Id NOT IN ( SELECT DISTINCT RoleId FROM tblRoleGroup)
	end
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllRoleMasterPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  declare @x int Exec stpGetAllRoleMasterPageWise 1,10, @x out
create PROCEDURE [dbo].[stpGetAllRoleMasterPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 10,
      @RecordCount INT OUTPUT
	  --,@VesselID int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [RoleName] ASC
      )AS RowNumber

	  ,RoleId
     ,RoleName
	 ,CreatedBy
	 ,ModifiedBy

 INTO #Results

    FROM dbo.tblRoleMaster 
    --where UserId=@UserId
	where IsActive=1

      SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUser]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpGetAllUser]  
AS  
Begin  
Select  UserName,CreatedOn,UM.Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName,UserId  
FROM dbo.tblUserMaster UM  
LEFT OUTER JOIN tblRank R  
ON UM.RankId = R.RankId  
where UM.IsActive=1  
End 
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUserGroup]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   exec stpGetAllUserGroup
CREATE PROCEDURE [dbo].[stpGetAllUserGroup]


AS
BEGIN

Select
    UserGroupId
	  ,U.UserId
     ,U.UserName
	 ,G.GroupName AS SelectedGroups  -- should comma sapaa--------------

 INTO #Results

    FROM dbo.tblUserGroupMapping UG 

    LEFT OUTER JOIN tblUserMaster U
    ON UG.UserId = U.UserId

	LEFT OUTER JOIN tblGroupMaster G
    ON G.GroupId = UG.GroupId

    WHERE UG.IsActive=1
    --AND UM.IsActive=@UserId

    
     


	    SELECT DISTINCT UserId,UserName, SelectedGroups = 
     STUFF((SELECT ', ' + SelectedGroups
           FROM #Results b 
           WHERE b.UserId = a.UserId 
          FOR XML PATH('')), 1, 2, '')


      --SELECT * FROM #Results 
	  FROM #Results a
	 --GROUP BY UserGroupId,UserName,SelectedGroups
      DROP TABLE #Results
END



GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUserGroupByUserID]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   exec stpGetAllUserGroupByUserID  1011
CREATE PROCEDURE [dbo].[stpGetAllUserGroupByUserID]
(
@UserId int

)
AS
Begin
Select  UM.UserName,GM.GroupName,UGM.CreatedBy,UGM.ModifiedBy,GM.GroupId

FROM dbo.tblUserGroupMapping UGM
LEFT OUTER JOIN tblUserMaster UM
ON UGM.UserId = UM.UserId

LEFT OUTER JOIN tblGroupMaster GM
ON GM.GroupId = UGM.GroupId

WHERE UGM.UserId= @UserId 
AND UGM.IsActive=1

--where UGM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUserGroupPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  declare @x int Exec stpGetAllUserGroupPageWise 1,10, @x out
CREATE PROCEDURE [dbo].[stpGetAllUserGroupPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 10,
      @RecordCount INT OUTPUT
	  --,@VesselID int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [UserGroupId] ASC
      )AS RowNumber

	  ,UserGroupId
	  ,U.UserId
     ,U.UserName
	 ,G.GroupName AS SelectedGroups  -- should comma sapaa--------------

 INTO #Results

    FROM dbo.tblUserGroupMapping UG 

    LEFT OUTER JOIN tblUserMaster U
    ON UG.UserId = U.UserId

	LEFT OUTER JOIN tblGroupMaster G
    ON G.GroupId = UG.GroupId

    WHERE UG.IsActive=1
    --AND UM.IsActive=@UserId

      SELECT @RecordCount = COUNT(*)
      FROM #Results       


	    SELECT DISTINCT UserId,UserName, SelectedGroups = 
     STUFF((SELECT ', ' + SelectedGroups
           FROM #Results b 
           WHERE b.UserId = a.UserId 
          FOR XML PATH('')), 1, 2, '')


      --SELECT * FROM #Results 
	  FROM #Results a
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
	 --GROUP BY UserGroupId,UserName,SelectedGroups
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUserPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  declare @x int Exec stpGetAllUserPageWise 1,10, @x out, 2
CREATE PROCEDURE [dbo].[stpGetAllUserPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 10,
      @RecordCount INT OUTPUT
	  --,@VesselID int
	  ,@UserType int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [UserName] ASC
      )AS RowNumber

	  ,UserId
     ,UserName
	 ,UM.UserCode
	 --,SUBSTRING(CreatedOn, 1, 10)as CreatedOn
	 ,  convert(varchar, getdate(), 103) as CreatedOn
	 ,UM.Email as Email
	 ,UM.CreatedBy
	 ,ModifiedBy
	 ,Gender
	 ,VesselIMO
	 ,UploadPermission
	 ,IsActive
	
	 --,R.RankName
	 --,S.ShipName

 INTO #Results

    FROM dbo.tblUserMaster UM
	where
	UserType = @UserType

      SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetDownloadableFromsPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  declare @x int Exec stpGetDownloadableFromsPageWise 1,10, @x out, 9
CREATE PROCEDURE [dbo].[stpGetDownloadableFromsPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 10,
      @RecordCount INT OUTPUT
	  --,@VesselID int
	  ,@CategoryId int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [FormName] ASC
      )AS RowNumber

	  ,ID
     ,FormName
	 ,[Path]
	 ,[Version]

 INTO #Results

    FROM dbo.tblForms 
	where IsActive=1
	and CategoryId = @CategoryId

      SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetGroupMasterByGroupId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetGroupMasterByGroupId 3
CREATE PROCEDURE [dbo].[stpGetGroupMasterByGroupId]
(
@GroupId int
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
--@RankId int  
)
AS
Begin
Select  GroupId, GroupName
		--,CreatedBy,ModifiedBy

FROM tblGroupMaster 

WHERE GroupId= @GroupId
AND IsActive=1
End


GO
/****** Object:  StoredProcedure [dbo].[stpGetRankByRankId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   exec stpGetRankByRankId 1
create PROCEDURE [dbo].[stpGetRankByRankId]
(
@RankId int
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
--@RankId int  
)
AS
Begin
Select  RankId, RankName, Description

FROM tblRank

WHERE RankId= @RankId 
AND IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetRevisionHistoryPageWise]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  declare @x int Exec stpGetRevisionHistoryPageWise 1,1000, @x out           -----, 2
CREATE PROCEDURE [dbo].[stpGetRevisionHistoryPageWise]
 (
      @PageIndex INT = 1,
      @PageSize INT = 1000,
      @RecordCount INT OUTPUT
	  --,@VesselID int
)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
            ORDER BY [FormName] ASC
      )AS RowNumber

	 ,F.ID
     ,F.FormName
	 ,FA.ModifiedSection
	 ,convert(varchar, F.UpdatedOn, 103) as UpdatedOn
	 ,F.[Version]

 INTO #Results

    FROM tblForms F
    inner JOIN tblFormsArchiveLog FA
    ON FA.FormId = F.ID

	where IsActive=1
	--and VesselID=@VesselID

      SELECT @RecordCount = COUNT(*)
      FROM #Results       
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
      --AND VesselID = @VesselID
      DROP TABLE #Results
END

GO
/****** Object:  StoredProcedure [dbo].[stpGetRoleMasterByRoleId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetRoleMasterByRoleId 2
create PROCEDURE [dbo].[stpGetRoleMasterByRoleId]
(
@RoleId int
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
--@RankId int  
)
AS
Begin
Select  RoleId, RoleName
		--,CreatedBy,ModifiedBy

FROM tblRoleMaster 

WHERE RoleId= @RoleId
AND IsActive=1
End
GO
/****** Object:  StoredProcedure [dbo].[stpGetRoleNameByGroupId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpGetRoleNameByGroupId]  
(
	@GroupId int
)
AS  
Begin  
	Select  R.RoleName,R.Id
	FROM tblRoles R
	INNER JOIN tblRoleGroup RG
	ON R.Id = RG.RoleId
	WHERE RG.GroupId = @GroupId 
	
End  
GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByEmailId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByEmailId "hgf1"
CREATE PROCEDURE [dbo].[stpGetUserByEmailId]
(
--@UserId int,
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
@Email varchar(200)
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
--@RankId int  
)
AS
Begin
Select  UserName,CreatedOn,UM.Email as Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.Email= @Email 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByIMO]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByIMO "gfhfg"
CREATE PROCEDURE [dbo].[stpGetUserByIMO]
(
--@UserId int,
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
@VesselIMO varchar(50)--,
--@RankId int  
)
AS
Begin
Select  UserName,CreatedOn,UM.Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.VesselIMO= @VesselIMO 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByRank]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByRank 1
CREATE PROCEDURE [dbo].[stpGetUserByRank]
(
--@UserId int,
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
@RankId int  
)
AS
Begin
Select  UserName,CreatedOn,UM.Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.RankId= @RankId 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByUserId]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpGetUserByUserId]
(
@UserId int
--@UserName varchar(200),
--@Password varchar(200),
--@CreatedOn varchar(200),
--@IsActive int,
--@Email varchar(200),
--@CreatedBy varchar(200),
--@ModifiedBy varchar(200),
--@Gender varchar(50),
--@VesselIMO varchar(50),
--@RankId int  
)
AS
Begin
Select  UserId, UserName, [Password],
        UM.CreatedOn, --ISNULL(REPLACE(REPLACE(CONVERT(varchar(12),CreatedOn,dbo.ufunc_GetDateFormat()), ' ','-'), ',',''), '-') CreatedOn, 
        UM.Email as Email
		--,CreatedBy,ModifiedBy
		,Gender,VesselIMO
		, UM.RankId 
		,R.RankName
		,UM.ShipId
		,UserCode --Added on 20th JAN 2021 @Prasenjit
		,UserType
		,IsAdmin  --Added on 20th JAN 2021 @Prasenjit

FROM tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

LEFT OUTER JOIN tblShip S
ON UM.ShipId = S.ID

WHERE UM.UserId= @UserId 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserRoleByUserID]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpGetUserRoleByUserID 1012
create PROCEDURE [dbo].[stpGetUserRoleByUserID]
(
	@UserID INT
)
AS
BEGIN
	SELECT RM.RoleName
	FROM tblUserMaster UM

	
		inner join tblRoleMaster RM
		ON UM.UserType = RM.RoleId


	where UserID = @UserID

END
GO
/****** Object:  StoredProcedure [dbo].[stpLogin]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpLogin '12345', 'user'

CREATE procedure [dbo].[stpLogin]
(
	@Password VARCHAR(200),
	@UserCode VARCHAR(500)
)
AS
Begin
SELECT ISNULL(
(Select UserId 
from tblUserMaster  

Where UserCode=@UserCode 

AND [Password]=@Password 
--AND VesselID = @VesselID
AND IsActive= 1),0)



End

GO
/****** Object:  StoredProcedure [dbo].[stpSavePermissionRole]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec stpSavePermissionRole 1, '1,2'

CREATE PROCEDURE [dbo].[stpSavePermissionRole]
(
	@PermissionId int,
	@MappingRoles varchar(500)
)
AS
BEGIN

	BEGIN TRY
		BEGIN TRAN
		DELETE  FROM tblPermissionRoleMapping WHERE PermissionId = @PermissionId

		IF (@MappingRoles IS  NOT NULL)
		BEGIN
			INSERT INTO tblPermissionRoleMapping(PermissionId,RoleId,IsActive)
			SELECT @PermissionId,String,1 FROM [dbo].[ufn_CSVToTable](@MappingRoles,',')
		END
		
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[stpSavePermissionRole_old]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpSavePermissionRole NULL,1,1,"jgj","smmn"
CREATE PROCEDURE [dbo].[stpSavePermissionRole_old]
(
@PermissionRoleId int ,
@PermssionId int,
@RoleId int,
--@IsActive int,
@CreatedBy varchar(200),
@ModifiedBy varchar(200)
)
AS
BEGIN
IF @PermissionRoleId IS NULL
 BEGIN
	Insert into tblPermissionRoleMapping (PermissionId,RoleId,IsActive,CreatedBy)
	values (@PermssionId,@RoleId,1,@CreatedBy)
END
--ELSE
--BEGIN
--UPDATE [dbo].tblUserGroupMapping SET UserId=@UserId, ModifiedBy=@ModifiedBy ,GroupId= String FROM ufn_CSVToTable(@SelectedGroup,',')
--	WHERE UserGroupId=@UserGroupId 
--	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 

--END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveRevisionHistory]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveRevisionHistory NULL,"rytufllfullf","xyzz","ghjghkjhlj"

CREATE PROCEDURE [dbo].[stpSaveRevisionHistory]
(
@RevisionHistoryId int,
@HeaderId int,
@Chapter varchar(1000),
@Section varchar(2000),
@ChangeComment varchar(3000),
@ModificationDate datetime
)
AS
BEGIN
IF @RevisionHistoryId IS NULL
 BEGIN
	Insert into tblRevisionHistory (Chapter,Section,ChangeComment,ModificationDate,HeaderId)
	values(@Chapter, @Section, @ChangeComment, @ModificationDate,@HeaderId)
END
ELSE
BEGIN
UPDATE [dbo].tblRevisionHistory SET Chapter=@Chapter,Section=@Section,ChangeComment=@ChangeComment,ModificationDate=@ModificationDate
	WHERE RevisionHistoryId=@RevisionHistoryId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 
	--AND CreatedBy=@CreatedBy AND ModifiedBy=@ModifiedBy AND Gender=@Gender AND VesselIMO=@VesselIMO AND RankId=@RankId

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveRoleGroup]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[stpSaveRoleGroup]
(
	@RoleId int,
	@GroupId int
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRAN
			INSERT INTO tblRoleGroup(RoleId,GroupId) VALUES (@RoleId,@GroupId)
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateGroupMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  exec stpSaveUpdateGroupMaster NULL,'1st Group' ,"Created By Someone","Modified By Someone"

CREATE PROCEDURE [dbo].[stpSaveUpdateGroupMaster]
(
	@GroupId int,
@GroupName varchar(200),
--@IsActive int,
@CreatedBy varchar(200),
@ModifiedBy varchar(200)
)
AS
BEGIN
IF @GroupId IS NULL
 BEGIN
	Insert into tblGroupMaster (GroupName,IsActive,CreatedBy)
	values(@GroupName,1,@CreatedBy)
END
ELSE
BEGIN
UPDATE tblGroupMaster SET GroupName=@GroupName, ModifiedBy=@ModifiedBy
	WHERE GroupId=@GroupId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 
	--AND CreatedBy=@CreatedBy AND ModifiedBy=@ModifiedBy AND Gender=@Gender AND VesselIMO=@VesselIMO AND RankId=@RankId

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateRank]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateRank NULL,"2st Engineer","xyzz"

create PROCEDURE [dbo].[stpSaveUpdateRank]
(
@RankId int,
@RankName varchar(200),
@Description varchar(200)
)
AS
BEGIN
IF @RankId IS NULL
 BEGIN
	Insert into tblRank (RankName,Description,IsActive)
	values(@RankName,@Description,1)
END
ELSE
BEGIN
UPDATE [dbo].tblRank SET RankName=@RankName,Description=@Description
	WHERE RankId=@RankId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 
	--AND CreatedBy=@CreatedBy AND ModifiedBy=@ModifiedBy AND Gender=@Gender AND VesselIMO=@VesselIMO AND RankId=@RankId

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateRoleMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateRoleMaster NULL,'jkkjk' ,"ghhgjg","hfgjhgj"

CREATE PROCEDURE [dbo].[stpSaveUpdateRoleMaster]
(
	@RoleId int,
@RoleName varchar(200),
--@IsActive int,
@CreatedBy varchar(200),
@ModifiedBy varchar(200)
)
AS
BEGIN
IF @RoleId IS NULL
 BEGIN
	Insert into tblRoleMaster (RoleName,IsActive,CreatedBy)
	values(@RoleName,1,@CreatedBy)
END
ELSE
BEGIN
UPDATE tblRoleMaster SET RoleName=@RoleName, ModifiedBy=@ModifiedBy
	WHERE RoleId =@RoleId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 
	--AND CreatedBy=@CreatedBy AND ModifiedBy=@ModifiedBy AND Gender=@Gender AND VesselIMO=@VesselIMO AND RankId=@RankId

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateUser]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateUser NULL,"un","pw","eml","cby","mby","m","123456",1,1

CREATE PROCEDURE [dbo].[stpSaveUpdateUser]
(
	@UserId		int,
	@UserName	varchar(200),
	@Password	varchar(200),
	@Email		varchar(200),
	@CreatedBy	varchar(200),
	@ModifiedBy varchar(200),
	@Gender		varchar(50),
	@VesselIMO	varchar(50),
	@RankId		int ,
	@ShipId		int,
	@UserCode	varchar(500),
	@UserType	int,
	@RoleType	int
)
AS
BEGIN
	DECLARE @CODE VARCHAR(50)
	SELECT @CODE = dbo.udf_GenerateUserCode(@UserType,@ShipId,@RankId,@UserName)
	--Print @CODE
	DECLARE @NewUserId int
	SET @NewUserId = 0

	IF @UserId IS NULL
		 BEGIN
			BEGIN TRY
							BEGIN TRAN

							Insert into tblUserMaster (UserName,UserCode, [Password],IsActive,Email,CreatedBy,Gender,VesselIMO,RankId,ShipId,UserType,CreatedOn)
							values(@UserName,@UserCode, @Password,1,@Email,@CreatedBy,@Gender,@VesselIMO,@RankId,@ShipId,@UserType,GETDATE())

							SET @NewUserId = @@IDENTITY

							IF @RoleType = 1
							BEGIN
								INSERT INTO tblUserGroupMapping(UserId,GroupId,IsActive) VALUES ( @NewUserId,1007,1)
								INSERT INTO tblRoleGroup(RoleId,GroupId) VALUES ( @RoleType , 1007)
							END
							IF @RoleType = 2
							BEGIN
								INSERT INTO tblUserGroupMapping(UserId,GroupId,IsActive) VALUES ( @NewUserId,1008,1)
								INSERT INTO tblRoleGroup(RoleId,GroupId) VALUES ( @RoleType , 1008)
							END
							IF @RoleType = 4
							BEGIN
								INSERT INTO tblUserGroupMapping(UserId,GroupId,IsActive) VALUES ( @NewUserId,1010,1)
								INSERT INTO tblRoleGroup(RoleId,GroupId) VALUES ( @RoleType , 1010)
							END
							IF @RoleType = 5
							BEGIN
								INSERT INTO tblUserGroupMapping(UserId,GroupId,IsActive) VALUES ( @NewUserId,1011,1)
								INSERT INTO tblRoleGroup(RoleId,GroupId) VALUES ( @RoleType , 1011)
							END

							COMMIT TRAN
			END TRY
			BEGIN CATCH
					ROLLBACK TRAN
			END CATCH
		END
	ELSE
		BEGIN
			UPDATE [dbo].tblUserMaster 
					SET UserName	= @UserName
						,[Password]	= @Password
						,Email		= @Email
						,ModifiedBy	= @ModifiedBy
						,Gender		= @Gender
						,VesselIMO	= @VesselIMO
						,RankId		= @RankId
						,ShipId		= @ShipId
						,CreatedOn	= GETDATE()
				
				WHERE UserId=@UserId 
				

		END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateUserGroup]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateUserGroup NULL,1012,  '1002,1003'   ,"Created By Someone","Modified By Someone"

CREATE PROCEDURE [dbo].[stpSaveUpdateUserGroup]
(
	@UserGroupId int,
@UserId int ,
--@GroupId int ,
@SelectedGroup varchar(1000),
--@IsActive int,
@CreatedBy varchar(200),
@ModifiedBy varchar(200)

)
AS
BEGIN
IF @UserGroupId IS NULL
 BEGIN
	Insert into tblUserGroupMapping (UserId,IsActive,CreatedBy,GroupId)
	select @UserId, 1, @CreatedBy, String FROM ufn_CSVToTable(@SelectedGroup,',')
END
ELSE
BEGIN
UPDATE [dbo].tblUserGroupMapping SET UserId=@UserId, ModifiedBy=@ModifiedBy ,GroupId= String FROM ufn_CSVToTable(@SelectedGroup,',')
	WHERE UserGroupId=@UserGroupId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUserGroupMapping]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec stpSaveUserGroupMapping 1011, '1,2,1003'

CREATE PROCEDURE [dbo].[stpSaveUserGroupMapping]
(
	@UserId int,
	@MappingGroups varchar(500)
)
AS
BEGIN

	BEGIN TRY
		BEGIN TRAN
		DELETE  FROM tblUserGroupMapping WHERE UserId = @UserId

		IF (@MappingGroups IS  NOT NULL)
		BEGIN
			INSERT INTO tblUserGroupMapping(UserId,GroupId,IsActive)
			SELECT @UserId,String,1 FROM [dbo].[ufn_CSVToTable](@MappingGroups,',')
		END
		
		COMMIT TRAN
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[stpUploadPermissionUserMaster]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpUploadPermissionUserMaster '1013'

create procedure [dbo].[stpUploadPermissionUserMaster] 
( 
@UserId int
) 
AS 
BEGIN 

DECLARE @UserMasterCount int
SET @UserMasterCount =0

SELECT @UserMasterCount = UploadPermission FROM tblUserMaster 
WHERE UserId = @UserId

IF @UserMasterCount = 0
BEGIN
UPDATE tblUserMaster SET UploadPermission = 1
WHERE UserId = @UserId
 
END
ELSE 
BEGIN
UPDATE tblUserMaster SET UploadPermission = 0
WHERE UserId = @UserId
END

RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateCommonToAllManualWithActionName]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : UpdateCommonToAllManualWithActionName :  Update Common To All Manual With ActionName 
** Description  : This Procedure call from another application TCCConverter
				  By Support Team
** Created By   : BINGSHU BAIDYA
** Created On   : 27/03/2021
*******************************************************/

create procedure [dbo].[UpdateCommonToAllManualWithActionName]
(
	@ActionName varchar(500),
	@FileName varchar(max)
)
as
begin
	declare @Count int =0
	select @Count = COUNT([Name]) from tblCommonToAllManual where CONVERT(varchar(max),[Name]) = @FileName and ActionName = @ActionName
	if(@Count = 0)
		begin 
			update tblCommonToAllManual
				set ActionName = @ActionName
			where CONVERT(varchar(max),[Name]) = @FileName
		end
	
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateShipManualWithActionName]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UpdateShipManualWithActionName]
(
	@ActionName varchar(500),
	@FileName varchar(max),
	@ShipNo int
)
as
begin
	declare @Count int =0
	select @Count = COUNT([Name]) from tblShipsManual where CONVERT(varchar(max),[Name]) = @FileName and ShipNo=@ShipNo and ActionName = @ActionName
	if(@Count = 0)
		begin 
			update tblShipsManual
				set ActionName = @ActionName
			where CONVERT(varchar(max),[Name]) = @FileName and ShipNo=@ShipNo
		end
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllGroupsForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllGroupsForDrp
create procedure [dbo].[usp_GetAllGroupsForDrp]
--(
--@VesselID int
--)
AS
Begin

Select GroupId, GroupName
from tblGroupMaster
--Where VesselID=@VesselID
End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllPermissionForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllPermissionForDrp
create procedure [dbo].[usp_GetAllPermissionForDrp]
--(
--@VesselID int
--)
AS
Begin

Select PermissionId, PermissionName
from tblPermissionMaster
--Where VesselID=@VesselID
End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllRanksForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllRanksForDrp
CREATE procedure [dbo].[usp_GetAllRanksForDrp]
--(
--@VesselID int
--)
AS
Begin

Select RankId, RankName
from tblRank
--Where VesselID=@VesselID
End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllRolesForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllRolesForDrp
create procedure [dbo].[usp_GetAllRolesForDrp]
--(
--@VesselID int
--)
AS
Begin

Select RoleId, RoleName
from tblRoleMaster
--Where VesselID=@VesselID
End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllShipForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllShipForDrp
create procedure [dbo].[usp_GetAllShipForDrp]
--(
--@VesselID int
--)
AS
Begin

Select ID, ShipName
from tblShip
--Where VesselID=@VesselID
End
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllUserForDrp]    Script Date: 06-04-2021 10:22:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec usp_GetAllUserForDrp
create procedure [dbo].[usp_GetAllUserForDrp]
--(
--@VesselID int
--)
AS
Begin

Select UserId, UserName
from tblUserMaster
--Where VesselID=@VesselID
End
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Support = 0, Ship = 1, company = 2.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'tblUserMaster', @level2type=N'COLUMN',@level2name=N'UserType'
GO
