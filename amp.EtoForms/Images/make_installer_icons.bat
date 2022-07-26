magick convert -transparent white music-note-svgrepo-com_modified.svg -resize 32x32 amp_32.png
magick convert -compress None amp_32.png amp_install_32.ico

magick convert -transparent white music-note-svgrepo-com_modified_uninstall.svg -resize 32x32 amp_uninstall_32.png
magick convert -compress None amp_uninstall_32.png amp_uninstall_32.ico
del amp_uninstall_32.png
del amp.png
pause