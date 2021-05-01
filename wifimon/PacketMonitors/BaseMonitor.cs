using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet.Ieee80211;
using wifimon.TrblApi;

namespace wifimon
{
    public class BaseMonitor<T>
    {
        public static TrblWiFiApiClient TrblClient;
        public Dictionary<string, List<string>> Cache;
        public string LogFilePath;
        public StreamWriter Writer;

        public BaseMonitor(string path)
        {
            Cache = new Dictionary<string, List<string>>();
            Writer = new StreamWriter(path, true, Encoding.UTF8);
            Writer.AutoFlush = true;

            if (TrblClient == null)
            {
                TrblClient = new TrblWiFiApiClient();
                TrblClient.LoginAsync(null, null).GetAwaiter().GetResult();
                Console.WriteLine("-- Connected to Her.st Intelligence API!");
            }
        }
        public virtual Task HandlePacket(RadioPacket rp, T frame) => Task.CompletedTask;

        public bool TryCache(string key, string val)
        {
            if (Cache.TryGetValue(key, out var list))
            {
                var isNew = !list.Contains(val);

                if (isNew)
                    list.Add(val);

                return isNew;
            }
            else
                return Cache.TryAdd(key, new List<string> { val });
        }
        public void Log(string data)
        {
            Writer.WriteLine(data);
            Console.WriteLine(data);
        }
    }
}