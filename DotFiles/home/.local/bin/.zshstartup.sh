#!/bin/zsh

time=$(date +%s)
filetime=$(stat -c "%Y" ~/.cache/ip)
expire=$(( filetime + 60 ))

if [ $time -gt $expire ]; then
    curl --no-progress-meter wtfismyip.com/text > ~/.cache/ip &
fi

disk=$(df -lhk / --output=target,pcent,size,used,avail -B G | sed 1d)

figlet -c "$(hostname).her.st" | lolcat
echo "\x1B[01;96m$(figlet -l -f term "  OS INFO ================================================")"
echo "  \x1B[01;94mHost:       \x1B[01;95m $(hostname).her.st, $(cat ~/.cache/ip)"
echo "  \x1B[01;94mOS:         \x1B[01;95m $(cat /etc/os-release | grep NAME | head -n1 |  cut -d '"' -f 2), $(uname -r)"
echo ""
echo "\x1B[01;96m$(figlet -l -f term "  HW INFO ================================================")"
echo "  \x1B[01;94mCPU:        \x1B[01;95m"`lscpu | grep "Model name:" | xargs | awk -F ': ' '{print $2}'`
echo "  \x1B[01;94mMemory:    \x1B[01;95m"`free -mht| awk '/Mem/{print " \t\tTotal: " $2 "\tFree: " $7}'`
echo "  \x1B[01;94mDisk:      \x1B[01;95m $(echo "$disk" | awk -F ' ' '{print $3}') / $(echo "$disk" | awk -F ' ' '{print $4}') ($(echo "$disk" | awk -F ' ' '{print $2}') used)"
echo ""
echo "  type 'screen -RR' if you want a screen session"%  