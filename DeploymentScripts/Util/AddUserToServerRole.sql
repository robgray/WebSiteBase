USE MASTER
GO

IF NOT EXISTS(SELECT name FROM sys.server_principals WHERE name = N'$(UserName)')
BEGIN
	PRINT 'Creating server login ' + N'$(UserName)'
    CREATE LOGIN [$(UserName)] FROM WINDOWS
END
ELSE
BEGIN
	PRINT 'Server login already exists for ' + N'$(UserName)'
END

IF LOWER(N'$(ServerRole)') <> 'public'
BEGIN
	PRINT 'Adding ' + N'$(UserName)' + ' to ' + N'$(ServerRole)'
	EXEC sp_addsrvrolemember N'$(UserName)', N'$(ServerRole)'
END
ELSE
BEGIN
	PRINT N'$(UserName)' + ' is already a member of the role ' + N'$(ServerRole)'
END

