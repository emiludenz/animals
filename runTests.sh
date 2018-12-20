#!/bin/bash

fsharpc --nologo -a animalsSmall.fs 
fsharpc --nologo -r animalsSmall.dll testAnimalsSmall.fsx && mono testAnimalsSmall.exe

echo "Du ønskes godt nytår!"