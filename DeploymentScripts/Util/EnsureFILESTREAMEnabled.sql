Use Master
Go
-- 0 = FILESTREAM disabled
-- 1 = FILESTREAM for TSQL enabled
-- 2 = FILESTREAM for TSQL and WIN32 streaming enabled
EXEC sp_configure 'filestream access level', 2
Go
RECONFIGURE
Go
