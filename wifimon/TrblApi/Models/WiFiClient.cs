using System;
using System.Collections.Generic;

namespace wifimon.TrblApi.Models
{
    public class WiFiClient
    {
        public ulong WiFiClientId{get;set;}
        public WiFiMac WiFiMac {get;set;}
        public List<WiFiAccessPoint> AccessPoints {get;set;}
        public DateTime LastSeen { get; set; }

        public override string ToString() => $"WiFi Client #{WiFiClientId}{Environment.NewLine}MAC: {WiFiMac.MAC}{Environment.NewLine}Last Seen: {LastSeen}";
    }
}