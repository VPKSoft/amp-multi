$wiki_folder = ".\amp.wiki"
Remove-Item -Path $wiki_folder -Recurse -Force -ErrorAction Ignore
git clone https://github.com/VPKSoft/amp.wiki.git
Copy-Item ".\amp.wiki\*.md" ".\docs"