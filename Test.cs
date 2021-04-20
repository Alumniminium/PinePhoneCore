using System;
using System.Threading;
using PinePhoneCore.Helpers;
using PinePhoneCore.Enums;
using PinePhoneCore.PinePhoneHelpers;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using PinePhoneCore.Devices;

namespace PinePhoneCore
{
    public static class Test
    {
        public static string HexDump(byte[] bytes, int bytesPerLine)
        {
            StringBuilder sb = new StringBuilder();
            for (int line = 0; line < bytes.Length; line += bytesPerLine)
            {
                byte[] lineBytes = bytes.Skip(line).Take(bytesPerLine).ToArray();
                sb.AppendFormat("{0:x8} ", line);
                sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
                       .ToArray()).PadRight(bytesPerLine * 3));
                sb.Append(" ");
                sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char)b)
                       .ToArray()));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        
        public static void Main(string[] args)
        {
            Dependencies.TestDependencies().GetAwaiter().GetResult();
            
            //SoC.CpuCores[1].Enabled = !SoC.CpuCores[1].Enabled;
            //SoC.CpuCores[3].Enabled = !SoC.CpuCores[3].Enabled;
            //
            //Digitizer.OnEvent += (e) =>
            //{
            //    Console.WriteLine($"Code: {e.Code}, Type: {e.Type}, Value: {e.Value}");
            //};
            // Digitizer.OnPositionChanged += (p) => Console.WriteLine($"Position: {p.X},{p.Y} - Finger #{p.FingerIndex}");
            //Digitizer.OnTouching += (p) => Console.WriteLine($"First Touch!");
            //Digitizer.OnStopTouching += (p) => Console.WriteLine($"Nothing touching the screen anymore");
            // Digitizer.OnFingerAdded += (p) => Console.WriteLine($"Finger {p} added");
            // Digitizer.OnFingerRemoved += (p) => Console.WriteLine($"Finger {p} removed");
            Digitizer.OnGesture += (g) => Console.WriteLine($"Gesture: "+g.ToString());
            
            HeadphoneJack.OnPluggedRaw += (d) =>
            {
                Console.WriteLine($"Code: {d.Code}, Type: {d.Type}, Value: {d.Value}");
            };
            HardwareButtons.OnKeyStateChanged += (button, state) => Console.WriteLine($"{button}{(state ? "pressed" : "released")}");
            HardwareButtons.OnVolumeDownKeyStateChanged += (down) => Console.WriteLine($"VolumeDown: {(down ? "Pressed!" : "Released!")}");
            HardwareButtons.OnVolumeUpKeyStateChanged += (down) => Console.WriteLine($"VolumeUp: {(down ? "Pressed!" : "Released!")}");
            HardwareButtons.OnPowerKeyStateChanged += (down) => Console.WriteLine($"PowerButon: {(down ? "Pressed!" : "Released!")}");

            //UeventMon m = new UeventMon("/sys/class/power_supply/axp20x-usb/uevent");

            PowerSupply.InputCurrentLimit = 2500;
            PowerSupply.InputCurrentLimitDCP = 2500;
            // Console.WriteLine("Connecting to wifi");
            // WiFi.Enabled_NMCLI = true;
            // WiFi.Connect("fsociety");
            // Console.WriteLine("Connected: " + WiFi.IsConnected_NMCLI);
            // Console.WriteLine();

            while (true)
            {
                var state = PinePhoneBattery.GetState();

                switch (state)
                {
                    case BatteryState.Unknown:
                        break;
                    case BatteryState.Charging:
                        Console.WriteLine($"Battery full in {PinePhoneBattery.GetTimeUntilFull().ToString("hh'h 'mm'min'")} (delivering {PinePhoneBattery.GetChargeFlowMilliAmps()} mAh)");
                        break;
                    case BatteryState.Discharging:
                        Console.WriteLine($"Battery empty in {PinePhoneBattery.GetTimeUntilEmpty().ToString("hh'h 'mm'min'")} (drawing {PinePhoneBattery.GetChargeFlowMilliAmps()} mAh)");
                        break;
                }
                Thread.Sleep(5000);
            }
        }
    }
}
