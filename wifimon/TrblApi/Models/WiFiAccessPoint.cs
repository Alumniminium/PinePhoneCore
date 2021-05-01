using System;
using System.Collections.Generic;

namespace wifimon.TrblApi.Models
{
    public class WiFiAccessPoint
    {
        public ulong WiFiAccessPointId{get;set;}
        public WiFiMac WiFiMac {get;set;}
        public WiFiNetworkName WiFiNetworkName {get;set;}
        public List<WiFiClient> Clients {get;set;}
        public DateTime LastSeen { get; set; }

        public override string ToString() => $"AccessPoint #{WiFiAccessPointId}{Environment.NewLine}SSID: {WiFiNetworkName.SSID}{Environment.NewLine}MAC: {WiFiMac.MAC}{Environment.NewLine}Last Seen: {LastSeen}";
    }
}