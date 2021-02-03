using System;
using System.Text;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{

    public static class HardwareButtons
    {
        public static DevInputEventMonitor VolumeButtonMonitor = new DevInputEventMonitor("/dev/input/event3");
        public static DevInputEventMonitor PowerButtonMonitor = new DevInputEventMonitor("/dev/input/event0");

        public static Action<bool> OnVolumeDownKeyStateChanged;
        public static Action<bool> OnVolumeUpKeyStateChanged;
        public static Action<bool> OnPowerKeyStateChanged;
        public static Action<HardwareButton,bool> OnVolumeKeyStateChanged;
        public static Action<HardwareButton,bool> OnKeyStateChanged;

        public static bool PowerKeyDown => PowerButtonMonitor.LastEvent.Value == 1;
        public static bool VolumeDownKeyDown => VolumeButtonMonitor.LastEvent.Value == 1;
        public static bool VolumeUpKeyDown => VolumeButtonMonitor.LastEvent.Value == 1;

        static HardwareButtons()
        {
            PowerButtonMonitor.OnData = Handler;
            VolumeButtonMonitor.OnData = Handler;
        }

        private static void Handler(NativeInputEvent inputEvent)
        {
            // button, state, IsVolumeKey
            var (b,s,v) = ((HardwareButton)inputEvent.Code,inputEvent.Value==1,inputEvent.Code < 116);
            
            switch (b)
            {
                case HardwareButton.Power:
                    OnPowerKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
                case HardwareButton.VolumeUp:
                    OnVolumeUpKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
                case HardwareButton.VolumeDown:
                    OnVolumeDownKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
            }
            if (v)
                OnVolumeKeyStateChanged?.Invoke(b,s);

            OnKeyStateChanged?.Invoke(b,s);
        }


        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(HardwareButtons).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }

    }
}