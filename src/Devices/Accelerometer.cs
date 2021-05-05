using System;
using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Accelerometer
    {
        public const string PATH="/sys/bus/i2c/devices/2-0068/iio:device2";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static float Scale => float.Parse(File.ReadAllText($"{PATH}/in_accel_scale"));
        public static float X => (float)Math.Round(RawX * Scale,2);
        public static float Y => (float)Math.Round(RawY * Scale,2);
        public static float Z => (float)Math.Round(RawZ * Scale,2);
        public static float RawX => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_raw"));
        public static float RawY => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_raw"));
        public static float RawZ => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_raw"));
        public static float BiasX => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_calibbias"));
        public static float BiasY => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_calibbias"));
        public static float BiasZ => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_calibbias"));


        public static float AngularVelocityScale => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_scale"));
        public static float AngularVelocityX => (float)Math.Round(AngularVelocityRawX * AngularVelocityScale,2);
        public static float AngularVelocityY => (float)Math.Round(AngularVelocityRawY * AngularVelocityScale,2);
        public static float AngularVelocityZ => (float)Math.Round(AngularVelocityRawZ * AngularVelocityScale,2);
        public static float AngularVelocityRawX => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_raw"));
        public static float AngularVelocityRawY => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_raw"));
        public static float AngularVelocityRawZ => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_raw"));
        public static float AngularVelocityBiasX => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_calibbias"));
        public static float AngularVelocityBiasY => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_calibbias"));
        public static float AngularVelocityBiasZ => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_calibbias"));
        public static short SamplingFrequency
        {
            get => short.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        public static string AvalilableSamplingFrequencies => File.ReadAllText($"{PATH}/sampling_frequency_available").Trim();


        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Accelerometer).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}