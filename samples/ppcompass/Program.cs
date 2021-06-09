using System;
using PinePhoneCore;
using PinePhoneCore.Devices;

namespace ppcompass
{
    class Program
    {
        /// My magnetometer isnt working properly..
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var rawX = Magnetometer.RawX;
            var rawY = Magnetometer.RawY;
            var rawZ = Magnetometer.RawZ;
            Console.ReadLine();
            var deltaX = rawX - Magnetometer.RawX;
            var deltaY = rawY - Magnetometer.RawY;
            var deltaZ = rawZ - Magnetometer.RawZ;

            while (true)
            {
                var x = Magnetometer.RawX - deltaX;
                var y = Magnetometer.RawY - deltaY;
                var z = Magnetometer.RawZ - deltaZ;
                var heading = 180 * Math.Atan2(y, x) / Math.PI;
                if(heading < 0)
                    heading += 360;
                
                Console.WriteLine($"[{Magnetometer.RawX}, {Magnetometer.RawY}, {Magnetometer.RawZ}],");
                Console.WriteLine($"Heading: {heading}");
            }
        }
    }
}
