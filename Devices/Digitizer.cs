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
        static Rectangle[] Zones;
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
            Zones = new Rectangle[4];
            Zones[0] = new Rectangle(0, 0, 720, 250);
            Zones[1] = new Rectangle(470, 100, 250, 1340);
            Zones[2] = new Rectangle(0, 1190, 720, 250);
            Zones[3] = new Rectangle(0, 100, 250, 1340);
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
                        OnFingerRemoved?.Invoke(CurrentFingerIndex);
                        var gesture = DetectGesture(CurrentFingerIndex);
                        if (gesture != TouchGesture.None)
                            OnGesture?.Invoke(gesture);
                    }
                    else
                    {
                        InitialStates[CurrentFingerIndex] = new DigitizerState { X = 0, Y = 0 };
                        OnFingerAdded?.Invoke(CurrentFingerIndex);
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

            var distance = Vector2.Distance(startV, endV);

            if (distance < 100)
                return TouchGesture.None;

            var dir = Vector2.Normalize(endV - startV);
            var x = (int)Math.Round(dir.X, 0, MidpointRounding.ToEven);
            var y = (int)Math.Round(dir.Y, 0, MidpointRounding.ToEven);
            var z = -1;

            for (int i = 0; i < Zones.Length; i++)
            {
                var zone = Zones[i];
                if (Zones[i].Contains(start.X, start.Y))
                {
                    z = i;
                    break;
                }
            }
            if (z == -1)
            {
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
            }
            if (z == 0) // top
            {
                if ((x, y) == (-1, 0))
                    return TouchGesture.TopEdgeSwipeLeft;
                if ((x, y) == (0, -1))
                    return TouchGesture.TopEdgeSwipeUp;
                if ((x, y) == (1, 0))
                    return TouchGesture.TopEdgeSwipeRight;
                if ((x, y) == (0, 1))
                    return TouchGesture.TopEdgeSwipeDown;
                if ((x, y) == (-1, -1))
                    return TouchGesture.TopEdgeSwipeUpLeft;
                if ((x, y) == (1, -1))
                    return TouchGesture.TopEdgeSwipeUpRight;
                if ((x, y) == (1, 1))
                    return TouchGesture.TopEdgeSwipeDownRight;
                if ((x, y) == (-1, 1))
                    return TouchGesture.TopEdgeSwipeDownLeft;
            }
            if (z == 1) // right
            {
                if ((x, y) == (-1, 0))
                    return TouchGesture.RightEdgeSwipeLeft;
                if ((x, y) == (0, -1))
                    return TouchGesture.RightEdgeSwipeUp;
                if ((x, y) == (1, 0))
                    return TouchGesture.RightEdgeSwipeRight;
                if ((x, y) == (0, 1))
                    return TouchGesture.RightEdgeSwipeDown;
                if ((x, y) == (-1, -1))
                    return TouchGesture.RightEdgeSwipeUpLeft;
                if ((x, y) == (1, -1))
                    return TouchGesture.RightEdgeSwipeUpRight;
                if ((x, y) == (1, 1))
                    return TouchGesture.RightEdgeSwipeDownRight;
                if ((x, y) == (-1, 1))
                    return TouchGesture.RightEdgeSwipeDownLeft;
            }
            if (z == 2) // bottom
            {
                if ((x, y) == (-1, 0))
                    return TouchGesture.BottomEdgeSwipeLeft;
                if ((x, y) == (0, -1))
                    return TouchGesture.BottomEdgeSwipeUp;
                if ((x, y) == (1, 0))
                    return TouchGesture.BottomEdgeSwipeRight;
                if ((x, y) == (0, 1))
                    return TouchGesture.BottomEdgeSwipeDown;
                if ((x, y) == (-1, -1))
                    return TouchGesture.BottomEdgeSwipeUpLeft;
                if ((x, y) == (1, -1))
                    return TouchGesture.BottomEdgeSwipeUpRight;
                if ((x, y) == (1, 1))
                    return TouchGesture.BottomEdgeSwipeDownRight;
                if ((x, y) == (-1, 1))
                    return TouchGesture.BottomEdgeSwipeDownLeft;
            }
            if (z == 3) // left
            {
                if ((x, y) == (-1, 0))
                    return TouchGesture.LeftEdgeSwipeLeft;
                if ((x, y) == (0, -1))
                    return TouchGesture.LeftEdgeSwipeUp;
                if ((x, y) == (1, 0))
                    return TouchGesture.LeftEdgeSwipeRight;
                if ((x, y) == (0, 1))
                    return TouchGesture.LeftEdgeSwipeDown;
                if ((x, y) == (-1, -1))
                    return TouchGesture.LeftEdgeSwipeUpLeft;
                if ((x, y) == (1, -1))
                    return TouchGesture.LeftEdgeSwipeUpRight;
                if ((x, y) == (1, 1))
                    return TouchGesture.LeftEdgeSwipeDownRight;
                if ((x, y) == (-1, 1))
                    return TouchGesture.LeftEdgeSwipeDownLeft;
            }
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