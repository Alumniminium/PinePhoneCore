using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CliWrap;

namespace ppwake
{
    public static class NetworkManager
    {
        public static async Task ToggleWiFi(bool on)
        {
            var state = on ? "on" : "off";
            Console.WriteLine($"Turning {state} wifi");
            await Cli.Wrap("nmcli")
                    .WithArguments($"radio wifi {state}")
                    .ExecuteAsync();
        }
        public static async Task Toggle4G(bool on)
        {
            var state = on ? "on" : "off";
            Console.WriteLine($"Turning {state} 4G");
            await Cli.Wrap("nmcli")
                    .WithArguments($"radio wwan {state}")
                    .ExecuteAsync();
        }
        public static async Task<List<string>> ScanWiFi()
        {
            var scanResults = new StringBuilder();
            var networks = new List<string>();

            Console.WriteLine("Scanning...");
            await Cli.Wrap("nmcli")
                    .WithArguments("-f SSID dev wifi")
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(scanResults))
                    .ExecuteAsync();
            
            foreach (var net in scanResults.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                networks.Add(net.Trim());

            return networks;
        }

        public static async Task Connect(string name)
        {
            Console.WriteLine("Connecting 4G");
            await Cli.Wrap("nmcli")
                .WithArguments($"connection up {name}")
                .ExecuteAsync();
        }
    }
}