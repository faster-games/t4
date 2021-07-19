#!/bin/bash

rm -rf dist/
rm -rf obj/

git clone https://github.com/faster-games/UnityFX.git dist/.unityfx
docfx
