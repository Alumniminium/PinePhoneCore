using System;
using System.Collections.Generic;
using System.Net;
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
                Global.WiFi.Enabled_IPA = on;
            else if (Dependencies.Found["rfkill"])
                Global.WiFi.Enabled_RFKILL = on;
            else if (Dependencies.Found["nmcli"])
                Global.WiFi.Enabled_NMCLI = on;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static bool IsEnabled()
        {
            if (Dependencies.Found["rfkill"])
                return Global.WiFi.Enabled_RFKILL;
            else if (Dependencies.Found["ifconfig"])
                return Global.WiFi.Enabled_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return Global.WiFi.Enabled_IPA;
            else if (Dependencies.Found["nmcli"])
                return Global.WiFi.Enabled_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }        
        public static bool IsConnected()
        {
            if (Dependencies.Found["ifconfig"])
                return Global.WiFi.IsConnected_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return Global.WiFi.IsConnected_IPA;
            else if (Dependencies.Found["nmcli"])
                return Global.WiFi.IsConnected_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static string GetMAC()
        {
            if (Dependencies.Found["ifconfig"])
                return Global.WiFi.MAC_IFCONFIG;
            else if (Dependencies.Found["ip"])
                return Global.WiFi.MAC_IPA;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static void SetMAC(string mac)
        {
            if (Dependencies.Found["ifconfig"])
                Global.WiFi.MAC_IFCONFIG = mac;
            else if (Dependencies.Found["ip"])
                Global.WiFi.MAC_IPA = mac;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static string GetCurrentSSID()
        {
            if (Dependencies.Found["iwgetid"])
                return Global.WiFi.SSID_IWGETID;
            else if (Dependencies.Found["nmcli"])
                return Global.WiFi.SSID_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static IPAddress GetLocalIP()
        {
            var ip = string.Empty;
            if (Dependencies.Found["ifconfig"])
                ip = Global.WiFi.LocalIP_IFCONFIG;
            else if (Dependencies.Found["ip"])
                ip = Global.WiFi.LocalIP_IPA;
            else
                throw new NotSupportedException("None of the required dependencies were found.");

            return IPAddress.Parse(ip);
        }
        public static void SetLocalIP(string ip)
        {
            if (Dependencies.Found["ifconfig"])
                Global.WiFi.LocalIP_IFCONFIG = ip;
            else if (Dependencies.Found["ip"])
                Global.WiFi.LocalIP_IPA = ip;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static int GetSignalLevel()
        {
            if (Dependencies.Found["iwconfig"])
                return Global.WiFi.SignalLevel_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static int GetNoiseLevel()
        {
            if (Dependencies.Found["iwconfig"])
                return Global.WiFi.NoiseLevel_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
        public static int GetLinkQuality()
        {
            if (Dependencies.Found["iwconfig"])
                return Global.WiFi.LinkQuality_IWCONFIG;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }

        public static List<string> Scan()
        {
            if (Dependencies.Found["iw"])
                return Global.WiFi.Scan_IW;
            else if (Dependencies.Found["iwlist"])
                return Global.WiFi.Scan_IWLIST;
            else if (Dependencies.Found["nmcli"])
                return Global.WiFi.Scan_NMCLI;
            else
                throw new NotSupportedException("None of the required dependencies were found.");
        }
    }
}
