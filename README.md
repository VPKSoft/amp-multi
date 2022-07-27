# amp#
A music player for common audio formats with a simple and intuitive GUI.

[![Linux, Windows and macOS .NET Core](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml)

#### Install
**NOTE**: This is currently beta and no installation method exist for Linux or Mac yet.

[Setup .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
##### Windows
An installer from the master workflow action: 

[![.NET Core Desktop Windows Installer](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-desktop-windows-install.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-desktop-windows-install.yml)

![image](https://user-images.githubusercontent.com/40712699/181163617-3b56f4e4-bc33-44c5-9864-f49214f8a67a.png)

##### From source
```
dotnet restore './amp multiplatform.sln'
cd ./amp.EtoForms/
dotnet run ./amp.EtoForms.csproj
```
## Screenshots

*Windows*

![image](https://user-images.githubusercontent.com/40712699/179715280-786dc9b4-1e95-4f51-80d7-8bd516b0696c.png)

*macOS*

![image](https://user-images.githubusercontent.com/40712699/179735086-3558eac2-968c-4938-bbbc-1e266b8a15f4.png)

*Linux*

![image](https://user-images.githubusercontent.com/40712699/179740523-63f2d2ec-d9ad-4534-97ce-f88b9b4be6b2.png)


### Thanks to
* [ManagedBass](https://github.com/ManagedBass/ManagedBass) and [BASS](http://www.un4seen.com) for the audio-playback.
* [Eto.Forms](https://github.com/picoe/Eto) for the great cross-platform UI.
* [Audio Tools Library (ATL) for .NET](https://github.com/Zeugma440/atldotnet)
* [Serilog](https://serilog.net)
* [AutoMapper](https://automapper.org)
* [Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons)
* [FluentMigrator](https://github.com/fluentmigrator/fluentmigrator)
* [ResX Resource Manager](https://github.com/dotnet/ResXResourceManager)
* [JetBrains](https://www.jetbrains.com/?from=amp#) for their open source license(s).


![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)
