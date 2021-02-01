using System;
using PinePhoneCore.Enums;

namespace PinePhoneCore.PinePhoneHelpers
{
    public static class PinePhoneBattery
    {
        public static BatteryState GetState() => Enum.Parse<BatteryState>(Global.Battery.Status);

        public static bool IsConnectedToCharger() => Global.PowerSupply.Present;
        public static int GetChargeMilliAmps() => (Global.Battery.Capacity / 100) * Global.Battery.ChargePercent;
        public static byte GetChargePercentage() => Global.Battery.ChargePercent;
        public static float GetChargeFlowMilliAmps() => Global.Battery.CurrentFlow;

        public static TimeSpan GetTimeUntilEmpty()
        {
            var hours = GetChargeMilliAmps() / Global.Battery.CurrentFlow;
            return TimeSpan.FromHours(hours);
        }
        public static TimeSpan GetTimeUntilFull()
        {
            var deltaCapacity = Global.Battery.Capacity - GetChargeMilliAmps();
            var hours = deltaCapacity / Global.Battery.CurrentFlow;
            return TimeSpan.FromHours(hours);
        }
    }
}
