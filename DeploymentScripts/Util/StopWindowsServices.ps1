
[CmdletBinding()]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The names of the Windows Services to stop")]
	[string[]]$Names,
	[Parameter(Mandatory=$False,Position=1,HelpMessage="The number of seconds to wait before checking the Services")]
	[int]$SecondsToWait = 10)

$ErrorActionPreference = "Stop";

$existingServices = Get-Service | Where-Object { $Names -contains $_.Name };
if (-not $existingServices) {
	Write-Host "There are no installed services to stop.";
	return;
}

Write-Host "Stopping these services:";
$existingServices | % { Write-Host $_.Name };
$existingServices | Stop-Service;

$interval = $SecondsToWait;
if ($interval -gt 5) { $interval = 5; }

while ($SecondsToWait -gt 0) {

	$existingServices | Get-Service | Where-Object { $_.Status -ne "Stopped" } | % { $stillRunning += " " + $_.Name };
	if (-not $stillRunning) {
		Write-Host "All services have stopped.";
		return;
	}

	$SecondsWaited = $interval;
	Write-Host "Waiting for $SecondsWaited seconds...";
	Start-Sleep -s $interval;
	$SecondsToWait -= $interval;
	$SecondsWaited += $interval;
}

$existingServices | Get-Service | Where-Object { $_.Status -ne "Stopped" } | % { $stillRunning += " " + $_.Name };
if (-not $stillRunning) {
	Write-Host "All services have stopped.";
	return;
} else {
	Throw "The following services are still running: " + $stillRunning;
}
