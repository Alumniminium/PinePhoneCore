using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace PinePhoneCore.Helpers
{
    public static class Dependencies
    {
        public static Dictionary<string, bool> Found = new Dictionary<string, bool>
        {
            ["nmcli"] = false,
            ["ip"] = false,
            ["ifconfig"] = false,
            ["iw"] = false,
            ["iwconfig"] = false,
            ["iwlist"] = false,
            ["iwgetid"] = false,
            ["rfkill"] = false,
        };

        public static async Task TestDependencies()
        {
            foreach (var kvp in Found)
            {
                try
                {
                    var cmd = await Cli.Wrap(kvp.Key).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync();
                    Found[kvp.Key] = true;
                    Console.WriteLine($"Found {kvp.Key}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}