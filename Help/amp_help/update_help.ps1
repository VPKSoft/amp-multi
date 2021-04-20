if ($Env:CIRCLECI -eq "true") {
    Write-Output "CircleCI detectected, set working directory to: $Env:CIRCLE_WORKING_DIRECTORY\amp"
    Set-Location -Path "$Env:CIRCLE_WORKING_DIRECTORY\amp"
}

$wiki_folder = ".\amp.wiki"
Remove-Item -Path $wiki_folder -Recurse -Force -ErrorAction Ignore
git clone https://github.com/VPKSoft/amp.wiki.git
Copy-Item ".\amp.wiki\*.md" ".\docs"
mkdocs build
$help_outpath = "..\..\amp\bin\Release\net5.0-windows\win10-x64\Help"
Remove-Item -Path $help_outpath -Recurse -Force -ErrorAction Ignore
mkdir -Path $help_outpath
Copy-Item .\site\*.* -Destination $help_outpath