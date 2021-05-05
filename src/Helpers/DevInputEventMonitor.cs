using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PinePhoneCore.Helpers
{
    public class UeventMon
    {
        private Thread monThread;
        public string Path;

        public UeventMon(string path)
        {
            Path = path;
            monThread = new Thread(MonitorLoop) { IsBackground = true };
            monThread.Start();
        }
        private unsafe void MonitorLoop()
        {
            using var file = new FileStream(Path,FileMode.Open,FileAccess.Read,FileShare.Read,4096,true);
            var buffer = new byte[4096];

            while (true)
            {
                var read = file.Read(buffer,0, buffer.Length);
                Console.WriteLine(Encoding.ASCII.GetString(buffer,0,read));
            }
        }
    }
    public class DevInputEventMonitor
    {
        private Thread monThread;
        public string Path;

        public Action<NativeInputEvent> OnData;
        public NativeInputEvent LastEvent;

        public DevInputEventMonitor(string path)
        {
            Path = path;
            monThread = new Thread(MonitorLoop) { IsBackground = true };
            monThread.Start();
        }
        private unsafe void MonitorLoop()
        {
            using var file = new FileStream(Path,FileMode.Open,FileAccess.Read);
            var buffer = new byte[24];

            while (true)
            {
                if(file.Read(buffer,0, buffer.Length)<24)
                    continue;

                fixed (byte* p = buffer)
                    LastEvent = *(NativeInputEvent*)p;

                OnData?.Invoke(LastEvent);
            }
        }
    }
}