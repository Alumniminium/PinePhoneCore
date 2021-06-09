using System;
using PinePhoneCore.Devices;

using System.Threading;

namespace pplightd
{
    class Program
    {
        public const int LUX_MAX = 3500;

        static void Main(string[] args)
        {
            AmbientLightSensor.ApplyPreset(AmbientLightSensor.Preset100ms);

            var last = 0;

            while (true)
            {
                var raw = (int)AmbientLightSensor.LuminanceRaw;
                var lux = (int)AmbientLightSensor.Lux;
                var p = 10 + (raw * 100 / LUX_MAX);
                
                if(p < 30)
                    p+=10;
                else
                    p+=20;

                var brightness = Math.Min(p * 10, 1000);
         
                if (last != brightness)
                {
                    last = brightness;
                    Display.Brightness = brightness;
                    //Fade(brightness);
                }
                Thread.Sleep(1000);   
                Console.WriteLine($"Lux: {lux}, Raw: {raw} Brightness: {Display.Brightness} P: {p}");
            }
        }

        public static void Fade(int to)
        {
            while(Display.Brightness != to)
            {
                if(Display.Brightness < to)
                    Display.Brightness+=10;
                else
                    Display.Brightness-=10;
                
                Thread.Sleep(10);
            }
        }
    }
}
