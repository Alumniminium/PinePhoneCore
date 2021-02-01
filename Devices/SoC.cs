using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class SoC
    {
        public float GpuTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon3/temp1_input")) / 1000f;
        public CpuCore[] CpuCores;
        public float CpuTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_input")) / 1000f;
        public float CpuCriticalTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_crit")) / 1000f;

        public SoC()
        {
            CpuCores = new CpuCore[4];
            for (int i = 0; i < 4; i++)
                CpuCores[i] = new CpuCore(i);
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
