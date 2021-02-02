# PinePhoneCore

### REJOICE BRETHEREN! 
I've built a library for you to use that does all the tedious stuff!
But this library is not yet complete (Feb 2021) and will probably change around for a while.
I recommend you fork and include the source project in your application, not a compiled library

![https://www.nuget.org/packages/PinePhoneCore/](https://img.shields.io/nuget/vpre/PinePhoneCore?style=for-the-badge)

#### Contribution 
Contribute however you like. No rules.
*just use common sense if available*

## Features

### Accelerometer (mpu6050)
* Scale Get/Set
* Sampling Frequency Get/Set
* Bias Get/Set
* Angular Velocity Bias Get/Set
* Scaled/Raw X, Y, Z
* Scaled/Raw Angular Velocity X, Y, Z

### Ambient Light Sensor (stk3310)
* Scale Get/Set
* Integration Time Get/Set
* Scaled/Raw Luminance

### Battery (axp20x-usb)
* Status
* Health
* Online
* Presence
* Type
* Charge Percentage
* Min/Max Voltage (Get/Set)
* Current Voltage
* Voltage Calibration (Last Full Voltage?)
* Constant Charging (Max)Current (Get/Set)
* Current Flow

### Bluetooth
* Enabled (Get/Set)

### CPU Core
* Enabled (Get/Set)
* Governor (Get/Set)
* (Critical)Temperature
* Frequency (Get/Set)
* Min/Max (Governor)Frequency
* Frequency Stats

### Display
* Brightness (Get/Set)
* Backlight Power (Get/Set)

### Headphone Jack
* Event based plug detection
* Headphone Kind (Headset, Headphones)

### Magnetometer (lis3mdl)
* Sampling Frequency (Get/Set)
* Scale (Get/Set)
* Raw/Scaled X,Y,Z

### Power Supply (axp20x-usb)
* Status
* Health
* Online
* Presence
* BC Enabled
* Type
* Min Voltage
* Input Current Limit (Get/Set)
* Input Current Limit DCP (Get/Set)
* Charging Protocol

### Proximity Sensor (stk3310)
* Scale Get/Set
* Integration Time Get/Set
* Scaled/Raw Proximity

### SoC (A64)
* GPU Temp
* CPU Temp

### WiFi
* Enabled (RFKILL, NMCLI, IFCONFIG, IP) (Get/Set)
* Connected (NMCLI, IP, IFCONFIG)
* MAC (IFCONFIG, IP) (Get/Set)
* SSID (NMCLI, IWGETID)
* Local IP (IFCONFIG, IP) (Get/Set)
* Signal/Noise Level (IWCONFIG)
* Link Quality (IWCONFIG)
* Scan (IW, IWLIST, NMCLI)

