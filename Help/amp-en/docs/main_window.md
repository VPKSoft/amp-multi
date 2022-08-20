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
Clears the queued tracks from the current album. Keyboard shortcut: <kbd>Ctrl</kbd>+<kbd>D</kbd> or <kbd>⌘</kbd>+<kbd>D</kbd>.

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

## Playback toolbar
The playback toolbar contains typical playback controls and few mode settings.

![image](img/toolbar1.png)

**The previous track button -** Plays the previous track if there is one in the history.

![image](img/gui/ic_fluent_previous_48_filled.png)

**The play/pause button -** Resumes or pauses the playback.

![image](img/gui/play.png) ![image](img/gui/ic_fluent_pause_48_filled.png)

**The next track button -** Plays the next track. The next track depends on if there are tracks queued, shuffle and repeat modes.

![image](img/gui/ic_fluent_next_48_filled.png)

**The show queue toggle button -** Filters the task list by queued tracks. If nothing is queued, the track list will be empty. Searching via text can also be applied to the track list filtered by queue.

![image](img/gui/queue_three_dots.png)

**The shuffle toggle button -** Toggles the playback randomization.

![image](img/gui/shuffle-random-svgrepo-com_modified.png)

**The repeat toggle button -** Toggles to playback repeat. E.g. continuous playback.

![image](img/gui/repeat-svgrepo-com_modified.png)

**The toggle stack queue button -** The stack queue mode ensures that the queue never ends. The played tracks just are moved to the back of the queue and a defined part of the queue is re-randomized.

![image](img/gui/stack_queue_three_dots.png)

## Album selector
The album can be changed from the album selector. The previous album is automatically saved before an album is changed.

![image](img/album_selector1.png)

## Sound and rating
**The volume slider**
The volume slider controls the main volume of the playback. The volume level has no effect on the system volume setting.

**The audio track volume slider**
The audio track volume slider controls the volume of an individual audio track playback. This value is saved to the internal database of the software.

**The rating slider**
With the rating slider the track can be given a rating. This value is saved to the internal database of the software.

## The position slider
The position slider allows to control the playback position of the current track.

## Current track title and duration
The duration displays the time left to playback the track. By clicking the track title you can focus on the current track in the track list.

## The search box
The search box allows to filter the tracks from the current album. The search is directed to all the properties of the audio track:

- Tag data

    * Year
    * Album
    * Artist
    * File tag contents including possible lyrics
    * Etc...

- File name

The search box is focused when the main window is active and characters are written, so the search does not need to be explicitly focused by mouse or by keyboard. **Just type!**

Use <kbd>Escape</kbd> key to clear the search box.

## The track list
The track list contains all the tracks in the current album. It also has columns to indicate the index in the queue titled `Q` and a column to indicate the alternate queue index titled `*`. The columns can be re-ordered and the ordering is saved into the application settings.

Tracks can be added to queue using the <kbd>+</kbd> key or inserted on top of the queue using <kbd>Ctrl</kbd>+<kbd>+</kbd> or <kbd>⌘</kbd>+<kbd>+</kbd>.

Tracks can be added to the alternate queue using the <kbd>\*</kbd> key or inserted on top of the queue using <kbd>Ctrl</kbd>+<kbd>\*</kbd> or <kbd>⌘</kbd>+<kbd>\*</kbd>.

The reason for the alternate queue is to allow to create a new queue to save while listening to music and possibly queuing new tracks to playback.

## The status panel
The status panel at the bottom of the main displays the number of queued tracks, the number of filtered tracks and the number of total tracks within the album.