# The settings dialog
The settings dialog contains the software settings divided into different categories.

## Common settings
The common settings contains common and miscellaneous settings.

**Enabled quit hours**
This setting allows the software to go on pause or quiet down the playback volume on a specific time span. E.g. you can set the evening to late morning to be quiet to keep the peace with the neighbors.

**Language**
The UI language, see [the currently supported languages](supported_languages.md) for a list. For this setting to take place the software must be manually restarted.

**Check for updates upon startup**
Setting this to true the software checks for updates when it is run. By default the setting is disabled.

**Stack queue**
The software can be set to stack queue mode. In this mode the queue will not be consumed during playback. The played item is moved to the back of the queue and a specified percentage playback order in the queue is re-randomized.

**Use track image window**
This option displays a small frame window with the track image on the right side of the main window if the track has an image assigned to it.

The *Auto-hide album image window* keeps the track image window hidden if there is no image assigned for the track.

**Display playlist column header**
This option simply disables or enables the column headers of the main window track list.

**Retry count on playback failure**
A value of how many times the software tries playback with a different track if the playback fails before stopping to retry.

**Help folder**
As the help files are in a different package this is the location where the package was extracted. This should be set to the location of the extracted `help_pack` folder which contains many sub-folders.

*The Common settings tab*

![image](img/settings1.png)

## Modified random
The modified or biased or weighted random is a randomization so that the software randomizes tracks with better rating more often than those with less rating. You can also take into account the amount of playbacks, amount of early skips, amounts of randomized non-skipped playbacks.

*The modified random tab*

![image](img/settings2.png)

## Track naming
The track naming settings affect of how the tracks are displayed in the track list. The *Track naming formula* is for tracks which have not been renamed by the user. The *Renamed track naming formula* is for user named tracks.

The *Minimum name length* and the *If generated name contains no letters, fall back to the file name* affect on situations when the track naming formula would generate a non-valid name for the track. E.g. 'Track 01 - .'.

The allowed formula sections are explained in the track naming tab and they can always be reset to default values in case an error.

*The track naming tab*

![image](img/settings3.png)

## Audio visualization

**Display Audio visualization**
*Display audio visualization* indicates whether to display audio visualization on bottom of the track list during playback.

*Audio visualizer FFT Window function*
With this option the FFT ([Fast Fourier Transform](https://en.wikipedia.org/wiki/Fast_Fourier_transform)) [windowing function](https://en.wikipedia.org/wiki/Window_function) can be changed. Just keep it to Hanning if you do not know what this means. It affects the form of the visualized audio signal bu assigning weight to the different frequency bands - thats how the author (with extremely limited knowledge) understands the thing.

**Bar visualization mode**
The audio visualization is rendered in bars if this is checked. Otherwise a line is rendered.

**Visualize audio levels**
A value indicating whether to visualize the audio levels with the audio visualization area.

**Horizontal level visualization**
A value indicating whether to visualize the audio levels vertical or horizontal.

*The audio levels in vertical*

![image](img/audio_visualization1.png)

*The audio levels in horizontal*

![image](img/audio_visualization2.png)


*The audio visualization tab*

![image](img/settings4.png)

## Miscellaneous
On this tab there are the miscellaneous settings which do not belong to any specific category directly.

**Actions after the queue has finished**

Two actions can be defined to in the programs which are run when the queue has finished playing.

The actions are: 

* None
* Stop playback
* Pop stashed queue
* Quit the application

If the first action can not be run, the second action is run. Otherwise the first action is run and the situation is re-evaluated after the queue has finished playing again.

**Backup application data**

Allows user to backup the SQLite database and setting files of the software into a single zip file. The backup does not require application restart, only the playback is required to be stopped.

Before the backup is made the following information is displayed: *"The playback will be stopped for the backup."* followed by *"Backup completed"* message.

**Restore application data**

Allows user to restore backed up application data from a zip file. The zip file contents will not validated. The playback will be stopped during the restore and afterwards the application will exit and it has to be started again manually.

Before the backup is restored a following information is displayed: *"The application will shut down after the backup has been restored. Start the software again manually."*. After the backup has been restored a following message is displayed: *"Backup restore completed."*.

**Error-tolerant search**

Allows to use error-tolerant search for audio track filtering using *FuzzyWuzzy* algorithm.

The *Minimum tolerance* defines the filtering tolerance between 0 to 100. The *Maximum results* defines the amount of filtered results the filtering should return.

The *Always use error-tolerant search* defines if the *FuzzyWuzzy* algorithm should always be used for filtering, otherwise the algorithm is only used if the normal filtering yields no results.

**Display track rating column**

Indicates if the track rating column should be displayed in the playlist.

*The Miscellaneous tab*

![image](img/settings5.png)
