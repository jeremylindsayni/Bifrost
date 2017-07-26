param ([string]$ip, [string]$applicationName)

$cred = Get-Credential
$session = New-PSSession -ComputerName $ip -Credential $cred

$destination = "WebApps\$applicationName"
$remoteDestination = "\\$ip\c$\$destination"
$localDestination = "c:\$destination"

Write-Host "Creating $remoteDestination"
if(!(Test-Path -Path $destination )){
    New-Item -ItemType directory -Path "\\$ip\c$\$destination"
    Write-Host "Folder $destination created"
}
else
{
  Write-Host "Folder already exists"
}

Write-Host "Adding firewall exception"
Invoke-Command -Session $session {
	New-NetFirewallRule -DisplayName "Allow traffic to $using:applicationName" -Action Allow -Direction Inbound -Protocol "TCP" -LocalPort 5000
}

Write-Host "Scheduling to run at startup"
Invoke-Command -Session $session {
	schtasks /create /tn "WebApps\$using:applicationName" /tr "$using:localDestination\$applicationName.exe" /sc onstart /ru SYSTEM
	#does not work on windows iot core
	#$action = New-ScheduledTaskAction -Execute "$using:localDestination\$using:applicationName.exe"
	#$trigger = New-ScheduledTaskTrigger -AtStartup
	#Register-ScheduledTask -Action $action -Trigger $trigger -User "SYSTEM" -TaskName "$using:applicationName" -Description "Start $using:applicationName at boot"
}