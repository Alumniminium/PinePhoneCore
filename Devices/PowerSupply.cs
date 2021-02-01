using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class PowerSupply
    {
        public const string PATH = "/sys/class/power_supply/axp20x-usb";

        public string Health => File.ReadAllText($"{PATH}/health").Trim();
        public bool Online => File.ReadAllText($"{PATH}/online").Trim() == "1";
        public bool Present => File.ReadAllText($"{PATH}/present").Trim() == "1";
        public bool BCEnabled => File.ReadAllText($"{PATH}/usb_bc_enabled").Trim() == "1";
        public string Type => File.ReadAllText($"{PATH}/type").Trim();
        public float MinVoltage => int.Parse(File.ReadAllText($"{PATH}/voltage_min")) / 1000000f;
        public float InputCurrentLimit => int.Parse(File.ReadAllText($"{PATH}/input_current_limit")) / 1000f;
        public float InputCurrentLimitDCP => int.Parse(File.ReadAllText($"{PATH}/usb_dcp_input_current_limit")) / 1000f;
        public string Protocol => File.ReadAllText($"{PATH}/usb_type").Split('[')[1].Split(']')[0];
        
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            return sb.ToString();
        }
    }
}
