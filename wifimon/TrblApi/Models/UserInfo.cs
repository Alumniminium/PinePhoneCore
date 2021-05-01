using System;

namespace wifimon.TrblApi.Models
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString() => $"User: {Username}{Environment.NewLine}Pass: {Password}";
    }
}