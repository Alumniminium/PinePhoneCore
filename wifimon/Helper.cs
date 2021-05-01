using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wifimon
{
    public static class Helper
    {        
        static Dictionary<int, int> _dbPercentTable = new Dictionary<int, int>
        {
            [-1] = 100,[-26] = 98,[-51] = 78,[-76] = 38,[-2] = 100,[-27] = 97,
            [-52] = 76,[-77] = 36,[-3] = 100,[-28] = 97, [-53] = 75,[-78] = 34, [-4] = 100,
            [-29] = 96,[-54] = 74,[-79] = 32,[-5] = 100,[-30] = 96,[-55] = 73,[-80] = 30,[-6] = 100,
            [-31] = 95,[-56] = 71,[-81] = 28,[-7] = 100,[-32] = 95,[-57] = 70,[-82] = 26,[-8] = 100,
            [-33] = 94,[-58] = 69,[-83] = 24,[-9] = 100,[-34] = 93,[-59] = 67,[-84] = 22,[-10] = 100,
            [-35] = 93,[-60] = 66,[-85] = 20,[-11] = 100,[-36] = 92,[-61] = 64,[-86] = 17,[-12] = 100,
            [-37] = 91,[-62] = 63,[-87] = 15,[-13] = 100,[-38] = 90,[-63] = 61,[-88] = 13,[-14] = 100,
            [-39] = 90,[-64] = 60,[-89] = 10,[-15] = 100,[-40] = 89,[-65] = 58,[-90] = 8, [-16] = 100,
            [-41] = 88,[-66] = 56,[-91] = 6, [-17] = 100,[-42] = 87,[-67] = 55,[-92] = 3, [-18] = 100,
            [-43] = 86,[-68] = 53,[-93] = 1, [-19] = 100,[-44] = 85,[-69] = 51,[-94] = 1,[-20] = 100,
            [-45] = 84,[-70] = 50,[-95] = 1,[-21] = 99,[-46] = 83,[-71] = 48,[-96] = 1,[-22] = 99,
            [-47] = 82,[-72] = 46,[-97] = 1,[-23] = 99,[-48] = 81,[-73] = 44,[-98] = 1,[-24] = 98,
            [-49] = 80,[-74] = 42,[-99] = 1,[-25] = 98,[-50] = 79,[-75] = 40,[-100] = 1,
        };
        public static string DbToPercent(int db)
        {
            if(db < -100)
                return "0%";
            if(db > -1)
                return "100%";
            
            return $"{_dbPercentTable[db]}%";
        }
        
        public static string HexDump(byte[] bytes, int bytesPerLine)
        {
            var sb = new StringBuilder();
            for (int line = 0; line < bytes.Length; line += bytesPerLine)
            {
                byte[] lineBytes = bytes.Skip(line).Take(bytesPerLine).ToArray();
                sb.AppendFormat("{0:x8} ", line);
                sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
                       .ToArray()).PadRight(bytesPerLine * 3));
                sb.Append(" ");
                sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char)b)
                       .ToArray()));
                // sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}