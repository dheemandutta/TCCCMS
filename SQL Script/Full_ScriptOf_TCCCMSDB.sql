USE [TCCCMSDB]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_CSVToTable]    Script Date: 31-12-2020 08:28:16 PM ******/
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
/****** Object:  Table [dbo].[tblGroupMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPermissionMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblPermissionRoleMapping]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPermissionRoleMapping](
	[PermissionRoleId] [int] IDENTITY(1,1) NOT NULL,
	[PermssionId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRank]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRank](
	[RankId] [int] IDENTITY(1,1) NOT NULL,
	[RankName] [varchar](200) NOT NULL,
	[Description] [varchar](200) NULL,
	[IsActive] [int] NULL,
 CONSTRAINT [PK_tblRank] PRIMARY KEY CLUSTERED 
(
	[RankId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRoleMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRoleMaster](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](200) NOT NULL,
	[IsActive] [int] NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [nchar](10) NULL,
 CONSTRAINT [PK_tblRoleMaster] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRolGroupMapping]    Script Date: 31-12-2020 08:28:16 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserGroupMapping]    Script Date: 31-12-2020 08:28:16 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserMaster](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](200) NOT NULL,
	[Password] [varchar](200) NULL,
	[CreatedOn] [varchar](200) NULL,
	[IsActive] [int] NULL,
	[Email] [varchar](200) NULL,
	[CreatedBy] [varchar](200) NULL,
	[ModifiedBy] [varchar](200) NULL,
	[Gender] [varchar](50) NULL,
	[VesselIMO] [varchar](50) NULL,
	[RankId] [int] NOT NULL,
 CONSTRAINT [PK_tblUserMaster] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblGroupMaster] ON 

INSERT [dbo].[tblGroupMaster] ([GroupId], [GroupName], [IsActive], [CreatedBy], [ModifiedBy]) VALUES (1, N'fdd', 1, N'hfgh', N'gfhfgh')
SET IDENTITY_INSERT [dbo].[tblGroupMaster] OFF
GO
SET IDENTITY_INSERT [dbo].[tblRank] ON 

INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (1, N'Master', N'Master', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (2, N'Chief Off', N'Chief Off', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (3, N'2nd Off', N'2nd Off', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (4, N'Chief Engineer', N'Chief Engineer', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (5, N'1st Engineer', N'1st Engineer', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (6, N'ETO', N'ETO', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (7, N'3rd Engineer', N'3rd Engineer', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (8, N'Bosun / Crane', N'Bosun / Crane', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (9, N'Crane Operator', N'Crane Operator', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (10, N'AB', N'AB', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (11, N'Fitter', N'Fitter', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (12, N'Oiler', N'Oiler', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (13, N'Cook', N'Cook', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (14, N'Steward', N'Steward', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (15, N'O/S', N'O/S', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (16, N'2nd Cook', N'2nd Cook', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (17, N'Third Deck Officer', N'Third Deck Officer', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (18, N'SDPO', N'SDPO', 1)
INSERT [dbo].[tblRank] ([RankId], [RankName], [Description], [IsActive]) VALUES (19, N'DPO', N'DPO', 1)
SET IDENTITY_INSERT [dbo].[tblRank] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUserMaster] ON 

INSERT [dbo].[tblUserMaster] ([UserId], [UserName], [Password], [CreatedOn], [IsActive], [Email], [CreatedBy], [ModifiedBy], [Gender], [VesselIMO], [RankId]) VALUES (3, N'gdfg', N'fdgfd', N'fdgfdg', 1, N'hgf', N'gfhhgfh', N'gfhgfh', N'gfhfg', N'gfhfg', 1)
SET IDENTITY_INSERT [dbo].[tblUserMaster] OFF
GO
ALTER TABLE [dbo].[tblPermissionRoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblPermissionRoleMapping_tblPermissionMaster] FOREIGN KEY([PermssionId])
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
ALTER TABLE [dbo].[tblUserMaster]  WITH CHECK ADD  CONSTRAINT [FK_tblUserMaster_tblRank] FOREIGN KEY([RankId])
REFERENCES [dbo].[tblRank] ([RankId])
GO
ALTER TABLE [dbo].[tblUserMaster] CHECK CONSTRAINT [FK_tblUserMaster_tblRank]
GO
/****** Object:  StoredProcedure [dbo].[stpDeleteGroupMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
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
/****** Object:  StoredProcedure [dbo].[stpDeletePermissionRole]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[stpDeletePermissionRole] 
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
/****** Object:  StoredProcedure [dbo].[stpDeleteRoleMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[stpDeleteRoleMaster] 
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
/****** Object:  StoredProcedure [dbo].[stpDeleteUserGroup]    Script Date: 31-12-2020 08:28:16 PM ******/
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
/****** Object:  StoredProcedure [dbo].[stpDeleteUserMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec stpDeleteUserMaster '3'

CREATE procedure [dbo].[stpDeleteUserMaster] 
( 
@UserId int
) 
AS 
BEGIN 

--DECLARE @UserMasterCount int
--SET @UserMasterCount =0

--SELECT @UserMasterCount = COUNT(*) FROM tblUserMaster 
--WHERE UserId = @UserId

--IF @UserMasterCount = 0
BEGIN
UPDATE tblUserMaster SET IsActive = 0
WHERE UserId = @UserId
 
END


RETURN  
END
GO
/****** Object:  StoredProcedure [dbo].[stpGetAllGroupMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetAllGroupMaster
create PROCEDURE [dbo].[stpGetAllGroupMaster]
--(
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
--@RankId int  
--)
AS
Begin
Select  GroupName,CreatedBy,ModifiedBy

FROM dbo.tblGroupMaster

--WHERE UM.UserId= @UserId 
--AND UM.IsActive=1

where IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUser]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetAllUser
CREATE PROCEDURE [dbo].[stpGetAllUser]
--(
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
--@RankId int  
--)
AS
Begin
Select  UserName,CreatedOn,Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

--WHERE UM.UserId= @UserId 
--AND UM.IsActive=1

where UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetAllUserGroupByUserID]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--   exec stpGetAllUserGroupByUserID  3
create PROCEDURE [dbo].[stpGetAllUserGroupByUserID]
(
@UserId int--,
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
Select  UM.UserName,GM.GroupName,UGM.CreatedBy,UGM.ModifiedBy

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
/****** Object:  StoredProcedure [dbo].[stpGetUserByEmailId]    Script Date: 31-12-2020 08:28:16 PM ******/
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
Select  UserName,CreatedOn,Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.Email= @Email 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByIMO]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByIMO "gfhfg"
create PROCEDURE [dbo].[stpGetUserByIMO]
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
Select  UserName,CreatedOn,Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.VesselIMO= @VesselIMO 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByRank]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByRank 1
create PROCEDURE [dbo].[stpGetUserByRank]
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
Select  UserName,CreatedOn,Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.RankId= @RankId 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpGetUserByUserId]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--   exec stpGetUserByUserId 3
CREATE PROCEDURE [dbo].[stpGetUserByUserId]
(
@UserId int--,
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
Select  UserName,CreatedOn,Email,CreatedBy,ModifiedBy,Gender,VesselIMO,R.RankName

FROM dbo.tblUserMaster UM
LEFT OUTER JOIN tblRank R
ON UM.RankId = R.RankId

WHERE UM.UserId= @UserId 
AND UM.IsActive=1
End



GO
/****** Object:  StoredProcedure [dbo].[stpSavePermissionRole]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[stpSavePermissionRole]
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
	Insert into tblPermissionRoleMapping (PermssionId,RoleId,IsActive,CreatedBy)
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
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateGroupMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--  exec stpSaveUpdateGroupMaster NULL,'A Das' , 3, '17-Dec-2019', 'Kolkata'

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
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateRoleMaster]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateRoleMaster NULL,'A Das' , 3, '17-Dec-2019', 'Kolkata'

create PROCEDURE [dbo].[stpSaveUpdateRoleMaster]
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
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateUser]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateUser NULL,'A Das' , 3, '17-Dec-2019', 'Kolkata'

CREATE PROCEDURE [dbo].[stpSaveUpdateUser]
(
	@UserId int,
@UserName varchar(200),
@Password varchar(200),
@CreatedOn varchar(200),
--@IsActive int,
@Email varchar(200),
@CreatedBy varchar(200),
@ModifiedBy varchar(200),
@Gender varchar(50),
@VesselIMO varchar(50),
@RankId int 
)
AS
BEGIN
IF @UserId IS NULL
 BEGIN
	Insert into tblUserMaster (UserName,[Password],CreatedOn,IsActive,Email,CreatedBy,Gender,VesselIMO,RankId)
	values(@UserName,@Password,@CreatedOn,1,@Email,@CreatedBy,@Gender,@VesselIMO,@RankId)
END
ELSE
BEGIN
UPDATE [dbo].tblUserMaster SET UserName=@UserName,[Password]=@Password,CreatedOn=@CreatedOn,Email=@Email,
                               ModifiedBy=@ModifiedBy,Gender=@Gender,VesselIMO=@VesselIMO,RankId=@RankId 
	WHERE UserId=@UserId 
	--AND UserName=@UserName AND [Password]=@Password AND CreatedOn=@CreatedOn AND IsActive=@IsActive AND Email=@Email 
	--AND CreatedBy=@CreatedBy AND ModifiedBy=@ModifiedBy AND Gender=@Gender AND VesselIMO=@VesselIMO AND RankId=@RankId

END
END
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateUserGroup]    Script Date: 31-12-2020 08:28:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateUserGroup NULL,'A Das' , 3, '17-Dec-2019', 'Kolkata'

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
