#!/usr/bin/env zsh

notify-send "syncing music..."
out=$(rsync -avh trbl@192.168.0.2:/mnt/SDA/SyncMusic/ /home/mo/music/)
notify-send "$out" -t 10000
exit 0
