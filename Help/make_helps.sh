#!/bin/bash
systems=("linux" "windows" "macos")
locales=("en" "fi")
for i in "${systems[@]}"
do
  for j in "${locales[@]}"
  do
    cd "amp-$j"
    ./copy_platform_files.sh
    mkdocs build
    mv site "amp-$j-$i"
    zip -r "amp-$j-$i.zip" "amp-$j-$i"
  done
done
