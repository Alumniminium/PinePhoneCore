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

            ProximitySensor.IntegrationTime = 0.094720f;
            ProximitySensor.Gain = 0.1f;

            bool off = false;
            while(true)
            {
                var closeness = ProximitySensor.Proximity;
                if(closeness == ushort.MaxValue && !off)
                {
                    off = true;
                    Display.PowerOn = false;
                    Digitizer.Enabled = false;
                    PinePhoneCore.Helpers.Shell.Execute("pkill","-STOP conky");
                }
                else if (closeness < 50000 && off)
                {
                    off = false;
                    Display.PowerOn = true;
                    Digitizer.Enabled = true;
                    PinePhoneCore.Helpers.Shell.Execute("pkill","-CONT conky");
                }
                //Console.WriteLine($"Distance: {distance}, Backlight: {Display.PowerOn}");
                Thread.Sleep(250);
            }
        }
    }
}
