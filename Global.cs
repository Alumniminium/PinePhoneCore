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
        public static SoC SoC = new();
        public static WiFi WiFi = new();
        public static Battery Battery = new();
        public static Display Display = new();
        public static Bluetooth Bluetooth = new();
        public static PowerSupply PowerSupply = new();
        public static Magnetometer Magnetometer = new();
        public static Accelerometer Accelerometer = new();
        public static ProximitySensor ProximitySensor = new();
        public static AmbientLightSensor AmbientLightSensor = new();

        static async Task Main(string[] args)
        {
            await Dependencies.TestDependencies();
            //Display.Brightness = 500;
            //Display.PowerOn = true;

            Console.WriteLine("Battery: ");
            Console.WriteLine(Battery.ToString());
            Console.WriteLine();
            Console.WriteLine("PowerSupply:");
            Console.WriteLine(PowerSupply.ToString());
            Console.WriteLine();
            Console.WriteLine("WiFi:");
            Console.WriteLine(WiFi.ToString());
            Console.WriteLine();
            Console.WriteLine("Display:");
            Console.WriteLine(Display.ToString());
            Console.WriteLine();
            Console.WriteLine("SoC:");
            Console.WriteLine(SoC.ToString());
            Console.WriteLine();
            Console.WriteLine("Bluetooth:");
            Console.WriteLine(Bluetooth.ToString());
            Console.WriteLine();

            Console.WriteLine("Magnetometer:");
            Console.WriteLine(Magnetometer.ToString());
            Console.WriteLine();
            Console.WriteLine("Proximity Sensor:");
            Console.WriteLine(ProximitySensor.ToString());
            Console.WriteLine();
            Console.WriteLine("Ambient Light Sensor:");
            Console.WriteLine(AmbientLightSensor.ToString());
            Console.WriteLine();
            Console.WriteLine("Accelerometer:");
            Console.WriteLine(Accelerometer.ToString());
            Console.WriteLine();

            SoC.CpuCores[1].Enabled = !SoC.CpuCores[1].Enabled;
            SoC.CpuCores[3].Enabled = !SoC.CpuCores[3].Enabled;

            foreach(var network in PinePhoneWiFi.Scan())
            {
                Console.WriteLine($"Found: {network}");
            }

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
                Thread.Sleep(1000);
            }
        }

        public bool IsCharging() => PowerSupply.Online;
    }
}
