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
