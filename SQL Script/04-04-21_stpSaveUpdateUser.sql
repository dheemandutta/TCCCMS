USE [TCCCMSDB]
GO
/****** Object:  StoredProcedure [dbo].[stpSaveUpdateUser]    Script Date: 4/4/2021 3:40:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--  exec stpSaveUpdateUser NULL,"un","pw","eml","cby","mby","m","123456",1,1

ALTER PROCEDURE [dbo].[stpSaveUpdateUser]
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
							values(@UserName,@CODE, @Password,1,@Email,@CreatedBy,@Gender,@VesselIMO,@RankId,@ShipId,@UserType,GETDATE())

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
