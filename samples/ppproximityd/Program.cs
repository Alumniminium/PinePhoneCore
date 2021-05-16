using System;
using PinePhoneCore.Devices;
using System.Threading;

namespace ppproximityd
{
    class Program
    {
        static void Main(string[] args)
        {
            ProximitySensor.IntegrationTime = 0.094720f;
            ProximitySensor.Scale = 0.1f;

            bool off = false;
            while(true)
            {
                var distance = ProximitySensor.ProximityRaw;
                if(distance > 30000 && !off)
                {
                    off = true;
                    Display.PowerOn = false;
                    Digitizer.Enabled = false;
                    PinePhoneCore.Helpers.Shell.Execute("pkill","-STOP conky");
                }
                else if (distance < 30000 && off)
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
