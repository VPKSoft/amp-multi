#!/bin/bash
systems=("linux" "windows" "macos")
locales=("en" "fi")
for i in "${systems[@]}"
do
  for j in "${locales[@]}"
  do
    cd "amp-$j"
    ./copy_platform_files.sh "$i"
    mkdocs build
    mv site "amp-$j-$i"
    zip -r "amp-$j-$i.zip" "amp-$j-$i"
    mv "amp-$j-$i.zip" ../"amp-$j-$i.zip"
    cd ..
  done
done