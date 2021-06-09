using System;
using PinePhoneCore.Devices;
using System.Threading;
using System.Reflection;

namespace ppproximityd
{
    class Program
    {
        static void Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");

            ProximitySensor.IntegrationTime = ProximitySensor.INTEGRATION_TIME_0094720;
            ProximitySensor.Gain = ProximitySensor.GAIN_x1;

            bool off = false;
            while (true)
            {
                var closeness = ProximitySensor.Proximity;
                if (closeness == ushort.MaxValue && !off)
                {
                    off = true;
                    Display.PowerOn = false;
                    Digitizer.Enabled = false;
                    for (int i = 1; i < SoC.CpuCores.Length; i++)
                    {
                        SoC.CpuCores[i].Governor = "userspace";
                        SoC.CpuCores[i].Frequency = 100;
                        SoC.CpuCores[i].Enabled = false;
                    }
                    PinePhoneCore.Helpers.Shell.Execute("pkill", "-STOP conky");
                    PinePhoneCore.Helpers.Shell.Execute("pkill", "-STOP pplightd");
                }
                else if (closeness < 50000 && off)
                {
                    off = false;
                    Display.PowerOn = true;
                    Digitizer.Enabled = true;

                    for (int i = 1; i < SoC.CpuCores.Length; i++)
                    {
                        SoC.CpuCores[i].Governor = "conservative";
                        SoC.CpuCores[i].Enabled = true;
                    }
                    PinePhoneCore.Helpers.Shell.Execute("pkill", "-CONT conky");
                    PinePhoneCore.Helpers.Shell.Execute("pkill", "-CONT pplightd");
                }
                Console.WriteLine($"Distance: {closeness}, Backlight: {Display.PowerOn}");
                Thread.Sleep(500);
            }
        }
    }
}
