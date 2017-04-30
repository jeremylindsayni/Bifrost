param ([string]$ip, [string]$applicationName)


$cred = Get-Credential
$session = New-PSSession -ComputerName $ip -Credential $cred

$destination = "WebApps\$applicationName"
$remoteDestination = "\\$ip\c$\$destination"
$localDestination = "c:\$destination"

Write-Host "Uninstalling from $remoteDestination"
if(!(Test-Path -Path $remoteDestination )){
    Write-Host "Folder $remoteDestination does not exist"
}
else
{
	Remove-Item -Path "$remoteDestination"
	Write-Host "Folder $remoteDestination removed"
}

Write-Host "Removing firewall rule"
Invoke-Command -Session $session {
	Remove-NetFirewallRule -DisplayName "Allow traffic to $using:applicationName"
}

Write-Host "Removing startup task"
Invoke-Command -Session $session {
	schtasks /delete /tn "WebApps\$using:applicationName" /F
	#Unregister-ScheduledTask -TaskName "$using:applicationName"
}