mkdir bin
dotnet publish ../../amp.EtoForms/amp.EtoForms.csproj --configuration "Release" --output ./bin
mkdir -p ./amp.EtoForms.app/Contents/MacOS
mv ./bin/* ./amp.EtoForms.app/Contents/MacOS
mkdir -p ./amp.EtoForms.app/Contents/Resources
cp ../../amp.EtoForms/Images/AmpIcon.icns ./amp.EtoForms.app/Contents/Resources
cp ./Info.plist ./amp.EtoForms.app/Contents
chmod +x ./amp.EtoForms.app/Contents/MacOS/amp.EtoForms
xattr -c ./amp.EtoForms.app/Contents/MacOS/amp.EtoForms
chmod +x ./amp.EtoForms.app
xattr -c ./amp.EtoForms.app
zip -r ../amp.EtoForms.zip ./amp.EtoForms.app
