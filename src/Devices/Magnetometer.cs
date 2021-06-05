using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Magnetometer
    {
        public const string PATH="/sys/bus/i2c/devices/2-001e/iio:device2";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static byte SamplingFrequency
        {
            get => byte.Parse(File.ReadAllText($"{PATH}/sampling_frequency"));
            set => File.WriteAllText($"{PATH}/sampling_frequency", value.ToString());
        }

        // public static float X
        // {
        //     get 
        //     {
        //         switch(ScaleX)
        //         {
        //             case 0.000146f:
        //                 //return RawX / 1711f;
        //                 return RawX / 6842f;
        //             case 0.000292f:
        //                 //return RawX / 2281f;
        //                 return RawX / 3421f;
        //             case 0.000438f:
        //                 //return RawX / 3421f;
        //                 return RawX / 2281f;
        //             case 0.000584f:
        //                 //return RawX / 6842f;
        //                 return RawX / 1711f;
        //             default:
        //                 return RawX;
        //         }
        //     }
        // }
        // public static float Y
        // {
        //     get 
        //     {
        //         switch(ScaleY)
        //         {
        //             case 0.000146f:
        //                 return RawY / 6842f;
        //                 //return RawY / 1711f;
        //             case 0.000292f:
        //                 return RawY / 3421f;
        //                 //return RawY / 2281f;
        //             case 0.000438f:
        //                 return RawY / 2281f;
        //                 //return RawY / 3421f;
        //             case 0.000584f:
        //                 return RawY / 1711f;
        //                 //return RawY / 6842f;
        //             default:
        //                 return RawY;
        //         }
        //     }
        // }
        // public static float Z
        // {
        //     get 
        //     {
        //         switch(ScaleZ)
        //         {
        //             case 0.000146f:
        //                 return RawZ / 6842f;
        //                 //return RawZ / 1711f;
        //             case 0.000292f:
        //                 return RawZ / 3421f;
        //                 //return RawZ / 2281f;
        //             case 0.000438f:
        //                 return RawZ / 2281f;
        //                 //return RawZ / 3421f;
        //             case 0.000584f:
        //                 return RawZ / 1711f;
        //                 //return RawZ / 6842f;
        //             default:
        //                 return RawZ;
        //         }
        //     }
        // }
        public static float X => short.Parse(File.ReadAllText($"{PATH}/in_magn_x_raw")) * ScaleX;
        public static float Y => short.Parse(File.ReadAllText($"{PATH}/in_magn_y_raw")) * ScaleY;
        public static float Z => short.Parse(File.ReadAllText($"{PATH}/in_magn_z_raw")) * ScaleZ;
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