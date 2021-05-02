#!/usr/bin/env zsh
if [ "$(lpass status)" = 'Not logged in.' ]; then
    st -e sh -c 'lpass login domi@outlook.at'   
fi
st -e sh -c '6cord -u $(lpass show -u discordapp.com) -p $(lpass show -p discordapp.com)'
