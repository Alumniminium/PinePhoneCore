using System;

namespace wifimon.TrblApi.Models
{
    public class WiFiProbe
    {
        public ulong WiFiProbeId{get;set;}
        public WiFiMac WiFiMac {get; set; }
        public WiFiNetworkName WiFiNetworkName {get; set; }
        public DateTime LastSeen { get; set; }

        public WiFiClient Client {get;set;}

        public override string ToString() => $"Probe #{WiFiProbeId}{Environment.NewLine}MAC: {WiFiMac.MAC}{Environment.NewLine}SSID: {WiFiNetworkName.SSID}{Environment.NewLine}Last Seen: {LastSeen}";
    }
}