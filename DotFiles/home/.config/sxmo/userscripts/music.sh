#!/usr/bin/env sh

~/.local/bin/audioctl.sh h 60
st -e sh -c 'mpv --shuffle ~/music/'
