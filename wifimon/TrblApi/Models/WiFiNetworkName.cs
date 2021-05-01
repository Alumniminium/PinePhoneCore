using System;

namespace wifimon.TrblApi.Models
{
    public class WiFiNetworkName
    {
        public ulong WiFiNetworkNameId{get;set;}
        public string SSID { get; set; }

        public override string ToString() => $"WiFi NetworkName #{WiFiNetworkNameId}{Environment.NewLine}SSID: {SSID}";
    }
}