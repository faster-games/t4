Remove-Item -Recurse -Force dist/
Remove-Item -Recurse -Force obj/

git clone https://github.com/faster-games/UnityFX.git dist/.unityfx
docfx
