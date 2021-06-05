using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PinePhoneCore.Devices;

namespace ppstepd
{
    public static class StepDetector
    {
        public static int Steps = 0;
        public static Thread MonitorThread;
        public static List<int> SamplesXv = new List<int>();
        public static List<int> SamplesYv = new List<int>();
        public static List<int> SamplesZv = new List<int>();

        public static List<int> SamplesX = new List<int>();
        public static List<int> SamplesY = new List<int>();
        public static List<int> SamplesZ = new List<int>();


        public static void Start()
        {
            MonitorThread = new Thread(Monitor);
            MonitorThread.IsBackground = true;
            MonitorThread.Start();
        }
        
        public static void Monitor()
        {
            while (true)
            {
                for(int i = 0; i < 125; i++)
                {
                    SamplesXv.Add((int)Accelerometer.AngularVelocityRawX);
                    SamplesYv.Add((int)Accelerometer.AngularVelocityRawY);
                    SamplesZv.Add((int)Accelerometer.AngularVelocityRawZ);
                    SamplesX.Add((int)Accelerometer.RawX);
                    SamplesY.Add((int)Accelerometer.RawY);
                    SamplesZ.Add((int)Accelerometer.RawZ);
                    Thread.Sleep(1);
                }
                
                var avgXv = SamplesXv.Sum() / SamplesXv.Count;
                var avgX = SamplesX.Sum() / SamplesX.Count;
                SamplesXv.Clear();
                SamplesX.Clear();
                
                var avgYv = SamplesYv.Sum() / SamplesYv.Count;
                var avgY = SamplesY.Sum() / SamplesY.Count;
                SamplesYv.Clear();
                SamplesY.Clear();
                
                var avgZv = SamplesZv.Sum() / SamplesZv.Count;
                var avgZ = SamplesZ.Sum() / SamplesZ.Count;
                SamplesZv.Clear();
                SamplesZ.Clear();

                ProcessSample(avgXv,avgYv,avgZv,avgX,avgY,avgZ);
            }
        }

        private static void ProcessSample(int avgXv, int avgYv, int avgZv,int avgX, int avgY, int avgZ)
        {
            var total_Gravity = Math.Abs(avgX) + Math.Abs(avgY) + Math.Abs(avgZ);
            var total_Velocity = Math.Abs(avgXv) + Math.Abs(avgYv) + Math.Abs(avgZv);
            
            if(total_Velocity > 500)
            {
                Steps++;
                Console.WriteLine(Steps);
            }  
            Console.WriteLine($"######### V:{total_Velocity} #########");
            Console.WriteLine($"AvgXv: {avgXv}");
            Console.WriteLine($"AvgYv: {avgYv}");
            Console.WriteLine($"AvgZv: {avgZv}");    
            Console.WriteLine($"######### G:{total_Gravity} #########");
            Console.WriteLine($"AvgX: {avgX}");
            Console.WriteLine($"AvgY: {avgY}");
            Console.WriteLine($"AvgZ: {avgZ}");        
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Accelerometer.CalibratePosition();
            Accelerometer.CalibrateAngVel();

            StepDetector.Start();
            while(true)
                Thread.Sleep(int.MaxValue);
        }

        public static int Max = 100;
        public static void WriteSmth(int x)
        {
            var pct = (int)((Math.Abs(x) * 100) / Max);
            pct = Math.Min(100, pct);
            pct = Math.Max(0, pct);
            pct = pct / 5;
            string line = "------------------------------------------";
            if (x < 0)
            {
                line = line.Remove(20, 1);
                line = line.Insert(20 - pct, "o");
            }
            else
            {
                line = line.Remove(20, 1);
                line = line.Insert(20 + pct, "o");
            }

            if(pct > 5 && x > 0)
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(line);
            }
            else if (pct > 5 && x < 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(line);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(line);
            }
        }
    }
}
