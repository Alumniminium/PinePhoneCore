using System;
using System.Threading;
using PinePhoneCore.Devices;
using PinePhoneCore.Helpers;
using PinePhoneCore.Enums;
using PinePhoneCore.PinePhoneHelpers;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

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
            HardwareButtons.OnKeyStateChanged += (buttonState)=>
            {
                Console.WriteLine($"Power Pressed: {buttonState.PowerKeyDown}, Vol Up Perssed: {buttonState.VolumeDownKeyDown}, Vol Down Pressed: {buttonState.VolumeUpKeyDown}");
            };
            HardwareButtons.OnVolumeDownKeyStateChanged += (down)=>
            {
                Console.WriteLine($"VolumeDown: {(down ? "Pressed!" : "Released!")}");
            };
            HardwareButtons.OnVolumeUpKeyStateChanged += (down)=>
            {
                Console.WriteLine($"VolumeUp: {(down ? "Pressed!" : "Released!")}");
            };
            HardwareButtons.OnPowerKeyStateChanged += (down)=>
            {
                Console.WriteLine($"PowerButon: {(down ? "Pressed!" : "Released!")}");
            };

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
