using System;
using PacketDotNet.Ieee80211;
using SharpPcap;

namespace wifimon
{
    class Program
    {

        public static BeaconMonitor BeaconMonitor;
        public static ProbeMonitor ProbeMonitor;
        public static ICaptureDevice Device;

        static void Main(string[] args)
        {
            Device = FindDevice();
            try
            {
                MonitoringMode(true);
                Console.WriteLine($"-- Listening on { Device.Name}, hit 'Enter' to stop...");
                Console.ReadLine();
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { MonitoringMode(false); }
        }

        private static ICaptureDevice FindDevice()
        {
            foreach (var dev in CaptureDeviceList.Instance)
                if (dev.Name.Contains("wlan") || dev.Name.Contains("wlp"))
                    return dev;
            return null;
        }
        private static async void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            try
            {
                var packet = (RadioPacket)e.Packet.GetPacket();
                var beaconFrame = packet.Extract<BeaconFrame>();
                var probeRequest = packet.Extract<ProbeRequestFrame>();

                if (beaconFrame != null)
                    await BeaconMonitor.HandlePacket(packet, beaconFrame);

                if (probeRequest != null)
                    await ProbeMonitor.HandlePacket(packet, probeRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void MonitoringMode(bool on = true)
        {
            if (on)
            {
                Console.WriteLine($"-- Enabling Monitor mode on {Device.Name}");
                var wifiOffResult = CliWrap.Cli.Wrap("nmcli").WithArguments($"radio wifi off").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Disabling the WiFi chip took {wifiOffResult.RunTime.TotalMilliseconds}ms");
                var wifiOnResult = CliWrap.Cli.Wrap("nmcli").WithArguments($"radio wifi on").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Enabling the WiFi chip took {wifiOnResult.RunTime.TotalMilliseconds}ms");
                var wwanOnResult = CliWrap.Cli.Wrap("nmcli").WithArguments($"radio wwan on").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Enabling the modem took {wwanOnResult.RunTime.TotalMilliseconds}ms");
                var lteUpResult = CliWrap.Cli.Wrap("nmcli").WithArguments($"connection PublicIP up").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Connecting to 4G took {lteUpResult.RunTime.TotalMilliseconds}ms");
                var devDownResult = CliWrap.Cli.Wrap("ip").WithArguments($"link set {Device.Name} down").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Taking down {Device.Name} took {devDownResult.RunTime.TotalMilliseconds}ms");
                var modeResult = CliWrap.Cli.Wrap("iwconfig").WithArguments($"{Device.Name} mode Monitor").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Switching {Device.Name} to Monitor Mode took {modeResult.RunTime.TotalMilliseconds}ms");
                var setMacResult = CliWrap.Cli.Wrap("ip").WithArguments($"sudo ip link set dev {Device.Name} address DE:AD:BE:EF:13:37").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Setting {Device.Name} MAC to DE:AD:BE:EF:13:37 took {setMacResult.RunTime.TotalMilliseconds}ms");
                var devUpResult = CliWrap.Cli.Wrap("ip").WithArguments($"link set {Device.Name} up").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                Console.WriteLine($"-- Bringing up {Device.Name} took {devUpResult.RunTime.TotalMilliseconds}ms");
                BeaconMonitor = new BeaconMonitor("beacons.log");
                ProbeMonitor  = new ProbeMonitor("probes.log");
                Device.OnPacketArrival += device_OnPacketArrival;
                Device.Open(DeviceMode.Promiscuous, 2000, MonitorMode.Inactive, 16);
                Device.StartCapture();  

                Console.CancelKeyPress += (a, b) => MonitoringMode(false);
            }
            else
            {
                Console.WriteLine($"-- Disabling Monitor mode on {Device.Name}");
                Device.StopCapture();
                Console.WriteLine("-- Capture stopped.");
                CliWrap.Cli.Wrap("ip").WithArguments($"link set {Device.Name} down").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                CliWrap.Cli.Wrap("iwconfig").WithArguments($"{Device.Name} mode Managed").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                CliWrap.Cli.Wrap("ip").WithArguments($"sudo ip link set dev {Device.Name} address 12:7:00:00:00:01").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
                CliWrap.Cli.Wrap("ip").WithArguments($"link set {Device.Name} up").WithValidation(CliWrap.CommandResultValidation.None).ExecuteAsync().GetAwaiter().GetResult();
            }
        }
    }
}