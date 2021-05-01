using System;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace ppjackd
{
    class Program
    {
        public static string HookScript;

        static void Main(string[] args)
        {
            HookScript = CliArgs.Assign(args, "--hook", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/sxmo/hooks/headphonejack");
            HeadphoneJack.OnPlugged = Plugged;

            while (true)
                Thread.Sleep(int.MaxValue);
        }
        
        private static void Plugged(HeadphoneKind kind)
        {
            Shell.Execute($"sh", $"-c \"{HookScript} {HeadphoneJack.Connected}\"");
        }
    }
}
