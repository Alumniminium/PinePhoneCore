using System;
using System.Reflection;
using System.Threading.Tasks;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;
using PinePhoneCore.PinePhoneHelpers;

namespace ppmusicsyncd
{
    class Program
    {
        private const string HOME_SSID = ".fsociety";
        private const string LTE_PROFILE = "PublicIP";

        static async Task Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");
            await Dependencies.TestDependencies();

            var currentWifiState = PinePhoneWiFi.IsEnabled();

            Console.WriteLine("syncing..");
            Notification.Display("Synchronizing Music!", Environment.UserName, TimeSpan.FromSeconds(10));
            if (!currentWifiState)
            {
                Console.WriteLine("wifi on..");
                PinePhoneWiFi.Toggle(true);
            }

            var currentSSID = PinePhoneWiFi.GetCurrentSSID();

            if (currentSSID != HOME_SSID)
            {
                Notification.Display("You're not connected to your home network. Leaving wifi alone.", Environment.UserName, TimeSpan.FromSeconds(10));
                RemoteSync();
                return;
            }
            if (string.IsNullOrEmpty(currentSSID))
            {
                Notification.Display("Let's see where we are... scanning wifi networks", Environment.UserName, TimeSpan.FromSeconds(10));
                Console.WriteLine("scan..");
                var networks = PinePhoneWiFi.Scan();
                foreach (var net in networks)
                    Console.WriteLine(net);
                Notification.Display($"Found {networks.Count} networks...", Environment.UserName, TimeSpan.FromSeconds(10));

                if (networks.Contains(HOME_SSID))
                    LocalSync();
                else
                    RemoteSync();
            }
            if(!currentWifiState)
                PinePhoneWiFi.Toggle(false);
        }

        private static void RemoteSync()
        {
            Notification.Display($"Using Remote Sync! 4G UP!", Environment.UserName, TimeSpan.FromSeconds(10));
            Shell.Execute("nmcli", "radio wwan on");
            Shell.Execute("nmcli", $"connection up {LTE_PROFILE}");
            var output = Shell.GetValue("rsync", $"-avh trbl@trbl.her.st:/mnt/SDA/SyncMusic/ /home/{Environment.UserName}/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}", Environment.UserName, TimeSpan.FromSeconds(10));
        }

        private static void LocalSync()
        {
            Notification.Display($"Using Local Sync! WIFI CONNECT {HOME_SSID}!", Environment.UserName, TimeSpan.FromSeconds(10));
            WiFi.Connect(HOME_SSID);
            var output = Shell.GetValue("rsync", $"-avh trbl@192.168.0.2:/mnt/SDA/SyncMusic/ /home/{Environment.UserName}/music/");
            Notification.Display($"Synchronized Music!{Environment.NewLine}{output}", Environment.UserName, TimeSpan.FromSeconds(10));
        }
    }
}
