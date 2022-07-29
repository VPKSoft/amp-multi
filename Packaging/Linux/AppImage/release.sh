#!/bin/sh
mkdir -p ./AppDir/usr/bin/ 
dotnet publish ../../../amp.EtoForms/amp.EtoForms.csproj -c Release --runtime linux-x64 --output ./AppDir/usr/bin/ --self-contained
wget "https://github.com/AppImage/AppImageKit/releases/download/continuous/appimagetool-x86_64.AppImage"
chmod a+x appimagetool-x86_64.AppImage
./appimagetool-x86_64.AppImage ./AppDir
