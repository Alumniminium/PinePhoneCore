using System;
using System.Reflection;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace ppjackd
{
    class Program
    {
        static void Main(string[] args)
        {
            var self = Assembly.GetExecutingAssembly();
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");

            HeadphoneJack.OnPluggedIn = (k) =>
            {
                PinePhoneAudio.SetVolumeFor(AudioDevice.Headphones, 55); // dont blow my ears out
                PinePhoneAudio.SwitchToHeadset(); // Mutes other outputs
                Shell.Execute("pkill", "-CONT mpv"); // resume mpv if its suspended, my music player of choice.
                Notification.Display($"Headphones Plugged in!{Environment.NewLine}Resumed mpv!", Environment.UserName, TimeSpan.FromSeconds(10));
            };
            HeadphoneJack.OnPluggedOut = (k) =>
            {
                Shell.Execute("pkill", "-STOP mpv"); // Suspend mpv before switching on speakers!
                PinePhoneAudio.SwitchToSpeakers(); // Mutes other outputs
                Notification.Display($"Headphones Removed!{Environment.NewLine}Suspended mpv!", Environment.UserName, TimeSpan.FromSeconds(10));
            };

            Console.WriteLine($"Press any key to exit...");
            Console.ReadKey();
        }
    }
}
