[CmdletBinding()]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The name of the Windows Service")]
	[string]$Name,
	[Parameter(Mandatory=$True,Position=1,HelpMessage="The full path to the service executable")]
	[string]$BinaryPathName,
	[Parameter(Mandatory=$False,Position=2,HelpMessage="The user account to run the service as")]
	[string]$Username,
	[Parameter(Mandatory=$False,Position=3,HelpMessage="The path to the stored credential file")]
	[string]$CredentialFile
	)
	
$ErrorActionPreference = "Stop";
	
$service = Get-Service | Where-Object { $_.Name -eq "$Name" };

if (-not $service) {
	if (-not $Username) {
		Write-Host "Creating new service $Name running under default service account";
		$service = New-Service -Name "$Name" -BinaryPathName "$BinaryPathName" -StartupType Automatic;
	} else {
		Write-Host "Creating new service $Name running as $Username";
		$password = cat $CredentialFile | convertto-securestring;
		$credential = new-object -typename System.Management.Automation.PSCredential -argumentlist $Username,$password;
		if (-not $credential) {
			throw "The powershell credential could not be created.";
		}
		$service = New-Service -Name "$Name" -BinaryPathName "$BinaryPathName" -StartupType Automatic -Credential $credential;
	}
} else {
	Write-Host "Service $Name already exists... skipping create.";
}