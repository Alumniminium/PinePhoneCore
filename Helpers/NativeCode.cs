using System.Runtime.InteropServices;

namespace PinePhoneCore.Helpers
{
    public static class NativeCode
    {
        [DllImport("libc", EntryPoint = "open")]
        public static extern int Open(string portName, int mode);

        [DllImport("libc", EntryPoint = "close")]
        public static extern int Close(int handle);

        [DllImport("libc", EntryPoint = "read")]
        public static extern int Read(int handle, byte[] data, int length);
        
    }    
    public struct NativeInputEvent
    {
        public ulong Seconds;
        public ulong Microseconds;
        public ushort Type;
        public ushort Code;
        public uint Value;
    }
    public class ButtonState
    {
        public bool PowerKeyDown;
        public bool VolumeDownKeyDown;
        public bool VolumeUpKeyDown;
    }
}
