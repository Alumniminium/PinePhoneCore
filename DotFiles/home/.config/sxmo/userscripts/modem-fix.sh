#!/usr/bin/env sh

# basically 'unplugs' and then 'replugs' the modem into the phone
echo '2-1' | sudo tee /sys/bus/usb/drivers/usb/unbind
echo '2-1' | sudo tee /sys/bus/usb/drivers/usb/bind
