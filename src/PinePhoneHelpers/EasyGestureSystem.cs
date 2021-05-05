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
        public string Name;

        public GestureZone(string name, int x, int y, int w, int h)
        {
            Name = name;
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
        static List<GestureZone> Zones;
        static List<DigitizerState>[] TempStates;
        public static Action<TouchGestureType> OnGesture;

        public const int SCREEN_WIDTH = 720;
        public const int SCREEN_HEIGHT = 1440;
        public const int ZONE_THICKNESS = 215;
        static EasyGestureSystem()
        {
            Zones = new List<GestureZone>();
            TempStates = new List<DigitizerState>[10];
            InitialStates = new DigitizerState[10];

            for (int i = 0; i < TempStates.Length; i++)
            {
                TempStates[i] = new List<DigitizerState>();
                InitialStates[i] = new DigitizerState
                {
                    X = ushort.MaxValue,
                    Y = ushort.MaxValue
                };
            }
            Digitizer.OnFingerAdded += StartPeriod;
            Digitizer.OnFingerRemoved += StopPeriod;
            Digitizer.OnPositionChanged += UpdatePositions;
        }

        public static void AddZones(List<GestureZone> zones) => Zones.AddRange(zones);

        private static void UpdatePositions(DigitizerState obj)
        {
            if (InitialStates[obj.FingerIndex].X == ushort.MaxValue && InitialStates[obj.FingerIndex].Y == ushort.MaxValue)
                InitialStates[obj.FingerIndex] = obj;
            else
                TempStates[obj.FingerIndex].Add(obj);
        }
        public static double GetAngleDeg(Vector2 p1, Vector2 p2)
        {
            double deltaY = (p1.Y - p2.Y);
            double deltaX = (p1.X - p2.X);
            double result = (Math.Atan2(deltaY, deltaX) / 180) * Math.PI;
            return (result < 0) ? (360d + result) : result;
        }
        private static void StopPeriod(int fingerId)
        {
            var b = InitialStates[fingerId];
            var e = Digitizer.CurrentStates[fingerId];
            var sV = new Vector2(b.X, b.Y);
            var eV = new Vector2(e.X, e.Y);
            var dist = 0f;
            var angle = 0f;
            var pointyAngles = 0;

            for (int i = 1; i < TempStates[fingerId].Count - 1; i++)
                if (i % 5 == 0)
                    TempStates[fingerId].RemoveAt(i);

            for (int i = 1; i < TempStates[fingerId].Count - 1; i++)
            {
                var prev = TempStates[fingerId][i - 1];
                var cur = TempStates[fingerId][i];
                var next = TempStates[fingerId][i + 1];

                var pV = new Vector2(prev.X, prev.Y);
                var cV = new Vector2(cur.X, cur.Y);
                var nV = new Vector2(next.X, next.Y);

                dist += Vector2.Distance(pV, cV) + Vector2.Distance(cV, nV);
                var deg = (float)Math.Abs(GetAngleDeg(pV, cV));
                angle += deg;
                if (deg > 60)
                    pointyAngles++;
            }

            angle = angle / TempStates[fingerId].Count;

            if (pointyAngles == 4)
                Console.WriteLine("Rectangle!");
            else if (pointyAngles == 3)
                Console.WriteLine("Rectangle!");
            else if (pointyAngles == 2)
                Console.WriteLine("half circle!");
            else if (Math.PI - angle < 0.5)
                Console.WriteLine("Circle!");

            Console.WriteLine("Distance: " + (int)dist + ", Sum Angle: " + angle);
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

            foreach (var zone in Zones)
            {
                if (zone.Contains(b.X, b.Y))
                {
                    zone.OnGesture?.Invoke(g);
                    break;
                }
            }

            TempStates[fingerId].Clear();
            OnGesture?.Invoke(g);
        }

        private static void StartPeriod(int fingerId) => InitialStates[fingerId] = new DigitizerState { X = ushort.MaxValue, Y = ushort.MaxValue };
    }
}
