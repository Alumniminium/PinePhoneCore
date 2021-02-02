using System;
using System.Threading;

namespace PinePhoneCore.Helpers
{
    public class DevInputEventMonitor
    {
        private Thread MonitoringThread;
        public string Path;

        public Action<NativeInputEvent> OnData;
        public NativeInputEvent LastEvent;

        public DevInputEventMonitor(string path)
        {
            Path = path;
            MonitoringThread = new Thread(MonitorLoop);
            MonitoringThread.IsBackground = true;
            MonitoringThread.Start();
        }
        private void MonitorLoop()
        {
            var handle = NativeCode.Open(Path, 2);
            if (handle == -1)
                return;

            var buffer = new byte[128]; // 78 bytes
            while (true)
            {
                int lengthOfDataInBuffer = NativeCode.Read(handle, buffer, buffer.Length);
                var seconds = BitConverter.ToUInt64(buffer, 0);
                var microseconds = BitConverter.ToUInt64(buffer, 8);
                var type = BitConverter.ToUInt16(buffer, 16);
                var code = BitConverter.ToUInt16(buffer, 18);
                var val = BitConverter.ToUInt32(buffer, 20);

                LastEvent = new NativeInputEvent
                {
                    Seconds = seconds,
                    Microseconds = microseconds,
                    Type = type,
                    Code = code,
                    Value = val
                };
                OnData?.Invoke(LastEvent);
            }
        }
    }
}