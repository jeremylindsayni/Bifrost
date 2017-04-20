param ([string]$ip, [string]$destination, [string]$username)

& ".\publish-ubuntu.ps1"

& pscp.exe -r .\bin\Debug\netcoreapp2.0\ubuntu.16.04-arm\publish\* ${username}@${ip}:${destination}

& plink.exe -v -ssh ${username}@${ip} chmod u+x,o+x ${destination}/GpioSwitcher