param ([string]$ip, [string]$destination)

& ".\publish-windows.ps1"

& xcopy.exe /y ".\bin\Debug\netcoreapp2.0\win8-arm\publish" "\\$ip\$destination"