using System;
using PinePhoneCore;
using PinePhoneCore.Devices;

namespace ppcompass
{
    class Program
    {
        public static short XMin = -258;
        public static short XMax = 1974;

        public static short YMin = -1770;
        public static short YMax = 144;

        public static short ZMin = -1437;
        public static short ZMax = 1907;
        //public static int[] MAX_VALUES = { short.MinValue,short.MinValue,short.MinValue };
        //public static int[] MIN_VALUES = { short.MaxValue,short.MaxValue,short.MaxValue }; 
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

            // while(true)
            // {
            //     var rawX = Magnetometer.RawX;
            //     var rawY = Magnetometer.RawY;
            //     var rawZ = Magnetometer.RawZ;

            //     MIN_VALUES[0] = Math.Min(MIN_VALUES[0], rawX);
            //     MIN_VALUES[1] = Math.Min(MIN_VALUES[1], rawY);
            //     MIN_VALUES[2] = Math.Min(MIN_VALUES[2], rawZ);

            //     MAX_VALUES[0] = Math.Max(MAX_VALUES[0], rawX);
            //     MAX_VALUES[1] = Math.Max(MAX_VALUES[1], rawY);
            //     MAX_VALUES[2] = Math.Max(MAX_VALUES[2], rawZ);

            //     Console.WriteLine($"MIN_VALUESS: {MIN_VALUES[0]},{MIN_VALUES[1]},{MIN_VALUES[2]}");
            //     Console.WriteLine($"MAX_VALUESS: {MAX_VALUES[0]},{MAX_VALUES[1]},{MAX_VALUES[2]}");
            // }


            while (true)
            {
                // float scaleX = 1 / (XMax - XMin);
                // float scaleY = 1 / (YMax - YMin);
                // float scaleZ = 1 / (ZMax - ZMin);
                var x = Magnetometer.RawX - deltaX;
                var y = Magnetometer.RawY - deltaY;
                var z = Magnetometer.RawZ - deltaZ;
                var heading = 180 * Math.Atan2(y, x) / Math.PI;
                if(heading < 0)
                    heading += 360;
                
                Console.WriteLine($"[{Magnetometer.RawX}, {Magnetometer.RawY}, {Magnetometer.RawZ}],");
                //Console.WriteLine($"RawX: {rawX}");
                //Console.WriteLine($"RawY: {rawY}");
                //Console.WriteLine($"RawZ: {rawZ}");
                Console.WriteLine($"Heading: {heading}");
            }
        }
    }
}
