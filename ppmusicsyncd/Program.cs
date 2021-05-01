using System;
using System.Threading.Tasks;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;

namespace ppmusicsyncd
{
    class Program
    {
        private const string HOME_SSID = ".fsociety";
        private const string LTE_PROFILE = "PublicIP";

        static async Task Main(string[] args)
        {
            await Dependencies.TestDependencies();
            Notification.Display("Synchronizing Music!","alarm",TimeSpan.FromSeconds(10));
            WiFi.Enabled_NMCLI = true;
            Notification.Display("Let's see where we are... scanning wifi networks","alarm",TimeSpan.FromSeconds(10));
            var networks = PinePhoneCore.PinePhoneHelpers.PinePhoneWiFi.Scan();
            Notification.Display($"Found {networks.Count} networks...","alarm",TimeSpan.FromSeconds(10));

            if (networks.Contains(HOME_SSID))
                LocalSync();
            else
                RemoteSync();
        }

        private static void RemoteSync()
        {
            Notification.Display($"Using Remote Sync! 4G UP!","alarm",TimeSpan.FromSeconds(10));
            Shell.Execute("nmcli", "radio wwan on");
            Shell.Execute("nmcli", $"connection up {LTE_PROFILE}");
            var output = Shell.GetValue("rsync", "-avh trbl@trbl.her.st:/mnt/SDA/SyncMusic/ /home/alarm/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}","alarm",TimeSpan.FromSeconds(10));
        }

        private static void LocalSync()
        {
            Notification.Display($"Using Local Sync! WIFI CONNECT {HOME_SSID}!","alarm",TimeSpan.FromSeconds(10));
            WiFi.Connect(HOME_SSID);
            var output = Shell.GetValue("rsync", "-avh trbl@192.168.0.2:/mnt/SDA/SyncMusic/ /home/alarm/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}","alarm",TimeSpan.FromSeconds(10));
        }
    }
}
