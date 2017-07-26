param ([string]$ip, [string]$applicationName)

& ".\publish-windows.ps1"


$destination = "WebApps\$applicationName"
$remoteDestination = "\\$ip\c$\$destination"
$localDestination = "c:\$destination"

& robocopy.exe /MIR ".\bin\Debug\netcoreapp2.0\win8-arm\publish" "$remoteDestination"
