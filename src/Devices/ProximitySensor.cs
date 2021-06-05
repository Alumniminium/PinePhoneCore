using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class ProximitySensor
    {
        public const float INTEGRATION_TIME_0000185 = 0.000185f;
        public const float INTEGRATION_TIME_0000370 = 0.000370f;
        public const float INTEGRATION_TIME_0000741 = 0.000741f;
        public const float INTEGRATION_TIME_0001480 = 0.001480f;
        public const float INTEGRATION_TIME_0002960 = 0.002960f;
        public const float INTEGRATION_TIME_0005920 = 0.005920f;
        public const float INTEGRATION_TIME_0011840 = 0.011840f;
        public const float INTEGRATION_TIME_0023680 = 0.023680f;
        public const float INTEGRATION_TIME_0047360 = 0.047360f;
        public const float INTEGRATION_TIME_0094720 = 0.094720f;
        public const float INTEGRATION_TIME_0189440 = 0.189440f;
        public const float INTEGRATION_TIME_0378880 = 0.378880f;
        public const float INTEGRATION_TIME_0757760 = 0.757760f;
        public const float INTEGRATION_TIME_1515520 = 1.515520f;
        public const float INTEGRATION_TIME_3031040 = 3.031040f;
        public const float INTEGRATION_TIME_6062080 = 6.062080f;

        public const float GAIN_x64 = 6.4f;
        public const float GAIN_x16 = 1.6f;
        public const float GAIN_x4 = 0.4f;
        public const float GAIN_x1 = 0.1f;

        public static (float integrationTime, float gain) Preset100ms = (INTEGRATION_TIME_0094720, GAIN_x1);
        public static (float integrationTime, float gain) Preset500ms = (INTEGRATION_TIME_0378880, GAIN_x1);
        public static (float integrationTime, float gain) Preset1500ms = (INTEGRATION_TIME_1515520, GAIN_x1);
        public static (float integrationTime, float gain) Preset6500ms = (INTEGRATION_TIME_6062080, GAIN_x1);

        public const string PATH="/sys/bus/i2c/devices/2-0048/iio:device1";
        public static string Name => File.ReadAllText($"{PATH}/name").Trim();
        public static float Proximity => float.Parse(File.ReadAllText($"{PATH}/in_proximity_raw"));
        public static float Gain 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_proximity_scale"));
            set=>File.WriteAllText($"{PATH}/in_proximity_scale",value.ToString());
        }        
        
        public static float IntegrationTime 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_proximity_integration_time"));
            set=>File.WriteAllText($"{PATH}/in_proximity_integration_time",value.ToString());
        }        

        public static void ApplyPreset((float integrationTime, float gain) preset)
        {
            IntegrationTime = preset.integrationTime;
            Gain = preset.gain;
        }
        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(ProximitySensor).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
