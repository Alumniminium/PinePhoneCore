using System;
using System.Reflection;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace ppjackd
{
    class Program
    {
        public static string DotConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string HookScriptPath = DotConfigPath + "/sxmo/hooks/headphonejack";

        static void Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            HookScriptPath = CliArgs.Assign(args, "--hook", HookScriptPath);
            HeadphoneJack.OnPlugged = (k) => Console.WriteLine(Shell.GetValue($"sh", $"-c \"{HookScriptPath} {HeadphoneJack.Connected}\""));

            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");
            Console.WriteLine($"Hook Script: {HookScriptPath}");
            Console.WriteLine($"Press any key to exit...");
            Console.ReadKey();
        } 
    }
}
