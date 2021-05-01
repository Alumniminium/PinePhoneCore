using System;
using System.IO;

namespace ppwake
{
    public static class Sensors
    {
        public static bool PluggedIn()
        {
            //var raw = File.ReadAllText("/sys/devices/platform/soc/1f03400.rsb/sunxi-rsb-3a3/axp20x-battery-power-supply/power_supply/axp20x-battery");
            
            var raw = @"POWER_SUPPLY_NAME=axp20x-battery
POWER_SUPPLY_TYPE=Battery
POWER_SUPPLY_PRESENT=1
POWER_SUPPLY_ONLINE=1
POWER_SUPPLY_STATUS=Charging
POWER_SUPPLY_VOLTAGE_NOW=3775000
POWER_SUPPLY_CURRENT_NOW=95000
POWER_SUPPLY_CONSTANT_CHARGE_CURRENT=1200000
POWER_SUPPLY_CONSTANT_CHARGE_CURRENT_MAX=1200000
POWER_SUPPLY_HEALTH=Good
POWER_SUPPLY_VOLTAGE_OCV=3787300
POWER_SUPPLY_VOLTAGE_MAX_DESIGN=4200000
POWER_SUPPLY_VOLTAGE_MIN_DESIGN=3000000
POWER_SUPPLY_CAPACITY=40";
            
            var line = raw.Substring(raw.IndexOf("POWER_SUPPLY_STATUS=")).Split(Environment.NewLine)[0];
            var strValue = line.Split('=')[1];

            return strValue == "Charging";
        }
    }
}