# amp#
A music player for common audio formats with a simple and intuitive GUI.

[![Linux, Windows and macOS .NET Core](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml)

## Screenshots

*Windows*

![image](https://user-images.githubusercontent.com/40712699/182876733-d9747871-d529-4a76-b051-f500075d7bfb.png)

*macOS*

![image](https://user-images.githubusercontent.com/40712699/182877199-8977b660-12e9-478b-b00d-2da73ebdcbf2.png)

*Linux*

![image](https://user-images.githubusercontent.com/40712699/182876794-49b58f59-8e9b-4fc4-82d5-00cac749e563.png)

## Install
See the wiki for [stable release install](../../wiki).
* [Windows install](../../amp-multi/wiki/Windows-Install)
* [Linux install](../../amp-multi/wiki/Linux-instal)
* [macOS install](../../amp-multi/wiki/MacOS-Install)

### Nightly builds
See the wiki for the [nightly release install](../../wiki/Nightly-builds).

## Build from source
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
* [action gh-release](https://github.com/softprops/action-gh-release)
* [Nullsoft scriptable install system GitHub action](https://github.com/joncloud/makensis-action)
* [JetBrains](https://www.jetbrains.com/?from=amp#) for their open source license(s).


![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)
