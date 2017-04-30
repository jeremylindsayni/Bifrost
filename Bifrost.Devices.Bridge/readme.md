# Bifröst - Windows Device Bridge

## What is the Bifröst Windows Device Bridge?

Bifröst is a project for developers who want to write .NET Core 2 applications that use IoT devices, and want to target Ubuntu and Windows with the same code.  

Presently .NET Core 2 doesn't have the capability of accessing Raspberry Pi 3's hardware capabilities. The Windows Device Bridge allows developers to use a .NET Core 2 application access certain hardware capabilities of the Raspberry Pi 3 IoT Device.

## How do I install the Bifröst Windows Device Bridge?

### With Visual Studio 2015/2017

It's probably easiest to pull the code for this repository, and deploy the application through your instance of Visual Studio.  
  
Microsoft provide comprehensive instructions here:  
https://developer.microsoft.com/en-us/windows/iot/docs/appdeployment

### With the command line using MSBuild and Nuget

### Install Nuget 4.1
Make sure that Nuget 4.1 is installed - you can get it from:  
https://dist.nuget.org/index.html

You can test if Nuget is installed correctly by opening a command prompt and typing nuget.  
It should return the text: **NuGet Version: 4.1.0.2450**

If the text returned is "_The system cannot find the path specified._" then check if the directory that holds nuget.exe is on your path.

### Build the solution using MSBuild
From a command prompt, browse to the directory that holds the **Bifrost.Devices.Bridge.sln** file, and run:  
```nuget restore```

This will restore packages for the solution.

Next, from the same directory, run the command 

```
MSBuild .\Bifrost.Devices.Bridge\Bifrost.Devices.Bridge.csproj /p:AppxBundle=Always /p:AppxBundlePlatforms="x86|arm" /p:Configuration=Release  
```

### Deploy using the Windows Device Portal
After MSBuild completes, there will be a new folder in the solution called AppPackage, containing:  
  
#### App bundle
~\Bifrost.Devices.Bridge\AppPackages\Bifrost.Devices.Bridge_0.1.0.0_Test\Bifrost.Devices.Bridge_0.1.0.0_x86_arm.appxbundle  
  
#### Dependencies
~\Bifrost.Devices.Bridge\AppPackages\Bifrost.Devices.Bridge_0.1.0.0_Test\arm\Microsoft.NET.Native.Framework.1.6.appx
~\Bifrost.Devices.Bridge\AppPackages\Bifrost.Devices.Bridge_0.1.0.0_Test\arm\Microsoft.NET.Native.Runtime.1.6.appx  
  
Now install the app bundle and dependencies using the Windows Device Portal for your device - more details are available here:  
https://developer.microsoft.com/en-us/windows/iot/docs/appinstaller

Basically you just need to upload the app bundle and dependencies to your device through this page, and then just click on "Go" to deploy.  

Once it has been successfully deployed the UWP bridge to your device, you'll see it appear in your list of apps, similarly to the image below. If it isn't running, select the option to start the UWP app from the 'Actions' dropdown list on the left of the list of apps.  

You may wish to set this app to run at startup by selecting the radio button to do this.

![Image of Windows Device Portal](https://jeremylindsayni.files.wordpress.com/2017/04/screenshot-1493500867.png)
