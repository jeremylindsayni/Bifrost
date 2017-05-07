dotnet restore .
dotnet build . --configuration Release --runtime ubuntu.16.04-arm
dotnet publish . -r ubuntu.16.04-arm --configuration Release

& pscp.exe -r .\bin\Release\netcoreapp2.0\ubuntu.16.04-arm\publish\* ubuntu@192.168.1.110:/home/ubuntu/Cartographer

#rsync -avz -e ssh .\bin\Release\netcoreapp2.0\ubuntu.16.04-arm\publish\* ubuntu@192.168.1.110:/home/ubuntu/Cartographer

& plink.exe -v -ssh ubuntu@192.168.1.110 chmod u+x,o+x /home/ubuntu/Cartographer/Cartographer