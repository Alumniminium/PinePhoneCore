using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public static class Bluetooth
    {
        public static bool Enabled
        {
            get => File.ReadAllText("/sys/class/rfkill/rfkill1/soft").Trim() == "0";
            set => File.WriteAllText("/sys/class/rfkill/rfkill1/soft", $"{(value ? "0" : "1")}");
        }

        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(Bluetooth).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}
