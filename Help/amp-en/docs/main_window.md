# The amp# main window

The main window consists of the menu, playback controls album selector, volume, rating, position, search, track list, audio visualization and status bar.

![image](img/main_window1.png)

## The menu

### The File menu
#### Add music files...
* **Add files to album...**: This opens a select file dialog with multiple selection to the current album.

* **Add folder contents to album...**: This opens a select folder dialog to select a folder which music files to recursively add to the current album.

#### Album
This opens a dialog to [manage the albums](album.md) within the program database.

#### Track information
Opens a dialog to [display and manage](track_info.md) the currently selected track metadata (IDvX, etc). Also a track image can be set from track information. Keyboard shortcut: <kbd>F4</kbd>.

#### Quit
Exists the program. Keyboard shortcut: <kbd>Ctrl</kbd>+<kbd>Q</kbd> or <kbd>⌘</kbd>+<kbd>Q</kbd>

### The queue menu

#### Save current queue
Opens a dialog to allow user to specify a name for the current queue or alternate queue to save so it can be loaded and played back again when the user wishes to do so. Keyboard shortcut: <kbd>Ctrl</kbd>+<kbd>S</kbd> or <kbd>⌘</kbd>+<kbd>S</kbd>.

#### Saved queues
Opens a dialog where [saved queue snapshots](saved_queues_dialog.md) can be loaded, edited and deleted. Keyboard shortcut: <kbd>F3</kbd>.

#### Clear queue
Clears the queued songs from the current album. Keyboard shortcut: <kbd>Ctrl</kbd>+<kbd>D</kbd> or <kbd>⌘</kbd>+<kbd>D</kbd>.

#### Scramble queue
Re-randomizes the current queue playback order. If two or more queued items are selected, only the selection is re-randomized. Keyboard shortcut: <kbd>F7</kbd>.

### The Tools menu

#### Settings
Opens the [settings dialog](settings.md) for various software settings.

#### Color settings
Opens a [dialog where the software colors can be set](color_settings.md).

#### Update track metadata
Goes through all the tracks in all albums and reads the track file metadata and updates it to the corresponding record of software. This tool is designed to be used if the track metadata is modified elsewhere as the amp# only reads the metadata tags once.

### The Help menu

#### About
Displays an about dialog for version and license information of the application.

#### Help
This help is displayed via web browser. Keyboard shortcut: <kbd>F1</kbd>.

#### Check for new version
Checks if a new version of the amp# software is available for download and:
* If one is available the user is displayed a dialog with changes compared to the current version and a clickable link to the download web page.
* If no new version is available a dialog informing about that is displayed.