#!/usr/bin/env sh

state=$(sysctl net.ipv4.ip_forward | cut -d '=' -f 2 | cut -d ' ' -f 2)

if [ $state = "0" ]; then
	sudo sysctl net.ipv4.ip_forward=1
	sudo iptables -t nat -A POSTROUTING -o wwan0 -j MASQUERADE
	sudo iptables -A FORWARD -m conntrack --ctstate RELATED,ESTABLISHED -j ACCEPT
	sudo iptables -A FORWARD -i usb0 -o wwan0 -j ACCEPT
else
	sudo sysctl net.ipv4.ip_forward=0
	sudo iptables -t nat -D POSTROUTING -o wwan0 -j MASQUERADE
	sudo iptables -D FORWARD -m conntrack --ctstate RELATED,ESTABLISHED -j ACCEPT
	sudo iptables -D FORWARD -i usb0 -o wwan0 -j ACCEPT
fi
