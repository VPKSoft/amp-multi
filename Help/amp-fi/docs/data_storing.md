# amp# data storing
The software saves track data and statistics into a local [SQLite](https://sqlite.org/index.html) database which is located along with the settings JSON file in:

* *Linux*: `~/.local/share/VPKSoft/amp`
* *Windows*: `%LOCALAPPDATA%\VPKSoft\amp`
* *macOS*: `~/.local/share/VPKSoft/amp`

## The internet
The software accesses internet in the following cases:

1. The user selects *Help --> Check* for updates from the main window menu
2. The setting *Check updates upon startup*, which by default is disabled is set to enabled by the user and and the software is started.

In both access cases a JSON file is downloaded into memory and version information stored in it is compared to the current program version.