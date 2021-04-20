using System;
using System.Numerics;
using System.Text;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public static class Digitizer
    {
        static DevInputEventMonitor Monitor = new DevInputEventMonitor("/dev/input/event1");
        static DigitizerState[] CurrentStates;
        static DigitizerState[] InitialStates;
        static int CurrentFingerIndex;

        public static Action<TouchGesture> OnGesture;
        public static Action<NativeInputEvent> OnEvent;
        public static Action<DigitizerState> OnPositionChanged;
        public static Action<DigitizerState> OnStopTouching;
        public static Action<DigitizerState> OnTouching;
        public static Action<int> OnFingerAdded;
        public static Action<int> OnFingerRemoved;

        static Digitizer()
        {
            CurrentStates = new DigitizerState[10];
            InitialStates = new DigitizerState[10];
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
                    {
                        OnFingerRemoved(CurrentFingerIndex);
                        var gesture = DetectGesture(CurrentFingerIndex);
                        if (gesture != TouchGesture.None)
                            OnGesture(gesture);
                    }
                    else
                    {
                        InitialStates[CurrentFingerIndex] = new DigitizerState { X = 0, Y = 0 };
                        OnFingerAdded(CurrentFingerIndex);
                    }
                    break;
                case 53:
                    CurrentStates[CurrentFingerIndex].X = (ushort)newState.Value;
                    if (InitialStates[CurrentFingerIndex].X == 0)
                        InitialStates[CurrentFingerIndex].X = (ushort)newState.Value;
                    OnPositionChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
                    break;
                case 54:
                    CurrentStates[CurrentFingerIndex].Y = (ushort)newState.Value;
                    if (InitialStates[CurrentFingerIndex].Y == 0)
                        InitialStates[CurrentFingerIndex].Y = (ushort)newState.Value;

                    OnPositionChanged?.Invoke(CurrentStates[CurrentFingerIndex]);
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

        public static TouchGesture DetectGesture(int fingerIdx)
        {
            var start = InitialStates[fingerIdx];
            var end = CurrentStates[fingerIdx];

            var startV = new Vector2(start.X, start.Y);
            var endV = new Vector2(end.X, end.Y);

            var distance = Vector2.Distance(startV,endV);

            if(distance < 100)
                return TouchGesture.None;

            var dir = Vector2.Normalize(endV - startV);
            var x = (int)Math.Round(dir.X, 0, MidpointRounding.ToEven);
            var y = (int)Math.Round(dir.Y, 0, MidpointRounding.ToEven);

            if ((x, y) == (-1, 0))
                return TouchGesture.SwipeLeft;
            if ((x, y) == (0, -1))
                return TouchGesture.SwipeUp;
            if ((x, y) == (1, 0))
                return TouchGesture.SwipeRight;
            if ((x, y) == (0, 1))
                return TouchGesture.SwipeDown;
            if ((x, y) == (-1, -1))
                return TouchGesture.SwipeUpLeft;
            if ((x, y) == (1, -1))
                return TouchGesture.SwipeUpRight;
            if ((x, y) == (1, 1))
                return TouchGesture.SwipeDownRight;
            if ((x, y) == (-1, 1))
                return TouchGesture.SwipeDownLeft;

            return TouchGesture.None;
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