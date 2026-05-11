using Shadowsocks.Utilities;
using System;
using Xunit;

namespace Shadowsocks.Tests
{
    public class SubscriptionUrlsTests
    {
        [Theory]
        [InlineData("https://example.com/sub.json", "https://example.com/sub.json")]
        [InlineData("http://example.com/sub.json", "http://example.com/sub.json")]
        [InlineData("ssconf://example.com/sub.json?token=abc#frag", "https://example.com/sub.json?token=abc#frag")]
        [InlineData(" ssconf://example.com:8443/path ", "https://example.com:8443/path")]
        public void Parse_SupportedSubscriptionUrls(string raw, string expected)
        {
            var actual = SubscriptionUrls.Parse(raw);

            Assert.Equal(expected, actual.AbsoluteUri);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ss://example.com")]
        [InlineData("ftp://example.com/sub.json")]
        [InlineData("ssconf:/missing-authority")]
        public void Parse_RejectsUnsupportedSubscriptionUrls(string raw)
        {
            Assert.Throws<UriFormatException>(() => SubscriptionUrls.Parse(raw));
        }
    }
}
