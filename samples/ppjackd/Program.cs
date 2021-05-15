using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace ppjackd
{
    class Program
    {
        public static List<string> SuspendList = new List<string>
        {
            "mpv",
            "lollypop"
        };
        static void Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");

            HeadphoneJack.OnPluggedIn = (k) =>
            {
                PinePhoneAudio.SetVolumeFor(AudioDevice.Headphones, 55); // dont blow my ears out
                PinePhoneAudio.SwitchToHeadset(); // Mutes other outputs
                ResumeProcesses();
                Notification.Display($"Headphones Plugged in!{Environment.NewLine}Resumed mpv!", Environment.UserName, TimeSpan.FromSeconds(10));
            };
            HeadphoneJack.OnPluggedOut = (k) =>
            {
                SuspendProcesses();
                PinePhoneAudio.SwitchToSpeakers(); // Mutes other outputs
                Notification.Display($"Headphones Removed!{Environment.NewLine}Suspended mpv!", Environment.UserName, TimeSpan.FromSeconds(10));
            };

            Console.WriteLine($"Press any key to exit...");
            while (true)
                Thread.Sleep(int.MaxValue);
        }

        public static void SuspendProcesses()
        {
            foreach(var process in SuspendList)
                Shell.Execute("pkill", $"-STOP {process}");
        }
        public static void ResumeProcesses()
        {
            foreach(var process in SuspendList)
                Shell.Execute("pkill", $"-CONT {process}");
        }
    }
}
