using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class Display
    {
        public int MaxBrightness => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/max_brightness"));
        public int ActualBrightness => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/actual_brightness"));
        public bool PowerOn
        {
            get => File.ReadAllText("/sys/class/backlight/backlight/bl_power").Trim() == "0";
            set => File.WriteAllText("/sys/class/backlight/backlight/bl_power", value ? "0" : "1");
        }
        public int Brightness
        {
            get => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/brightness"));
            set => File.WriteAllText("/sys/class/backlight/backlight/brightness", value.ToString());
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
