# amp#
A music player for common audio formats with a simple and intuitive GUI.

[![Linux, Windows and macOS .NET Core](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml)

## Screenshots

*Windows*

![image](https://user-images.githubusercontent.com/40712699/179715280-786dc9b4-1e95-4f51-80d7-8bd516b0696c.png)

*macOS*

![image](https://user-images.githubusercontent.com/40712699/179735086-3558eac2-968c-4938-bbbc-1e266b8a15f4.png)

*Linux*

![image](https://user-images.githubusercontent.com/40712699/179740523-63f2d2ec-d9ad-4534-97ce-f88b9b4be6b2.png)

### Install
**NOTE**: This is currently beta and no installation method exist for Linux yet.

[Setup .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
#### Windows
An installer from the master workflow action: 

[![.NET Core Desktop Windows Installer](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-desktop-windows-install.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-desktop-windows-install.yml)

![image](https://user-images.githubusercontent.com/40712699/181163617-3b56f4e4-bc33-44c5-9864-f49214f8a67a.png)

#### MacOS
An app bundle from the master workflow action:

[![.NET Core MacOS bundle](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-macos-app-bundle.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/dotnet-macos-app-bundle.yml)

![image](https://user-images.githubusercontent.com/40712699/181792023-ec69bb0a-3cd6-42f2-a783-db1ca7c8d2f9.png)

**Note**, you meed to clear the `com.apple.quarantine` extended attribute by running: 
`xattr -c ./amp.EtoForms.app`

#### From source
```
dotnet restore './amp multiplatform.sln'
cd ./amp.EtoForms/
dotnet run ./amp.EtoForms.csproj
```

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
