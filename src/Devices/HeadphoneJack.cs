using System;
using System.Text;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public static class HeadphoneJack
    {
        public static DevInputEventMonitor Monitor = new DevInputEventMonitor("/dev/input/event5");
        public static Action<HeadphoneKind> OnPluggedIn;
        public static Action<HeadphoneKind> OnPluggedOut;
        public static Action<NativeInputEvent> OnPluggedRaw;
        public static Action<HeadphoneKind> OnPlugged;

        public static bool Connected => Monitor.LastEvent.Value == 1;
        public static HeadphoneKind Kind => (HeadphoneKind)Monitor.LastEvent.Code;

        static HeadphoneJack()
        {
            Monitor.OnData = Handler;
        }

        private static void Handler(NativeInputEvent inputEvent)
        {
            OnPluggedRaw?.Invoke(inputEvent);
            OnPlugged?.Invoke(Kind);
            
            if (inputEvent.Code == 2 && inputEvent.Value == 1)
                OnPluggedIn?.Invoke(Kind);
            else if (inputEvent.Code == 2 && inputEvent.Value == 0)
                OnPluggedOut?.Invoke(Kind);
        }


        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(HeadphoneJack).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}