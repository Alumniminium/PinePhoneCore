using System;
using PinePhoneCore.Devices;

using System.Threading;

namespace pplightd
{
    class Program
    {
        //Available IntegrationTimes: 0.000185 0.000370 0.000741 0.001480 0.002960 0.005920 0.011840 0.023680 0.047360 0.094720 0.189440 0.378880 0.757760 1.515520 3.031040 6.062080
        //Available Scales: 6.4 1.6 0.4 0.1

        static void Main(string[] args)
        {
            AmbientLightSensor.IntegrationTime = 0.757760f;
            AmbientLightSensor.Scale = 0.1f;
            var last = 0;

            while (true)
            {
                Thread.Sleep((int)(AmbientLightSensor.IntegrationTime * 1000));

                var lux = (int)AmbientLightSensor.Luminance;
                var brightness = 0;
                if (lux < 1)
                    brightness = 120;
                else if (lux < 100)
                    brightness = 200;
                else if (lux < 300)
                    brightness = 300;
                else if (lux < 600)
                    brightness = 400;
                else if (lux < 900)
                    brightness = 500;
                else if (lux < 1500)
                    brightness= 600;
                else if (lux < 2000)
                   brightness = 700;
                else if (lux < 3000)
                    brightness = 800;
                else if (lux < 4000)
                    brightness= 900;
                else
                   brightness = 1000;

                if (last != brightness)
                {
                    last = brightness;
                    Fade(brightness);
                }
            }
        }

        public static void Fade(int to)
        {
            while(Display.Brightness < to)
            {
                Console.WriteLine($"Lux: {AmbientLightSensor.Luminance} Brightness: {Display.Brightness}");
                Display.Brightness++;
                Thread.Sleep(16);
            }
            
            while(Display.Brightness > to)
            {
                Console.WriteLine($"Lux: {AmbientLightSensor.Luminance} Brightness: {Display.Brightness}");
                Display.Brightness--;
                Thread.Sleep(16);
            }
        }
    }
}
