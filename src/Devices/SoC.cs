using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class SoC
    {
        public static float GpuTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon3/temp1_input")) / 1000f;
        public static CpuCore[] CpuCores = new[] { new CpuCore(0), new CpuCore(1), new CpuCore(2), new CpuCore(3) };
        public static float CpuTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_input")) / 1000f;
        public static float CpuCriticalTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_crit")) / 1000f;

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(SoC).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
