#!/usr/bin/env zsh

notify-send "syncing music..."
out=$(dotnet ~/PinePhone/ppmusicsyncd/bin/Debug/net5.0/ppmusicsyncd.dll)
notify-send "$out" -t 10000
exit 0
