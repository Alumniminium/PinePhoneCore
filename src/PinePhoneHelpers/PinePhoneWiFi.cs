using System;
using System.Collections.Generic;
using System.Net;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.PinePhoneHelpers
{
    // [done] TODO
    // 
    // [X] Enable
    // [X] Disable
    // [X] IsConnected
    // [X] Get mac
    // [X] Set mac
    // [X] Get SSID
    // [ ] Connect
    // [ ] Disconnect
    // [X] Scan
    // [X] Signal Level
    // [X] Noise Level
    // [X] Link Quality
    // [X] Get Local IP
    // [X] Set Local IP
    //

    public static class PinePhoneWiFi
    {
        public static void Toggle(bool on)
        {
            if (Dependencies.Found["ip"])
                WiFi.Enabled_IPA = on;
            else if (Dependencies.Found["rfkill"])
                WiFi.Enabled_RFKILL = on;
            else if (Dependencies.Found["nmcli"])
                WiFi.Enabled_NMCLI = on;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static bool IsEnabled()
        {
            if (Dependencies.Found["rfkill"])
                return WiFi.Enabled_RFKILL;
            else if (Dependencies.Found["ifconfig"])
                return WiFi.Enabled_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return WiFi.Enabled_IPA;
            else if (Dependencies.Found["nmcli"])
                return WiFi.Enabled_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }        
        public static bool IsConnected()
        {
            if (Dependencies.Found["ifconfig"])
                return WiFi.IsConnected_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return WiFi.IsConnected_IPA;
            else if (Dependencies.Found["nmcli"])
                return WiFi.IsConnected_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static string GetMAC()
        {
            if (Dependencies.Found["ifconfig"])
                return WiFi.MAC_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return WiFi.MAC_IPA;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static void SetMAC(string mac)
        {
            if (Dependencies.Found["ifconfig"])
                WiFi.MAC_IFCONFIG = mac;
            else if (Dependencies.Found["ip"])
                WiFi.MAC_IPA = mac;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static string GetCurrentSSID()
        {
            if (Dependencies.Found["iwgetid"])
                return WiFi.SSID_IWGETID;
            else if (Dependencies.Found["nmcli"])
                return WiFi.SSID_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static IPAddress GetLocalIP()
        {
            var ip = string.Empty;
            if (Dependencies.Found["ifconfig"])
                ip = WiFi.LocalIP_IFCONFIG;
            else if (Dependencies.Found["ip"])
                ip = WiFi.LocalIP_IPA;
            else
                throw new NotSupportedException("None of the required dependencies were found.");

            return IPAddress.Parse(ip);
        }
        public static void SetLocalIP(string ip)
        {
            if (Dependencies.Found["ifconfig"])
                WiFi.LocalIP_IFCONFIG = ip;
            else if (Dependencies.Found["ip"])
                WiFi.LocalIP_IPA = ip;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static int GetSignalLevel()
        {
            if (Dependencies.Found["iwconfig"])
                return WiFi.SignalLevel_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static int GetNoiseLevel()
        {
            if (Dependencies.Found["iwconfig"])
                return WiFi.NoiseLevel_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static int GetLinkQuality()
        {
            if (Dependencies.Found["iwconfig"])
                return WiFi.LinkQuality_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static List<string> Scan()
        {
            if (Dependencies.Found["iwlist"])
                return WiFi.Scan_IWLIST;
            else if (Dependencies.Found["iw"])
                return WiFi.Scan_IW;
            else if (Dependencies.Found["nmcli"])
                return WiFi.Scan_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
    }
}
