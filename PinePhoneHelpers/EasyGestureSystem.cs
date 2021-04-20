using System;
using System.Collections.Generic;
using System.Numerics;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.PinePhoneHelpers
{
    public class GestureZone
    {
        public Action<TouchGestureType> OnGesture;
        public int X, Y, W, H;
        private int X2, Y2;

        public GestureZone(int x, int y, int w, int h)
        {
            X = x;
            W = w;
            X2 = X + W;
            Y = y;
            H = h;
            Y2 = Y + H;
        }
        public bool Contains(int x, int y) => (x >= X && x <= X2 && y >= Y && y <= Y2);
    }
    public static class EasyGestureSystem
    {
        static DigitizerState[] InitialStates;
        static Dictionary<GestureZoneName, GestureZone> Zones;
        public static Action<TouchGestureType> OnGesture;

        public const int SCREEN_WIDTH = 720;
        public const int SCREEN_HEIGHT = 1440;
        public static int ZoneThickness = 150;
        static EasyGestureSystem()
        {
            InitialStates = new DigitizerState[10];
            Zones = new Dictionary<GestureZoneName, GestureZone>
            {
                [GestureZoneName.Top] = new GestureZone(0, 0, SCREEN_WIDTH, ZoneThickness),
                [GestureZoneName.Right] = new GestureZone(SCREEN_WIDTH - ZoneThickness, 100, ZoneThickness, SCREEN_HEIGHT - ZoneThickness),
                [GestureZoneName.Bottom] = new GestureZone(0, SCREEN_HEIGHT - ZoneThickness, SCREEN_WIDTH, ZoneThickness),
                [GestureZoneName.Left] = new GestureZone(0, 100, ZoneThickness, SCREEN_HEIGHT - 100)
            };

            foreach (var kvp in Zones)
                kvp.Value.OnGesture += (x) =>
                 {
                     Console.Write(kvp.Key + ": ");
                     OnGesture?.Invoke(x);
                 };
            Digitizer.OnFingerAdded += StartPeriod;
            Digitizer.OnFingerRemoved += StopPeriod;
            Digitizer.OnPositionXChanged += UpdateXPositions;
            Digitizer.OnPositionYChanged += UpdateYPositions;
        }

        private static void UpdateXPositions(DigitizerState obj)
        {
            if (InitialStates[Digitizer.CurrentFingerIndex].X == 0)
                InitialStates[Digitizer.CurrentFingerIndex].X = (ushort)obj.X;
        }
        private static void UpdateYPositions(DigitizerState obj)
        {
            if (InitialStates[Digitizer.CurrentFingerIndex].Y == 0)
                InitialStates[Digitizer.CurrentFingerIndex].Y = (ushort)obj.Y;
        }

        private static void StopPeriod(int fingerId)
        {
            var b = InitialStates[fingerId];
            var e = Digitizer.CurrentStates[fingerId];
            var sV = new Vector2(b.X, b.Y);
            var eV = new Vector2(e.X, e.Y);
            var dist = Vector2.Distance(sV, eV);

            if (dist < 100)
                return;

            var dir = Vector2.Normalize(eV - sV);
            var x = (int)Math.Round(dir.X, 0, MidpointRounding.ToEven);
            var y = (int)Math.Round(dir.Y, 0, MidpointRounding.ToEven);
            var g = TouchGestureType.None;

            switch ((x, y))
            {
                case (-1, 0): g = TouchGestureType.SwipeLeft; break;
                case (0, -1): g = TouchGestureType.SwipeUp; break;
                case (1, 0): g = TouchGestureType.SwipeRight; break;
                case (0, 1): g = TouchGestureType.SwipeDown; break;
                case (-1, -1): g = TouchGestureType.SwipeUpLeft; break;
                case (1, -1): g = TouchGestureType.SwipeUpRight; break;
                case (1, 1): g = TouchGestureType.SwipeDownRight; break;
                case (-1, 1): g = TouchGestureType.SwipeDownLeft; break;
            }

            foreach (var kvp in Zones)
                if (kvp.Value.Contains(b.X, b.Y))
                    kvp.Value.OnGesture?.Invoke(g);

            OnGesture?.Invoke(g);
        }

        private static void StartPeriod(int fingerId) => InitialStates[fingerId] = new DigitizerState { X = 0, Y = 0 };
    }
}
