using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class Magnetometer
    {
        public const string PATH="/sys/bus/i2c/devices/2-001e/iio:device3";
        public string Name => File.ReadAllText($"{PATH}/name").Trim();
        public byte SamplingFrequency
        {
            get => byte.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        public float X => RawX * ScaleX;
        public float Y => RawY * ScaleY;
        public float Z => RawZ * ScaleZ;
        public short RawX => short.Parse(File.ReadAllText($"{PATH}/in_magn_x_raw"));
        public short RawY => short.Parse(File.ReadAllText($"{PATH}/in_magn_y_raw"));
        public short RawZ => short.Parse(File.ReadAllText($"{PATH}/in_magn_z_raw"));
        public float ScaleX
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_x_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_x_scale", value.ToString());
        }public float ScaleY
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_y_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_y_scale", value.ToString());
        }public float ScaleZ
        {
            get => float.Parse(File.ReadAllText($"{PATH}/in_magn_z_scale"));
            set => File.WriteAllText($"{PATH}/in_magn_z_scale", value.ToString());
        }
        public string AvalilableScales => File.ReadAllText($"{PATH}/in_magn_scale_available").Trim();
        public string AvalilableSamplingFrequencies => File.ReadAllText($"{PATH}/sampling_frequency_available").Trim();


        public override string ToString()
        {
            var freq  = SamplingFrequency;
            SamplingFrequency = 80; // speed up querying
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            SamplingFrequency=freq; // restore prev rate
            return sb.ToString();
        }
    }
}