[CmdletBinding(SupportsShouldProcess=$True)]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The domain name")]
	[string]$Name,
	[ValidatePattern("\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
	[Parameter(Mandatory=$True,Position=1,HelpMessage="The IP address to resolve")]
	[string]$IPAddress)
	
$ErrorActionPreference = "Stop";

$hostsPath = "$env:windir\System32\drivers\etc\hosts";
if (-not (Test-Path -Path $hostsPath)) {
   Throw "Hosts file not found";
}

$data = Get-Content $hostsPath;
$existingEntry = $data | Where-Object { $_.Contains("$Name") };
if ($existingEntry) {
	Write-Host "Hosts entry for $IPAddress $Name already exists. Skipping...";
} else {
	$newEntry = "$IPAddress $Name";
	Write-Host "Adding the entry $newEntry";
	$data += $newEntry;
	$data | Out-File $hostsPath -Force -Encoding ASCII;
}
