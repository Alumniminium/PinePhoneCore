using System;
using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Accelerometer
    {
        public const string PATH = "/sys/bus/i2c/devices/2-0068/iio:device2";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static float Scale => float.Parse(File.ReadAllText($"{PATH}/in_accel_scale"));
        public static float X => (float)Math.Round(RawX * Scale, 2);
        public static float Y => (float)Math.Round(RawY * Scale, 2);
        public static float Z => (float)Math.Round(RawZ * Scale, 2);
        public static float RawX => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_raw"));
        public static float RawY => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_raw"));
        public static float RawZ => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_raw"));
        public static float BiasX
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_calibbias"));
            set => File.WriteAllText($"{PATH}/in_accel_x_calibbias", $"{value}");
        }
        public static float BiasY
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_calibbias"));
            set => File.WriteAllText($"{PATH}/in_accel_y_calibbias", $"{value}");
        }
        public static float BiasZ
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_calibbias"));
            set => File.WriteAllText($"{PATH}/in_accel_z_calibbias", $"{value}");
        }

        public static float AngularVelocityScale => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_scale"));
        public static float AngularVelocityX => (float)Math.Round(AngularVelocityRawX * AngularVelocityScale, 2);
        public static float AngularVelocityY => (float)Math.Round(AngularVelocityRawY * AngularVelocityScale, 2);
        public static float AngularVelocityZ => (float)Math.Round(AngularVelocityRawZ * AngularVelocityScale, 2);
        public static float AngularVelocityRawX => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_raw"));
        public static float AngularVelocityRawY => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_raw"));
        public static float AngularVelocityRawZ => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_raw"));
        public static float AngularVelocityBiasX
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_calibbias"));
            set => File.WriteAllText($"{PATH}/in_anglvel_x_calibbias", $"{value}");
        }
        public static float AngularVelocityBiasY
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_calibbias"));
            set => File.WriteAllText($"{PATH}/in_anglvel_y_calibbias", $"{value}");
        }
        public static float AngularVelocityBiasZ
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_calibbias"));
            set => File.WriteAllText($"{PATH}/in_anglvel_z_calibbias", $"{value}");
        }
        public static short SamplingFrequency
        {
            get => short.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        public static string AvalilableSamplingFrequencies => File.ReadAllText($"{PATH}/sampling_frequency_available").Trim();


        public static bool CalibratePosition(int maxIterations = 1024)
        {
            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"x:{RawX} y:{RawY} z:{RawZ}");
                if (RawX != 0)
                    BiasX += RawX > 0 ? -1 : 1;
                if (RawY != 0)
                    BiasY += RawY > 0 ? -1 : 1;
                if (RawZ != 0)
                    BiasZ += RawZ > 0 ? -1 : 1;

                if ((RawX + RawY + RawZ) == 0)
                {
                    Console.WriteLine("Calibrated X,Y,Y");
                    return true;
                }
            }
            return false;
        }
        public static bool CalibrateAngVel(int maxIterations = 1024)
        {
            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine($"Vx:{AngularVelocityRawX} Vy:{AngularVelocityRawY} Vz:{AngularVelocityRawZ}");

                if (AngularVelocityRawX != 0)
                    AngularVelocityBiasX += AngularVelocityRawX > 0 ? -1 : 1;
                if (AngularVelocityRawY != 0)
                    AngularVelocityBiasY += AngularVelocityRawY > 0 ? -1 : 1;
                if (AngularVelocityRawZ != 0)
                    AngularVelocityBiasZ += AngularVelocityRawZ > 0 ? -1 : 1;

                if ((AngularVelocityRawX + AngularVelocityRawY + AngularVelocityRawZ) == 0)
                {
                    Console.WriteLine("Calibrated Angular Velocity");
                    return true;
                }
            }
            return false;
        }

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Accelerometer).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}