#!/bin/bash

export PATH="/bin:/opt:/usr/local/sbin:/usr/local/bin:/usr/bin"
export PATH="${PATH}:${HOME}/.local/bin/"
export PATH="$PATH:${HOME}/.dotnet/tools"
export PATH="$PATH:${HOME}/.cargo/bin"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export EDITOR=/usr/bin/micro
export XDG_CONFIG_HOME="$HOME/.config"

# fix java applications not rendering
export _JAVA_AWT_WM_NONREPARENTING=1
