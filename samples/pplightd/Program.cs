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
                var lux = (int)AmbientLightSensor.LuminanceRaw;
              
                var p = 10 + (lux * 100 / LUX_MAX);
                
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
                   Console.WriteLine($"Lux: {lux} Brightness: {Display.Brightness} P: {p}");
                }
                Thread.Sleep(1000);
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
