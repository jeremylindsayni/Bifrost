#param ([string]$ip, [string]$destination, [string]$username)

dotnet clean .
dotnet restore .
dotnet build .

dotnet publish . -r ubuntu.16.04-arm

& pscp.exe -r .\bin\Debug\netcoreapp2.0\ubuntu.16.04-arm\publish\* ubuntu@192.168.1.110:/home/ubuntu/tmp102

& plink.exe -v -ssh ubuntu@192.168.1.110 chmod u+x,o+x /home/ubuntu/tmp102/Tmp102