using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class AmbientLightSensor
    {
        public const string PATH="/sys/bus/i2c/devices/2-0048/iio:device1";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static float Luminance => LuminanceRaw * Scale;
        public static float LuminanceRaw => float.Parse(File.ReadAllText($"{PATH}/in_illuminance_raw"));
        public static float Scale 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_illuminance_scale"));
            set=>File.WriteAllText($"{PATH}/in_illuminance_scale",value.ToString());
        }        
        public static string ScalesAvailable => File.ReadAllText($"{PATH}/in_proximity_scale_available").Trim();
        public static string IntegrationTimesAvailable => File.ReadAllText($"{PATH}/in_illuminance_integration_time_available").Trim();
         
        public static float IntegrationTime 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_illuminance_integration_time"));
            set=>File.WriteAllText($"{PATH}/in_illuminance_integration_time",value.ToString());
        }        

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(AmbientLightSensor).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null, null)}");
            return sb.ToString();
        }
    }
}