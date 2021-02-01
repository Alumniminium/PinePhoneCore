using System.IO;
using System.Text;

namespace PinePhoneCore.Devices
{
    public class Bluetooth
    {
        public bool Enabled
        {
            get => File.ReadAllText("/sys/class/rfkill/rfkill1/soft").Trim() == "0";
            set => File.WriteAllText("/sys/class/rfkill/rfkill1/soft", $"{(value ? "0" : "1")}");
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
