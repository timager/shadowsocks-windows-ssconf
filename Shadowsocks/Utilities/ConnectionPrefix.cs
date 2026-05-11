using System;
using System.Collections.Generic;
using System.Text;

namespace Shadowsocks.Utilities
{
    /// <summary>
    /// Helpers for encoding the optional Shadowsocks connection prefix.
    /// </summary>
    public static class ConnectionPrefix
    {
        /// <summary>
        /// Decodes a configured prefix into bytes sent before the first encrypted TCP payload.
        /// Prefix values are stored as strings; JSON unicode escapes are already decoded by
        /// the JSON parser. Percent-encoded bytes are accepted for compatibility with
        /// Outline-style prefix values.
        /// </summary>
        public static byte[] Decode(string? prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return Array.Empty<byte>();

            if (!prefix.Contains('%'))
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
            value is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';

        private static int FromHex(char value) => value switch
        {
            >= '0' and <= '9' => value - '0',
            >= 'a' and <= 'f' => value - 'a' + 10,
            >= 'A' and <= 'F' => value - 'A' + 10,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }
}
