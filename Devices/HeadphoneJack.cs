using System.Text;
using PinePhoneCore.Enums;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public static class HardwareButtons
    {
        public static DevInputEventMonitor VolumeButtonMonitor = new DevInputEventMonitor("/dev/input/event3");
        public static DevInputEventMonitor PowerButtonMonitor = new DevInputEventMonitor("/dev/input/event0");
    }
    public static class HeadphoneJack
    {
        public static bool Connected => Monitor.LastEvent.Value == 1;
        public static HeadphoneKind Kind => (HeadphoneKind)Monitor.LastEvent.Code;
        public static DevInputEventMonitor Monitor = new DevInputEventMonitor("/dev/input/event4");


        new public static string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in typeof(HeadphoneJack).GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(null)}");
            return sb.ToString();
        }
    }
}