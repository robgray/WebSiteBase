[CmdletBinding()]
param(
	[Parameter(Mandatory=$True,Position=0,HelpMessage="The name of the queue to create")]
	[string]$Name)
	
$ErrorActionPreference = "Stop";

$queuename = ".\private$\" + $Name

Add-Type -AssemblyName System.Messaging

if ([System.Messaging.MessageQueue]::Exists($queuename) -eq 1) {
	Write-Host "Queue called $queuename already exists. Skipping create..."
} else {
	Write-Host "Attempting to create Private Transactional queue called $queuename..."
	$q = [System.Messaging.MessageQueue]::Create($queuename, $true) 

	if($q -eq $null)
	{
		Write-Host "ERROR: The MessageQueue.Create() method did not return a MessageQueue instance."
		exit
	}
}

$q = new-object System.Messaging.MessageQueue($queuename)

Write-Host "Granting FullControl permissions to Local Administrators"
$q.SetPermissions("Administrators", [System.Messaging.MessageQueueAccessRights]::FullControl, [System.Messaging.AccessControlEntryType]::Allow) 

Write-Host "Granting FullControl permissions to user Everyone"
$q.SetPermissions("Everyone", [System.Messaging.MessageQueueAccessRights]::FullControl, [System.Messaging.AccessControlEntryType]::Allow) 

Write-Host "Granting FullControl permissions to user ANONYMOUS LOGON"
$q.SetPermissions("ANONYMOUS LOGON", [System.Messaging.MessageQueueAccessRights]::FullControl, [System.Messaging.AccessControlEntryType]::Allow) 

$q.label = $queuename
	
