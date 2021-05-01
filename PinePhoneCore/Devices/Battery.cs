using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Battery
    {
        public const string PATH = "/sys/class/power_supply/axp20x-battery";
        public static int Capacity = 3000;
        public static string Status => File.ReadAllText($"{PATH}/status").Trim();
        public static string Health => File.ReadAllText($"{PATH}/health").Trim();
        public static bool Online => File.ReadAllText($"{PATH}/online").Trim() == "1";
        public static bool Present => File.ReadAllText($"{PATH}/present").Trim() == "1";
        public static string Type => File.ReadAllText($"{PATH}/type").Trim();
        public static byte ChargePercent => byte.Parse(File.ReadAllText($"{PATH}/capacity"));
        public static float MaxDesignVoltage
        {
            get => int.Parse(File.ReadAllText($"{PATH}/voltage_max_design")) / 1000000f;
            set => File.WriteAllText($"{PATH}/voltage_max_design", (value * 1000000f).ToString());
        }
        public static float MinDesignVoltage
        {
            get => int.Parse(File.ReadAllText($"{PATH}/voltage_min_design")) / 1000000f;
            set => File.WriteAllText($"{PATH}/voltage_min_design", (value * 1000000f).ToString());
        }
        public static float Voltage => int.Parse(File.ReadAllText($"{PATH}/voltage_now")) / 1000000f;
        public static float VoltageCalibration => int.Parse(File.ReadAllText($"{PATH}/voltage_ocv")) / 1000000f;
        public static float ConstantChargeCurrent
        {
            get => int.Parse(File.ReadAllText($"{PATH}/constant_charge_current")) / 1000f;
            set => File.WriteAllText($"{PATH}/constant_charge_current", (value * 1000f).ToString());
        }
        public static float MaxConstantChargeCurrent
        {
            get => int.Parse(File.ReadAllText($"{PATH}/constant_charge_current_max")) / 1000f;
            set => File.WriteAllText($"{PATH}/constant_charge_current_max", (value * 1000f).ToString());
        }
        public static float CurrentFlow => int.Parse(File.ReadAllText($"{PATH}/current_now")) / 1000f;
        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Battery).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
