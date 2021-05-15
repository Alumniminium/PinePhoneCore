using System;
using PinePhoneCore.Devices;
using PinePhoneCore.Enums;

namespace PinePhoneCore.PinePhoneHelpers
{
    public static class PinePhoneBattery
    {
        public static BatteryState GetState()
        {
            var state = Enum.Parse<BatteryState>(Battery.Status);
            if (GetChargePercentage() > 89 && IsConnectedToCharger())
                state = BatteryState.Full;
            return state;
        }
        public static bool IsConnectedToCharger() => PowerSupply.Present;
        public static int GetChargeMilliAmps() => (Battery.Capacity / 100) * Battery.ChargePercent;
        public static byte GetChargePercentage() => Battery.ChargePercent;
        public static int GetChargeFlowMilliAmps() => Battery.CurrentFlow;

        public static TimeSpan GetTimeUntilEmpty()
        {
            var hours = GetChargeMilliAmps() / Battery.CurrentFlow;
            return TimeSpan.FromHours(hours);
        }
        public static TimeSpan GetTimeUntilFull()
        {
            var deltaCapacity = Battery.Capacity - GetChargeMilliAmps();
            var hours = deltaCapacity / Battery.CurrentFlow;

            Console.WriteLine($"deltaCapacity: {deltaCapacity}, 85%: { Battery.Capacity - (Battery.Capacity * 0.85)}");

            if (IsFull(deltaCapacity))
                hours = 0;

            return TimeSpan.FromHours(hours);
        }

        private static bool IsFull(int capacityLeft) => capacityLeft <= Battery.Capacity - (Battery.Capacity * 0.89);
    }
}
