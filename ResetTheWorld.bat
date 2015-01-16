@echo off

echo *
echo * Test for Elevated Command Prompt
echo * 
set tst="%windir%\$del_me$"
(type nul>%tst%) 2>nul && (del %tst% & set elev=t) || (set elev=)
IF NOT DEFINED elev GOTO NOTELEVATED

echo * 
echo * Install Windows Features
echo * 
SET commonPackages=IIS-WebServerRole;IIS-WebServer;IIS-CommonHttpFeatures;IIS-StaticContent;IIS-DefaultDocument;IIS-HttpErrors;IIS-ASPNET;IIS-NetFxExtensibility;IIS-ISAPIExtensions;IIS-ISAPIFilter;IIS-HttpLogging;IIS-LoggingLibraries;IIS-RequestMonitor;IIS-HttpTracing;IIS-Security;IIS-BasicAuthentication;IIS-WindowsAuthentication;IIS-RequestFiltering;IIS-Performance;IIS-HttpCompressionStatic;IIS-ManagementConsole;IIS-ManagementService;WCF-HTTP-Activation;WAS-WindowsActivationService;WAS-ProcessModel;WAS-NetFxEnvironment;WAS-ConfigurationAPI;

REM Check Windows Version
ver | findstr /i "6\.1\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Win7
ver | findstr /i "6\.2\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Win8
goto warn_and_exit

:ver_Win8
echo OS Version: Windows 8
start /w pkgmgr /iu:%commonPackages%IIS-ASPNET45;IIS-NetFxExtensibility45;WCF-Services45;WCF-HTTP-Activation45;WCF-TCP-Activation45;WCF-Pipe-Activation45;WCF-MSMQ-Activation45;WCF-TCP-PortSharing45;NetFx4-AdvSrvs;NetFx4Extended-ASPNET45;
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
goto INSTALLMSMQ

:ver_Win7
echo OS Version: Windows 7
start /w pkgmgr /iu:%commonPackages%
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
c:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -i
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
goto INSTALLMSMQ

:warn_and_exit
ECHO Must be Windows 7 or 8
GOTO ERROR

:INSTALLMSMQ
rem echo **
rem echo ** Ensure MSMQ is installed 
rem echo ** 
rem start /w pkgmgr /iu:MSMQ-Container;MSMQ-Server

REM SET nugetFileFolder=\\san1\data\nuget
REM SET backupFileFolder=\\san1\data\Projects\webbase
SET backupFileName=webbase-db.bak
SET backupFilePath=%backupFileFolder%\%backupFileName%
SET utilScripts=%~dp0DeploymentScripts\Util\
SET copyBackupFileTo=%~dp0DatabaseBackup\
SET backupFileToRestore=%copyBackupFileTo%%backupFileName%
SET targetDatabaseServer=(local)
set databaseEngineProjectFile=%~dp0Database\Database.csproj
SET utilScripts=%~dp0DeploymentScripts\Util\
SET appcmd=%windir%\system32\inetsrv\appcmd.exe
SET siteName=webbase
SET appPool=webbase
SET webbaseDomainName=webbase
SET sqlPath=C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER

endlocal
echo ###########################
echo Running as %username%
echo ###########################
net user "%username%" /domain


echo **
echo ** Deleting *.csproj.user files
echo **
del *.csproj.user /S /F/ Q

echo **
echo ** Deleting obj and bin folders
echo **
for /d /r . %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"

REM INSERT SITE CONFIGURATION HERE
echo **
echo ** Configure IIS and APPPools
echo **
iisreset /restart
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set config /section:windowsAuthentication /enabled:true
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% delete site /site.name:%siteName%
%appcmd% delete apppool /apppool.name:%appPool%
%appcmd% add apppool /name:%appPool%
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% add site /site.name:%siteName%
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% add app /site.name:%siteName% /path:"/" /physicalPath:"%~dp0Site"
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
rem %appcmd% set site /site.name:%siteName% /+bindings.[protocol='http',bindingInformation='*:9500:localhost']
rem IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set site /site.name:%siteName% /+bindings.[protocol='http',bindingInformation='*:80:%webbaseDomainName%']
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set app "%siteName%/" /applicationPool:%appPool%
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set apppool /apppool.name:%appPool% /managedRuntimeVersion:v4.0
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set apppool /apppool.name:%appPool% /enable32BitAppOnWin64:True
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
%appcmd% set apppool /apppool.name:%appPool% /managedPipelineMode:Integrated
IF %ERRORLEVEL% NEQ 0 GOTO ERROR


echo **
echo ** Configuring Hosts file
echo **
powershell.exe -ExecutionPolicy Bypass -InputFormat none -Command "%utilScripts%AddHostsFileEntry.ps1" -Name "%webbaseDomainName%" -IPAddress "127.0.0.1"
IF %ERRORLEVEL% NEQ 0 GOTO ERROR

echo **
echo ** Setting IIS Website permissions to folders
echo **
icacls "C:\Windows\Temp" /grant:r "BUILTIN\IIS_IUSRS":(OI)(CI)(S,RD)
IF %ERRORLEVEL% NEQ 0 GOTO ERROR
icacls "%~dp0Client" /grant:r "IIS APPPOOL\%appPool%":(OI)(CI)(RX)
IF %ERRORLEVEL% NEQ 0 GOTO ERROR

echo **
echo ** Building the Database Upgrade Utility...
echo **
rem net use %nugetFileFolder%
msbuild %databaseEngineProjectFile% /T:Build /P:Configuration=Debug,Platform="AnyCPU" /verbosity:minimal
IF %ERRORLEVEL% NEQ 0 GOTO ERROR

echo **
echo ** Copy latest database backup if a newer one is found
echo **
net use %backupFileFolder%
robocopy %backupFileFolder% %copyBackupFileTo% %backupFileName%

echo **
echo ** Restoring database...
echo **
SQLCMD /b -E -S %targetDatabaseServer% -i "%utilScripts%RestoreDatabase.sql" -v DatabaseName = "webbase" BackupLocation = "%copyBackupFileTo%" BackupFile = "%backupFileName%" DataFile = "%sqlPath%\MSSQL\DATA\webbase.mdf" LogFile = "%sqlPath%\MSSQL\DATA\webbase_Log.ldf" OriginalDataFile = "webbase" OriginalLogFile = "webbase_Log"
IF %ERRORLEVEL% EQU 0 GOTO UPDATEDB

:CREATEDB
echo **
echo ** Creating database...
echo **
%~dp0Database\bin\Debug\Database.exe --drop=true --quiet=true
IF %ERRORLEVEL% NEQ 0 GOTO ERROR

:UPDATEDB
echo **
echo ** Updating database...
echo **
%~dp0Database\bin\Debug\Database.exe --drop=false --quiet=true
IF %ERRORLEVEL% NEQ 0 GOTO ERROR

echo **
echo ** Adding webbase IISAppPool user to webbase database...
echo **
SQLCMD /b -E -S %targetDatabaseServer% -i "%utilScripts%AddUserToDatabase.sql" -v DatabaseName = "webbase" UserName = "IIS APPPOOL\%appPool%" DatabaseRole = "db_owner"
IF %ERRORLEVEL% NEQ 0 GOTO ERROR


echo -------------------------------
echo Finished ResetTheWorld on:
date /T
time /T
echo -------------------------------

GOTO END


:ERROR 
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
echo !!! Stopping Due to error !!!
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

GOTO END 

:NOTELEVATED

echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
echo !!! ERROR: Run from elevated command prompt  !!!
echo !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

GOTO END
:END