namespace PinePhoneCore.Helpers
{
    public struct NativeInputEvent
    {
        public ulong Seconds;
        public ulong Microseconds;
        public ushort Type;
        public ushort Code;
        public uint Value;
    }
}
