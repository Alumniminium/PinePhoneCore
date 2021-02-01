using System;
using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class Accelerometer
    {
        public const string PATH="/sys/bus/i2c/devices/2-0068/iio:device2";
        public string Name => File.ReadAllText($"{PATH}/name").Trim();

        public float Scale => float.Parse(File.ReadAllText($"{PATH}/in_accel_scale"));
        public float X => (float)Math.Round(RawX * Scale,2);
        public float Y => (float)Math.Round(RawY * Scale,2);
        public float Z => (float)Math.Round(RawZ * Scale,2);
        public float RawX => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_raw"));
        public float RawY => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_raw"));
        public float RawZ => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_raw"));
        public float BiasX => float.Parse(File.ReadAllText($"{PATH}/in_accel_x_calibbias"));
        public float BiasY => float.Parse(File.ReadAllText($"{PATH}/in_accel_y_calibbias"));
        public float BiasZ => float.Parse(File.ReadAllText($"{PATH}/in_accel_z_calibbias"));


        public float AngularVelocityScale => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_scale"));
        public float AngularVelocityX => (float)Math.Round(AngularVelocityRawX * AngularVelocityScale,2);
        public float AngularVelocityY => (float)Math.Round(AngularVelocityRawY * AngularVelocityScale,2);
        public float AngularVelocityZ => (float)Math.Round(AngularVelocityRawZ * AngularVelocityScale,2);
        public float AngularVelocityRawX => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_raw"));
        public float AngularVelocityRawY => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_raw"));
        public float AngularVelocityRawZ => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_raw"));
        public float AngularVelocityBiasX => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_x_calibbias"));
        public float AngularVelocityBiasY => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_y_calibbias"));
        public float AngularVelocityBiasZ => float.Parse(File.ReadAllText($"{PATH}/in_anglvel_z_calibbias"));
        public short SamplingFrequency
        {
            get => short.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        public string AvalilableSamplingFrequencies => File.ReadAllText($"{PATH}/sampling_frequency_available").Trim();


        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            return sb.ToString();
        }
    }
}