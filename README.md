# amp#
A music player for common audio formats with a simple and intuitive GUI.

[![Linux, Windows and macOS .NET Core](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/linux_windows_and_macos_dotnet.yml) [![Qodana](https://github.com/VPKSoft/amp-multi/actions/workflows/qodana.yml/badge.svg)](https://github.com/VPKSoft/amp-multi/actions/workflows/qodana.yml)

## Screenshots

*Windows*

![image](https://user-images.githubusercontent.com/40712699/209475030-7976bb36-800e-449a-aeca-252e72fca65c.png)

*macOS*

![image](https://user-images.githubusercontent.com/40712699/209474970-11f87fa7-21e9-45bb-b5c0-9de058de7585.png)

*Linux*

![image](https://user-images.githubusercontent.com/40712699/209474712-1aedacc5-6640-456d-a406-0d0a1e8a06fa.png)

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
* [CommandLineParser](https://github.com/commandlineparser/commandline)
* [JetBrains](https://www.jetbrains.com/?from=amp#) for their open source license(s).

![JetBrains Logo (Main) logo](https://resources.jetbrains.com/storage/products/company/brand/logos/jb_beam.svg)
