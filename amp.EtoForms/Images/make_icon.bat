magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 16x16 amp_16.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 24x24 amp_24.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 32x32 amp_32.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 48x48 amp_48.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 64x64 amp_64.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 96x96 amp_96.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 128x128 amp_128.png
magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 256x256 amp_256.png
magick convert -compress None amp_16.png amp_24.png amp_32.png amp_48.png amp_64.png amp_96.png amp_128.png amp_256.png amp.ico
del amp_16.png
del amp_24.png
del amp_32.png
del amp_48.png
del amp_64.png
del amp_96.png
del amp_128.png
del amp_256.png
pause