CREATE TABLE [dbo].[Role] (
	[RoleId] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Role] PRIMARY KEY,
	[RoleName] NVARCHAR(50) NOT NULL,
	[IsAdministrator] BIT NOT NULL CONSTRAINT [DEF_Role_IsAdminstrator] DEFAULT (0)
)
GO

SET IDENTITY_INSERT [Role] ON
INSERT INTO [dbo].[Role] ([RoleId], [RoleName], [IsAdministrator])
VALUES
	(1, 'Users', 0),
	(2, 'Administrators', 1)
SET IDENTITY_INSERT [Role] OFF
GO

CREATE TABLE [dbo].[User] (
	[UserId] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_User] PRIMARY KEY,
	[UserName] NVARCHAR(56) NOT NULL,
	[EmailAddress] NVARCHAR(255) NULL,
	[SecurityQuestion] NVARCHAR(255) NULL,
	[SecurityAnswer] NVARCHAR(255) NULL,
	[ProfileImageUrl] NVARCHAR(500) NULL,
	[DateJoined] Date NULL,	
	[IPAddres] NVARCHAR(255) NULL,	
	[ConfirmationToken] NVARCHAR(128) NULL,
	[IsConfirmed] BIT NOT NULL CONSTRAINT [DEF_User_IsConfirmed] DEFAULT (0),
	[LastPasswordFailureDate] DATETIME NULL,
	[PasswordFailuresSinceLastSuccess] INT NOT NULL CONSTRAINT [DEF_User_PasswordFailuresSinceLastSuccess] DEFAULT (0),
	[Password] NVARCHAR(128) NOT NULL,
	[PasswordChangedDate] DATETIME NULL,
	[PasswordSalt] NVARCHAR(128) NOT NULL CONSTRAINT [DEF_User_PasswordSalt] DEFAULT (''),
	[PasswordVerificationToken] NVARCHAR(129) NULL,
	[PasswordVerificationTokenExpirationDate] DATETIME NULL,	
	[RoleId] INT NOT NULL CONSTRAINT [FK_User_RoleId] FOREIGN KEY REFERENCES [Role]([RoleId])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UQ_User_UserName] ON [User]([UserName]);
CREATE UNIQUE NONCLUSTERED INDEX [UQ_User_EmailAddress] ON [User]([EmailAddress]);

-- PASSWORD is 'PASSWORD'
SET IDENTITY_INSERT [User] ON
INSERT INTO [User] ([UserId], [UserName], [EmailAddress], [DateJoined], [Password],[PasswordSalt],[IsConfirmed], [RoleId])
VALUES (1, 'user', 'user@system.com', getdate(), 'gwB/aRxk3sl7ZUMezrvW0G7wXj8=','c44UBKmxB6a2RJquH47DmxwE92N2bbEq',1,1)
INSERT INTO [User] ([UserId], [UserName], [EmailAddress], [DateJoined], [Password],[PasswordSalt],[IsConfirmed], [RoleId])
VALUES (2, 'admin', 'admin@system.com', getdate(), 'gwB/aRxk3sl7ZUMezrvW0G7wXj8=','c44UBKmxB6a2RJquH47DmxwE92N2bbEq', 1, 2)
SET IDENTITY_INSERT [User] OFF
GO

CREATE TABLE [dbo].[UserRole] (
	[Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT [PK_UserRole] PRIMARY KEY,
	[UserId] INT NOT NULL CONSTRAINT [FK_UserRole_UserID] FOREIGN KEY REFERENCES [User]([UserId]),
	[RoleId] INT NOT NULL CONSTRAINT [FK_UserRole_RoleId] FOREIGN KEY REFERENCES [Role]([RoleId])
)
GO

SET IDENTITY_INSERT [UserRole] ON
INSERT INTO [UserRole] ([Id], [UserId], [RoleId])
VALUES 
	(1, 1, 1),
	(2, 2, 1),
	(3, 2, 2)
SET IDENTITY_INSERT [UserRole] OFF
GO

