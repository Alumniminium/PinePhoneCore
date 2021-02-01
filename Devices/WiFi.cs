using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PinePhoneCore.Helpers;

namespace PinePhoneCore.Devices
{
    public class WiFi
    {
        public bool Enabled_RFKILL
        {
            get => File.ReadAllText("/sys/class/rfkill/rfkill0/soft").Trim() == "0";
            set => File.WriteAllText("/sys/class/rfkill/rfkill0/soft", $"{(value ? "0" : "1")}");
        }
        public bool Enabled_NMCLI
        {
            get => Shell.GetValue("nmcli", "radio wifi") == "enabled";
            set => Shell.GetValue("nmcli", $"radio wifi {(value ? "on" : "off")}");
        }
        public bool Enabled_IPA
        {
            get => Shell.GetValue("ip", "a show dev wlan0").Split(Environment.NewLine)[0].Contains("state UP");
            set => Shell.GetValue("ip", $"link set dev wlan0 {(value ? "up" : "down")}");
        }
        public bool Enabled_IFCONFIG
        {
            get => Shell.GetValue("ifconfig", "wlan0").Contains(" UP ");
            set => Shell.GetValue("ifconfig", $"wlan0 {(value ? "up" : "down")}");
        }
        public bool IsConnected_NMCLI => Shell.GetValue("nmcli", "connection show --active").Contains("wlan0");
        public bool IsConnected_IPA => Shell.GetValue("ip", "a show dev wlan0").Contains(" inet ");
        public bool IsConnected_IFCONFIG => Shell.GetValue("ifconfig", "wlan0").Contains("inet addr");

        public string MAC_IFCONFIG
        {
            get
            {
                var output = Shell.GetValue("ifconfig", "wlan0").Trim();

                if (string.IsNullOrEmpty(output))
                    return string.Empty;

                var macPattern = "DE:AD:BE:EF:13:37";
                var pattern = "HWaddr ";
                var index = output.IndexOf(pattern);

                return output.Substring(index + pattern.Length, macPattern.Length);
            }
            set => Shell.GetValue("ifconfig", "wlan0 hw ether " + value);
        }
        public string MAC_IPA
        {
            get
            {
                var output = Shell.GetValue("ip", "a show dev wlan0").Trim();

                if (string.IsNullOrEmpty(output))
                    return string.Empty;

                var macPattern = "DE:AD:BE:EF:13:37";
                var pattern = "link/ether ";
                var index = output.IndexOf(pattern);

                return output.Substring(index + pattern.Length, macPattern.Length);
            }
            set => Shell.GetValue("ip", "link set dev wlan0 address " + value);
        }

        public string SSID_NMCLI
        {
            get
            {
                var ssid = string.Empty;

                var val = Shell.GetValue("nmcli", "connection show --active");
                foreach (var line in val.Split(Environment.NewLine))
                {
                    if (line.Contains("wlan0"))
                    {
                        val = line;
                        break;
                    }
                }

                ssid = val.Split(' ')[0];

                return ssid;
            }
        }
        public string SSID_IWGETID => Shell.GetValue("iwgetid", "-r");

        public string LocalIP_IFCONFIG
        {
            get
            {
                const string pattern = "inet addr:";
                var found = Shell.FindByPattern("ifconfig", "wlan0", pattern);
                return found.Split(' ')[0];
            }
            set => Shell.GetValue("ifconfig","wlan0 "+value);
        }
        public string LocalIP_IPA
        {
            get
            {
                const string pattern = " inet ";
                var found = Shell.FindByPattern("ip", "a show dev wlan0", pattern);
                return found.Split(' ')[0].Split('/')[0];
            }
            set => Shell.GetValue("ip",$"a add {value} dev wlan0");
        }

        public byte SignalLevel_IWCONFIG
        {
            get
            {
                const string pattern = "Signal level=";
                var found = Shell.FindByPattern("iwconfig", "wlan0", pattern);
                return byte.Parse(found.Split('/')[0]);
            }
        }
        public byte NoiseLevel_IWCONFIG
        {
            get
            {
                const string pattern = "Noise level=";
                var found = Shell.FindByPattern("iwconfig", "wlan0", pattern);
                return byte.Parse(found.Split('/')[0]);
            }
        }
        public byte LinkQuality_IWCONFIG
        {
            get
            {
                const string pattern = "Link Quality=";
                var found = Shell.FindByPattern("iwconfig", "wlan0", pattern);
                return byte.Parse(found.Split('/')[0]);
            }
        }

        public List<string> Scan_IW
        {
            get
            {
                var ssids = new List<string>();
                var output = Shell.GetValue("iw", "dev wlan0 scan").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in output)
                {
                    if (!line.Contains("SSID: "))
                        continue;

                    var ssid = line.Split(":")[1].Trim();
                    ssids.Add(ssid);
                }
                return ssids;
            }
        }
        public List<string> Scan_IWLIST
        {
            get
            {
                var ssids = new List<string>();
                var output = Shell.GetValue("iwlist", "wlan0 scan").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in output)
                {
                    if (!line.Contains("SSID:"))
                        continue;

                    var ssid = line.Split("\"")[1].Trim();
                    ssids.Add(ssid);
                }
                return ssids;
            }
        }
        public List<string> Scan_NMCLI
        {
            get
            {
                var ssids = new List<string>();
                var output = Shell.GetValue("nmcli", "-f ssid dev iwifi").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < output.Length; i++)
                {
                    var ssid = output[i].Trim();
                    ssids.Add(ssid);
                }
                return ssids;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var p in GetType().GetProperties())
                sb.AppendLine($"{p.Name}: {p.GetValue(this, null)}");
            return sb.ToString();
        }
    }
}
