using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class CpuCore
    {
        public int Id;
        public bool Enabled
        {
            get => File.ReadAllText($"/sys/devices/system/cpu/cpu{Id}/online").Trim() == "1";
            set => File.WriteAllText($"/sys/devices/system/cpu/cpu{Id}/online", value ? "1" : "0");
        }
        public string Governor
        {
            get => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_governor").Trim();
            set => File.WriteAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_governor", value);
        }
        public float Temperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_input")) / 1000f;
        public float CriticalTemperature => int.Parse(File.ReadAllText("/sys/class/hwmon/hwmon2/temp1_crit")) / 1000f;
        public float Frequency 
        {
            get=> int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_cur_freq")) / 1000f;
            set=> File.WriteAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_setspeed",(value * 1000).ToString());
        }
        public float MaxFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/cpuinfo_max_freq")) / 1000f;
        public float MinFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/cpuinfo_min_freq")) / 1000f;
        public float GovernorMaxFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_max_freq")) / 1000f;
        public float GovernorMinFrequency => int.Parse(File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_min_freq")) / 1000f;
        public string AvailableFrequencies => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_available_frequencies").Trim();
        public string AvailableGovernors => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/scaling_available_governors").Trim();
        public string FrequencyStats => File.ReadAllText("/sys/devices/system/cpu/cpufreq/policy0/stats/time_in_state").Trim();
        
        public CpuCore(int id) => Id = id;


        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            return sb.ToString();
        }
    }
}
