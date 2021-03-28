using System.Text;

namespace PinePhoneCore.Helpers
{
    public struct NativeInputEvent
    {
        public ulong Seconds;
        public ulong Microseconds;
        public ushort Type;
        public short Code;
        public uint Value;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{nameof(Seconds)}: {Seconds}");
            sb.AppendLine($"{nameof(Microseconds)}: {Microseconds}");
            sb.AppendLine($"{nameof(Type)}: {Type}");
            sb.AppendLine($"{nameof(Code)}: {Code}");
            sb.AppendLine($"{nameof(Value)}: {Value}");
            return sb.ToString();
        }
    }
}
