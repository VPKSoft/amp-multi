mkdir bin
dotnet publish ../../amp.EtoForms/amp.EtoForms.csproj --configuration "Release" --output ./bin
mkdir -p ./amp#.app/Contents/MacOS
mv ./bin/* ./amp#.app/Contents/MacOS
mkdir -p ./amp#.app/Contents/Resources
cp ../../amp.EtoForms/Images/AmpIcon.icns ./amp#.app/Contents/Resources
cp ./Info.plist ./amp#.app/Contents
chmod +x ./amp#.app/Contents/MacOS/amp.EtoForms
xattr -c ./amp#.app/Contents/MacOS/amp.EtoForms
chmod +x ./amp#.app
xattr -c ./amp#.app
zip -r ../amp#.zip ./amp#.app
