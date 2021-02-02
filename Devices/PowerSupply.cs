using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class PowerSupply
    {
        public const string PATH = "/sys/class/power_supply/axp20x-usb";

        public static string Health => File.ReadAllText($"{PATH}/health").Trim();
        public static bool Online => File.ReadAllText($"{PATH}/online").Trim() == "1";
        public static bool Present => File.ReadAllText($"{PATH}/present").Trim() == "1";
        public static bool BCEnabled => File.ReadAllText($"{PATH}/usb_bc_enabled").Trim() == "1";
        public static string Type => File.ReadAllText($"{PATH}/type").Trim();
        public static float MinVoltage => int.Parse(File.ReadAllText($"{PATH}/voltage_min")) / 1000000f;
        public static float InputCurrentLimit => int.Parse(File.ReadAllText($"{PATH}/input_current_limit")) / 1000f;
        public static float InputCurrentLimitDCP => int.Parse(File.ReadAllText($"{PATH}/usb_dcp_input_current_limit")) / 1000f;
        public static string Protocol => File.ReadAllText($"{PATH}/usb_type").Split('[')[1].Split(']')[0];
        
        
       new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(PowerSupply).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
