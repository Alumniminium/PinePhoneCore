using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.PinePhoneHelpers;

namespace ppanalpower
{
    public static class Program
    {
        public const int SAMPLE_FREQUENCY_MS = 125;
        public const int SAMPLES_PER_ROUND = 64;
        public static Random Random = new Random(1337);
        
        public static Dictionary<int, List<int>> AveragesForBrightnessLevel = new Dictionary<int, List<int>>
        {
            [0] = new List<int>(),
            [100] = new List<int>(),
            [200] = new List<int>(),
            [300] = new List<int>(),
            [400] = new List<int>(),
            [500] = new List<int>(),
            [600] = new List<int>(),
            [700] = new List<int>(),
            [800] = new List<int>(),
            [900] = new List<int>(),
            [1000] = new List<int>(),
        };
        public static List<int> brightnesses = new List<int> { 0, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
        public static StreamWriter fs = new StreamWriter("power.log");

        public static (int, int) ConsolePosition = (0, 0);

        public static void Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");
            Display.Brightness = 1000;
            for (int i = 0;;i++)
            {
                if(i==0 || i % 10 == 0)
                    Console.WriteLine("    0%    10%   20%   30%   40%   50%   60%   70%   80%   90%   100%");
                    
                ConsolePosition = Console.GetCursorPosition();
                brightnesses.Shuffle();
                foreach (var brightness in brightnesses)
                    RunRounds(brightness);
                Console.WriteLine();
            }
        }

        private static void RunRounds(int brightness)
        {
            var samples = new List<int>();
            Display.Brightness = brightness;
            while (true)
            {
                samples.Add(PinePhoneBattery.GetChargeFlowMilliAmps());

                if (samples.Count == SAMPLES_PER_ROUND)
                {
                    var avg = samples.Sum() / samples.Count;
                    samples.Clear();

                    AveragesForBrightnessLevel[brightness].Add(avg);
                    Log(brightness, avg);
                    break;
                }
                Thread.Sleep(SAMPLE_FREQUENCY_MS);
            }
        }
        
        private static void Log(int brightness, int avg)
        {
            string _avgFor(int brightness) => (AveragesForBrightnessLevel[brightness].Sum() / Math.Max(1, AveragesForBrightnessLevel[brightness].Count)).ToString("000");

            Console.SetCursorPosition(ConsolePosition.Item1, ConsolePosition.Item2);
            Console.Write($"{AveragesForBrightnessLevel[brightness].Count}. {_avgFor(0)}mA {_avgFor(100)}mA {_avgFor(200)}mA {_avgFor(300)}mA {_avgFor(400)}mA {_avgFor(500)}mA {_avgFor(600)}mA {_avgFor(700)}mA {_avgFor(800)}mA {_avgFor(900)}mA {_avgFor(1000)}mA");

            fs.WriteLine($"{Display.Brightness},{avg},{_avgFor(brightness)} ({AveragesForBrightnessLevel[brightness].Count} samples)");
            fs.Flush();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            for(int i = list.Count;i>1;i--)
            {
                int k = Random.Next(i + 1);
                T value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
        }
    }
}
