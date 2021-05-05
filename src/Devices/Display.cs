using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Display
    {
        public static int MaxBrightness => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/max_brightness"));
        public static int ActualBrightness => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/actual_brightness"));
        public static bool PowerOn
        {
            get => File.ReadAllText("/sys/class/backlight/backlight/bl_power").Trim() == "0";
            set => File.WriteAllText("/sys/class/backlight/backlight/bl_power", value ? "0" : "1");
        }
        public static int Brightness
        {
            get => int.Parse(File.ReadAllText("/sys/class/backlight/backlight/brightness"));
            set => File.WriteAllText("/sys/class/backlight/backlight/brightness", value.ToString());
        }

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Display).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
