
[CmdletBinding()]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The names of the Windows Services to start")]
	[string[]]$Names,
	[Parameter(Mandatory=$False,Position=1,HelpMessage="The number of seconds to wait before checking the Services")]
	[int]$SecondsToWait = 30)

$ErrorActionPreference = "Stop";

Write-Host "Starting these services: $Names";
$Names | Start-Service;

Start-Sleep -s $SecondsToWait;

Write-Host "Checking state of these services: $Names";
$Names | Get-Service | Where-Object { $_.Status -ne "Running" } | % { $notRunning += " " + $_.Name };
if (-not $notRunning) {
	Write-Host "All services have started and are still running.";
	return;
} else {
	Throw "The following servies have started and then stopped: " + $Names;
}
