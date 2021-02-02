using System;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;

namespace PinePhoneCore.PinePhoneHelpers
{
    public static class PinePhoneBattery
    {
        public static BatteryState GetState() => Enum.Parse<BatteryState>(Battery.Status);

        public static bool IsConnectedToCharger() => PowerSupply.Present;
        public static int GetChargeMilliAmps() => (Battery.Capacity / 100) * Battery.ChargePercent;
        public static byte GetChargePercentage() => Battery.ChargePercent;
        public static float GetChargeFlowMilliAmps() => Battery.CurrentFlow;

        public static TimeSpan GetTimeUntilEmpty()
        {
            var hours = GetChargeMilliAmps() / Battery.CurrentFlow;
            return TimeSpan.FromHours(hours);
        }
        public static TimeSpan GetTimeUntilFull()
        {
            var deltaCapacity = Battery.Capacity - GetChargeMilliAmps();
            var hours = deltaCapacity / Battery.CurrentFlow;
            return TimeSpan.FromHours(hours);
        }
    }
}
