#!/bin/sh
mkdir -p ./AppDir/usr/bin/ 
dotnet publish ../../../amp.EtoForms/amp.EtoForms.csproj -c Release --runtime linux-x64 --output ./AppDir/usr/bin/ --self-contained
