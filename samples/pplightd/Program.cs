using System;
using PinePhoneCore.Devices;

using System.Threading;

namespace pplightd
{
    class Program
    {
//        Available IntegrationTimes: 0.000185 0.000370 0.000741 0.001480 0.002960 0.005920 0.011840 0.023680 0.047360 0.094720 0.189440 0.378880 0.757760 1.515520 3.031040 6.062080
//Available Scales: 6.4 1.6 0.4 0.1

        static void Main(string[] args)
        {
            AmbientLightSensor.IntegrationTime = 0.757760f; 
            AmbientLightSensor.Scale = 0.1f;
            while(true)
            {
                Thread.Sleep((int)(AmbientLightSensor.IntegrationTime * 1000));
                
                var lux = AmbientLightSensor.Luminance;
                if(lux == 0)
                    Display.Brightness = 100;//Display.PowerOn = false;
                else if(lux < 100)
                    Display.Brightness = 200;
                else if(lux < 300)
                    Display.Brightness = 300;
                else if (lux < 600)
                    Display.Brightness = 400;
                else if (lux < 900)
                    Display.Brightness = 500;
                else if (lux < 1500)
                    Display.Brightness = 600;
                else if (lux < 2000)
                    Display.Brightness = 700;
                else if (lux < 3000)
                    Display.Brightness=800;
                else if (lux < 4000)
                    Display.Brightness = 900;
                else
                    Display.Brightness = 1000;
            
                Console.WriteLine($"Lux: {AmbientLightSensor.Luminance} Brightness: {Display.Brightness}");
            }
        }
    }
}
