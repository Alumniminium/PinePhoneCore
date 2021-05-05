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
            HeadphoneJack.OnPluggedIn = (k) =>
            {
                PinePhoneAudio.SetVolumeFor(AudioDevice.Headphones,45);
                PinePhoneAudio.SwitchToHeadset();
            };
            HeadphoneJack.OnPluggedOut = (k) =>
            {
                PinePhoneAudio.SetVolumeFor(AudioDevice.Speakers,45);
                PinePhoneAudio.SwitchToSpeakers();
            };
            Console.WriteLine($"{self.GetName().Name} {self.GetName().Version} running.");
            Console.WriteLine($"Press any key to exit...");
            Console.ReadKey();
        } 
    }
}
