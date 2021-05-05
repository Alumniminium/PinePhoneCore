using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Magnetometer
    {
        public const string PATH="/sys/bus/i2c/devices/2-001e/iio:device3";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static byte SamplingFrequency
        {
            get => byte.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        public static float X => RawX * ScaleX;
        public static float Y => RawY * ScaleY;
        public static float Z => RawZ * ScaleZ;
        public static short RawX => short.Parse(File.ReadAllText($"{PATH}/in_magn_x_raw"));
        public static short RawY => short.Parse(File.ReadAllText($"{PATH}/in_magn_y_raw"));
        public static short RawZ => short.Parse(File.ReadAllText($"{PATH}/in_magn_z_raw"));
        public static float ScaleX
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_x_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_x_scale", value.ToString());
        }public static float ScaleY
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_y_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_y_scale", value.ToString());
        }public static float ScaleZ
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_z_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_z_scale", value.ToString());
        }
        public static string AvalilableScales => File.ReadAllText($"{PATH}/in_magn_scale_available").Trim();
        public static string AvalilableSamplingFrequencies => File.ReadAllText($"{PATH}/sampling_frequency_available").Trim();


        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Magnetometer).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}