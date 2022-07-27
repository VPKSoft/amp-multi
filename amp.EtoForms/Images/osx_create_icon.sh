#!/bin/bash
mkdir ./AmpIcon.iconset
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 16x16 ./AmpIcon.iconset/icon_16x16.png
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 32x32 ./AmpIcon.iconset/icon_16x16@2x.png

magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 64x64 ./AmpIcon.iconset/icon_64x64.png
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 128x128 ./AmpIcon.iconset/icon_64x64@2x.png

magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 128x128 ./AmpIcon.iconset/icon_128x128.png
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 256x256 ./AmpIcon.iconset/icon_128x128@2x.png

magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 256x256 ./AmpIcon.iconset/icon_256x256.png
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 512x512 ./AmpIcon.iconset/icon_256x256@2x.png

magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 512x512 ./AmpIcon.iconset/icon_512x512.png
magick convert -transparent white ./music-note-svgrepo-com_modified.svg -resize 1024x1024 ./AmpIcon.iconset/icon_512x512@2x.png

iconutil -c icns ./AmpIcon.iconset

rm -rf ./AmpIcon.iconset
