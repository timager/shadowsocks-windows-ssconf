using System;
using System.Collections.Generic;
using System.Text;

namespace Shadowsocks.Util
{
    public static class ConnectionPrefix
    {
        public static byte[] Decode(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return Array.Empty<byte>();

            if (!prefix.Contains("%"))
                return Encoding.UTF8.GetBytes(prefix);

            return DecodePercentEncodedBytes(prefix);
        }

        private static byte[] DecodePercentEncodedBytes(string prefix)
        {
            var bytes = new List<byte>(prefix.Length);
            for (var i = 0; i < prefix.Length; i++)
            {
                if (prefix[i] == '%' && i + 2 < prefix.Length && IsHex(prefix[i + 1]) && IsHex(prefix[i + 2]))
                {
                    bytes.Add((byte)((FromHex(prefix[i + 1]) << 4) + FromHex(prefix[i + 2])));
                    i += 2;
                }
                else
                {
                    bytes.AddRange(Encoding.UTF8.GetBytes(prefix[i].ToString()));
                }
            }

            return bytes.ToArray();
        }

        private static bool IsHex(char value) =>
            value >= '0' && value <= '9' || value >= 'a' && value <= 'f' || value >= 'A' && value <= 'F';

        private static int FromHex(char value)
        {
            if (value >= '0' && value <= '9') return value - '0';
            if (value >= 'a' && value <= 'f') return value - 'a' + 10;
            if (value >= 'A' && value <= 'F') return value - 'A' + 10;
            throw new ArgumentOutOfRangeException(nameof(value), value, null);
        }
    }
}
