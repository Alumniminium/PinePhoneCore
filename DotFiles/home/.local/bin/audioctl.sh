#!/bin/bash

input=$1
volume=$2
earpiecevol=$(($volume * 2))

amixer sset "AIF1 DA0" 115
amixer sset "AIF1 DA0 Stereo" "Stereo"
amixer sset "Line Out Source" 'Mono Differential'
amixer sset "DAC" 100%
amixer sset "DAC" unmute
amixer sset "DAC Reversed" unmute
amixer sset "AIF1 Slot 0 Digital DAC" unmute
amixer sset "Earpiece Source" 'Left Mix'

if [ $input = "s" ]; then
	amixer sset "Line Out" unmute
	amixer sset "Earpiece" unmute
	amixer sset "Earpiece" $earpiecevol%
	amixer sset "Line Out" $volume%
elif [ $input = "h" ]; then
	amixer sset "Earpiece" mute
	amixer sset "Line Out" mute
	amixer sset "Headphone" unmute
	amixer sset "Headphone" $volume%
else
	amixer sset "Line Out" mute
	amixer sset "Earpiece" unmute
	amixer sset "Earpiece" $volume%
fi
	
