# amp#
A music player for common audio formats with a simple and intuitive GUI.

[![Linux, Windows and macOS .NET Core](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml)

## Screenshots

*Windows*

![image](https://user-images.githubusercontent.com/40712699/187039283-2fa2b002-622f-46d4-be2a-a2d3224116aa.png)

*macOS*

![image](https://user-images.githubusercontent.com/40712699/187039304-cb90e389-c25f-4751-86b7-7708c5485a6a.png)

*Linux*

![image](https://user-images.githubusercontent.com/40712699/187039310-bd470e46-1550-4b39-90db-9dfdd300cbfa.png)

## Install
See the wiki for [stable release install](../../wiki).
* [Windows install](../../wiki/Windows-Install)
* [Linux install](../../wiki/Linux-install)
* [macOS install](../../wiki/MacOS-Install)

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
