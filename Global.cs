using System;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;
using PinePhoneCore.Enums;
using PinePhoneCore.PinePhoneHelpers;
using System.Threading.Tasks;

namespace PinePhoneCore
{
    class Global
    {
        static async Task Main(string[] args)
        {
            await Dependencies.TestDependencies();

            //SoC.CpuCores[1].Enabled = !SoC.CpuCores[1].Enabled;
            //SoC.CpuCores[3].Enabled = !SoC.CpuCores[3].Enabled;
            //
            HeadphoneJack.OnPluggedRaw+= (d)=>
            {
                Console.WriteLine($"Code: {d.Code}, Type: {d.Type}, Value: {d.Value}");
            };
            HardwareButtons.OnKeyStateChanged += (button,state)=> Console.WriteLine($"{button}{(state ? "pressed" : "released")}");
            HardwareButtons.OnVolumeDownKeyStateChanged += (down)=> Console.WriteLine($"VolumeDown: {(down ? "Pressed!" : "Released!")}");
            HardwareButtons.OnVolumeUpKeyStateChanged += (down)=> Console.WriteLine($"VolumeUp: {(down ? "Pressed!" : "Released!")}");
            HardwareButtons.OnPowerKeyStateChanged += (down)=> Console.WriteLine($"PowerButon: {(down ? "Pressed!" : "Released!")}");

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
                Thread.Sleep(10000);
            }
        }
    }
}
