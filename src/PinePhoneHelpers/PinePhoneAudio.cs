using System.Collections.Generic;
using PinePhoneCore.Enums;

namespace PinePhoneCore.Helpers
{
    public static class PinePhoneAudio
    {
        static Dictionary<AudioDevice, string> AudioDeviceLookup = new Dictionary<AudioDevice, string>();
        public static void MuteOutputDevice(AudioDevice device, bool mute = false)
        {
            var dev = AudioDeviceLookup[device];
            Shell.Execute("amixer",$"sset \"{dev}\" {(mute ? "mute" : "unmute")}");
        }
        public static void SetVolumeFor(AudioDevice device, int percent)
        {
            var dev = AudioDeviceLookup[device];
            Shell.Execute("amixer",$"sset \"{dev}\" {percent}%");
        }

        public static void SwitchToSpeakers()
        {
            MuteOutputDevice(AudioDevice.Headphones,true);  
            MuteOutputDevice(AudioDevice.Earpiece,true);   
            MuteOutputDevice(AudioDevice.Speakers,false);   
        }
        public static void SwitchToEarpiece()
        {
            MuteOutputDevice(AudioDevice.Headphones,true);  
            MuteOutputDevice(AudioDevice.Earpiece,false);   
            MuteOutputDevice(AudioDevice.Speakers,true);   
        }
        public static void SwitchToHeadset()
        {
            MuteOutputDevice(AudioDevice.Headphones,false);  
            MuteOutputDevice(AudioDevice.Earpiece,true);   
            MuteOutputDevice(AudioDevice.Speakers,true);   
        }
    }
}