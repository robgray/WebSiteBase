USE MASTER
GO

IF DB_ID(N'$(DatabaseName)') IS NULL
BEGIN
	PRINT 'Restoring database ' + N'$(DatabaseName)'
	DECLARE @dbBackupLoc NVARCHAR(MAX)
	SET @dbBackupLoc =  N'$(BackupLocation)' + N'$(BackupFile)'
	RESTORE DATABASE [$(DatabaseName)]
	FROM DISK =  @dbBackupLoc
	WITH MOVE N'$(OriginalDataFile)' TO N'$(DataFile)',
	MOVE N'$(OriginalLogFile)' TO N'$(LogFile)',
	REPLACE
END