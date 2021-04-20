using System;
using System.Text;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public static class Digitizer
    {
        static DevInputEventMonitor Monitor = new DevInputEventMonitor("/dev/input/event1");
        public static DigitizerState[] CurrentStates;
        public static int CurrentFingerIndex;

        public static Action<NativeInputEvent> OnEvent;
        public static Action<DigitizerState> OnPositionChanged;
        public static Action<DigitizerState> OnPositionXChanged;
        public static Action<DigitizerState> OnPositionYChanged;
        public static Action<DigitizerState> OnStopTouching;
        public static Action<DigitizerState> OnTouching;
        public static Action<int> OnFingerAdded;
        public static Action<int> OnFingerRemoved;

        static Digitizer()
        {
            CurrentStates = new DigitizerState[10];
            Monitor.OnData = Handler;
        }

        private static void Handler(NativeInputEvent newState)
        {
            OnEvent?.Invoke(newState);

            switch (newState.Code)
            {
                case 47:
                    CurrentFingerIndex = (int)newState.Value;
                    CurrentStates[CurrentFingerIndex].FingerIndex = (byte)newState.Value;
                    break;
                case 57:
                    CurrentStates[CurrentFingerIndex].FingerDown = ((int)newState.Value) != -1;

                    if (!CurrentStates[CurrentFingerIndex].FingerDown)
                        OnFingerRemoved?.Invoke(CurrentFingerIndex);
                    else
                        OnFingerAdded?.Invoke(CurrentFingerIndex);
                    break;
                case 53:
                    CurrentStates[CurrentFingerIndex].X = (ushort)newState.Value;
                    OnPositionChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
                    OnPositionXChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
                    break;
                case 54:
                    CurrentStates[CurrentFingerIndex].Y = (ushort)newState.Value;
                    OnPositionChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
                    OnPositionYChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
                    break;
                case 330:
                    CurrentStates[CurrentFingerIndex].FingerDown = newState.Value == 1;

                    if (CurrentStates[CurrentFingerIndex].FingerDown)
                        OnTouching?.Invoke(CurrentStates[CurrentFingerIndex]);
                    else
                        OnStopTouching?.Invoke(CurrentStates[CurrentFingerIndex]);
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