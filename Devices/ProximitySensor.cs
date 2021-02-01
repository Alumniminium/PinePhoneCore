using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class ProximitySensor
    {
        public const string PATH="/sys/bus/i2c/devices/2-0048/iio:device1";
        public string Name => File.ReadAllText($"{PATH}/name").Trim();
        public float Proximity => ProximityRaw * Scale;
        public float ProximityRaw => float.Parse(File.ReadAllText($"{PATH}/in_proximity_raw"));
        public float Scale 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_proximity_scale"));
            set=>File.WriteAllText($"{PATH}/in_proximity_scale",value.ToString());
        }        
        public string ScalesAvailable => File.ReadAllText($"{PATH}/in_proximity_scale_available").Trim();
        public string IntegrationTimesAvailable => File.ReadAllText($"{PATH}/in_proximity_integration_time_available").Trim();
        
        public float IntegrationTime 
        {
            get=>float.Parse(File.ReadAllText($"{PATH}/in_proximity_integration_time"));
            set=>File.WriteAllText($"{PATH}/in_proximity_integration_time",value.ToString());
        }        

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            return sb.ToString();
        }
    }
}