using Shadowsocks.Utilities;
using Xunit;

namespace Shadowsocks.Tests
{
    public class ConnectionPrefixTests
    {
        [Fact]
        public void Decode_ReturnsEmptyArrayForMissingPrefix()
        {
            Assert.Empty(ConnectionPrefix.Decode(null));
            Assert.Empty(ConnectionPrefix.Decode(""));
        }

        [Fact]
        public void Decode_ReturnsUtf8BytesForPlainTextPrefix()
        {
            Assert.Equal(new byte[] { 0x43, 0x4f, 0x4e, 0x4e, 0x45, 0x43, 0x54, 0x20 }, ConnectionPrefix.Decode("CONNECT "));
        }

        [Fact]
        public void Decode_ReturnsBytesForPercentEncodedPrefix()
        {
            Assert.Equal(new byte[] { 0x16, 0x03, 0x01, 0xff }, ConnectionPrefix.Decode("%16%03%01%ff"));
        }
    }
}
