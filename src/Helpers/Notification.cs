using System;

namespace PinePhoneCore.Helpers
{
    public static class Notification
    {
        public static void Display(string message, string user, TimeSpan timeout)
        {
            if (Environment.UserName != user)
                Shell.Execute("su", $"{user} -c \"notify-send '{message}' -t {(int)timeout.TotalMilliseconds}\"");
            else
                Shell.Execute("notify-send", $"\"{message}\" -t {timeout}");
        }
    }
}