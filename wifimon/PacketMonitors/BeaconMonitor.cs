using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet.Ieee80211;
using wifimon.TrblApi.Models;

namespace wifimon
{
    public class BeaconMonitor : BaseMonitor<BeaconFrame>
    {
        public BeaconMonitor(string path) : base(path) { }

        public override async Task HandlePacket(RadioPacket packet, BeaconFrame bp)
        {
            var signal = packet[RadioTapType.DbmAntennaSignal];
            var ssidObj = bp.InformationElements.Where(b => b.Id.ToString() == "ServiceSetIdentity").First();
            var ssid = Encoding.UTF8.GetString(ssidObj.Value, 0, ssidObj.ValueLength).Trim();

            if (string.IsNullOrWhiteSpace(ssid))
                return;

            var dbs = signal.ToString().Split(' ',StringSplitOptions.TrimEntries)[1];
            var db = int.Parse(dbs);
            var output = $"{DateTime.Now},Beacon,{bp.SourceAddress},{ssid},{Helper.DbToPercent(db)}";
            if (TryCache(ssid, bp.SourceAddress.ToString()))
            {
                var mac = new WiFiMac { MAC = bp.SourceAddress.ToString() };
                var ap = new WiFiAccessPoint
                {
                    LastSeen = DateTime.Now,
                    WiFiMac = mac,
                    WiFiNetworkName = new WiFiNetworkName { SSID = ssid }
                };
                await TrblClient.SubmitBeacon(ap);
                Log(output);
            }
        }
    }
}