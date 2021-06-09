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

        public static string CpuGovernor
        {
            get => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_governor").Trim();
            set => File.WriteAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_governor", value);
        }
        public static float Temperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_input")) / 1000f;
        public static float CriticalTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_crit")) / 1000f;
        public static float CpuFrequency 
        {
            get=> int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_cur_freq")) / 1000f;
            set=> File.WriteAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_setspeed",(value * 1000).ToString());
        }
        public static float MaxFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/cpuinfo_max_freq")) / 1000f;
        public static float MinFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/cpuinfo_min_freq")) / 1000f;
        public static float GovernorMaxFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_max_freq")) / 1000f;
        public static float GovernorMinFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_min_freq")) / 1000f;
        public static string AvailableFrequencies => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_available_frequencies").Trim();
        public static string AvailableGovernors => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_available_governors").Trim();
        public static string FrequencyStats => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/stats/time_in_state").Trim();
       

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(SoC).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
