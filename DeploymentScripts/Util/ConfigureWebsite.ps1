[CmdletBinding()]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The name of the WebSite, AppPool and URL for the Binding")]
	[string]$Name,
	[Parameter(Mandatory=$True,Position=1,HelpMessage="The full physical path of the Web Site")]
	[string]$PhysicalPath,
	[Parameter(Mandatory=$False,Position=2,HelpMessage="The username to run the AppPool as")]
	[string]$Username,
	[Parameter(Mandatory=$False,Position=3,HelpMessage="The full path to the file containing the stored credential")]
	[string]$CredentialFile,
	[Parameter(HelpMessage="Use the reset switch to completely remove and re-add the Web Site")]
	[Switch]$Reset)

$ErrorActionPreference = "Stop";

Import-Module WebAdministration

$existingSites = @(gci 'IIS:\Sites');
if ($existingSites.length -eq 0) {
	Throw "There needs to be at least one existing site on this server for this script to work since IIS uses a sequential id generation algorithm by default. See http://stackoverflow.com/questions/8346255/new-item-iis-sites-sitename-index-was-outside-the-bounds-of-the-array";
}

$ipAddress = [System.Net.Dns]::GetHostAddresses($Name) | Select-Object IPAddressToString -expandproperty IPAddressToString;
$sslCertificate = gci 'CERT:\LocalMachine\My' | Where-Object { $_.Subject -ilike "*$Name*" };

if (-not $sslCertificate) {
	Throw "Cannot find SSL certificate for $Name, cannot configure HTTPS... Installed Certificates are:";
	gci CERT:\LocalMachine\My | ft;
}

dir 'IIS:\AppPools' | Where-Object { $_.Name -eq $Name } | % { $appPool = $_ };
dir 'IIS:\Sites' | Where-Object { $_.Name -eq $Name } | % { $webSite = $_ };
dir 'IIS:\SslBindings' | Where-Object { $_.IPAddress -eq $ipAddress -and $_.Port -eq 443 } | % { $sslBinding = $_ };

if ($Reset) {
	if ($sslBinding) {
		Write-Host "Deleting SslBinding " + $sslBinding;
		Remove-Item -Path "IIS:\SslBindings\$ipAddress!443";
		$sslBinding = $null;
	}

	if ($webSite) {
		Write-Host "Deleting WebSite $Name";
		Remove-Website "$Name";
		$webSite = $null;
	}

	if ($appPool) {
		Write-Host "Deleting AppPool $Name";
		Remove-WebAppPool "$Name";
		$appPool = $null;
	}
}

Write-Host "Configuring Website: $Name using path $PhysicalPath"

if (-not $appPool) {
	Write-Host "IIS: Creating AppPool $Name";
	$appPool = New-Item "IIS:\AppPools\$Name";
}
else {
	Write-Host "IIS: AppPool $Name already exists..."
}

Write-Host "Configuring AppPool $Name";
Set-ItemProperty "IIS:\AppPools\$Name" ManagedRuntimeVersion "v4.0";
Set-ItemProperty "IIS:\AppPools\$Name" ManagedPipelineMode 0; # Integrated
Set-ItemProperty "IIS:\AppPools\$Name" Enable32BitAppOnWin64 $true;

if ($Username) {
	Write-Host "Configuring AppPool to run as $Username using credential stored in file $credentialFile";
	$securePassword = cat $CredentialFile | convertto-securestring;
	$credential = new-object -typename System.Management.Automation.PSCredential -argumentlist $Username,$securePassword;
	$password = $credential.GetNetworkCredential().Password;
	Set-ItemProperty "IIS:\AppPools\$Name" -Name processModel -Value @{UserName=$Username;password=$password;identitytype=3}; # IdentityType=SpecificUser
}

if (-not $webSite) {
	Write-Host "IIS: Creating WebSite $Name";
	$webSite = New-Item "IIS:\Sites\$Name" -physicalPath "$PhysicalPath" -bindings @{protocol="http";bindingInformation=":80:$Name"};
	Set-WebBinding -Name "$Name" -HostHeader "$Name" -Port 80 -PropertyName IPAddress -Value $ipAddress;
	Set-ItemProperty "IIS:\Sites\$Name" ApplicationPool "$Name"
	Write-Host "Adding HTTPS Binding for $Name on $ipAddress using $sslCertificate";
	New-WebBinding -Name "$Name" -IPAddress $ipAddress -HostHeader "$Name" -Port 443 -Protocol https;
	New-Item -Path "IIS:\SslBindings\$ipAddress!443" -Value $sslCertificate;
} else {
	Write-Host "IIS: WebSite $Name already exists... skipping create."
}

