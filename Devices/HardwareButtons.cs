using System;
using System.Text;
using PinePhoneCore.Enums;
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
        public static Action<ButtonState> OnVolumeKeyStateChanged;
        public static Action<ButtonState> OnKeyStateChanged;

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
            var buttonState = new ButtonState();
            var volDirty = inputEvent.Code < 116;
            
            switch (inputEvent.Code)
            {
                case 116:
                    buttonState.PowerKeyDown = (inputEvent.Value == 1);
                    OnPowerKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
                case 115:
                    buttonState.VolumeUpKeyDown = (inputEvent.Value == 1);
                    OnVolumeUpKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
                case 114:
                    buttonState.VolumeDownKeyDown = (inputEvent.Value == 1);
                    OnVolumeDownKeyStateChanged?.Invoke(inputEvent.Value == 1);
                    break;
            }
            if (volDirty)
                OnVolumeKeyStateChanged?.Invoke(buttonState);

            OnKeyStateChanged?.Invoke(buttonState);
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