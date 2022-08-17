#!/bin/bash
systems=("linux" "windows" "macos")
locales=("en" "fi")

mkdir help_pack

for i in "${systems[@]}"
do
  for j in "${locales[@]}"
  do
    cd "amp-$j"
    ./copy_platform_files.sh "$i"
    mkdocs build
    mkdir "../help_pack/amp-$j-$i"
    mv site/* "../help_pack/amp-$j-$i"
    cd ..
  done
done

zip -r "help_pack" "help_pack.zip"
