using System;
using System.Threading.Tasks;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;

namespace ppmusicsyncd
{
    class Program
    {
        private const string HOME_SSID = "fsociety";

        static async Task Main(string[] args)
        {
            await Dependencies.TestDependencies();
            Notification.Display("Synchronizing Music!","mo",TimeSpan.FromSeconds(10));
            WiFi.Enabled_NMCLI = true;
            var networks = PinePhoneCore.PinePhoneHelpers.PinePhoneWiFi.Scan();

            if (networks.Contains(HOME_SSID))
                LocalSync();
            else
                RemoteSync();
        }

        private static void RemoteSync()
        {
            //Modem.Enabled=true;
            //Modem.Connect("");
            var output = Shell.GetValue("rsync", "-avh trbl@trbl.her.st:/mnt/SDA/SyncMusic/ /home/mo/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}","mo",TimeSpan.FromSeconds(10));
        }

        private static void LocalSync()
        {
            WiFi.Connect(HOME_SSID);
            var output = Shell.GetValue("rsync", "-avh trbl@192.168.0.2:/mnt/SDA/SyncMusic/ /home/mo/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}","mo",TimeSpan.FromSeconds(10));
        }
    }
}
