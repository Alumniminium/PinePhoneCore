using System;
using System.Text;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public static class Digitizer
    {
        static DevInputEventMonitor Monitor = new DevInputEventMonitor("/dev/input/event1");
        static DigitizerState[] LastState;
        static int LastFingerIndex;
        
        public static Action<NativeInputEvent> OnEvent;
        public static Action<DigitizerState> OnPositionChanged;
        
        static Digitizer()
        {
            LastState = new DigitizerState[10];
            Monitor.OnData = Handler;
        }

        private static void Handler(NativeInputEvent inputEvent)
        {
            OnEvent?.Invoke(inputEvent);

            switch (inputEvent.Code)
            {
                case 47:
                    LastFingerIndex = (int)inputEvent.Value;
                    LastState[LastFingerIndex].FingerIndex = (byte)inputEvent.Value;
                    break;
                case 53: 
                    LastState[LastFingerIndex].X = (ushort)inputEvent.Value;
                    OnPositionChanged?.Invoke(LastState[LastFingerIndex]);
                    break;
                case 54: 
                    LastState[LastFingerIndex].Y = (ushort)inputEvent.Value;
                    OnPositionChanged?.Invoke(LastState[LastFingerIndex]);
                    break;
                case 330: 
                    LastState[LastFingerIndex].FingerDown = inputEvent.Value == 1;
                    OnPositionChanged?.Invoke(LastState[LastFingerIndex]);
                    break;
            }
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