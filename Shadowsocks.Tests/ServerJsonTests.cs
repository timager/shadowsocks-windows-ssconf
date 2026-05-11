using Shadowsocks.Models;
using System.Text.Json;
using Xunit;

namespace Shadowsocks.Tests
{
    public class ServerJsonTests
    {
        [Fact]
        public void Deserialize_ReadsSsconfPrefixFromSip008PrefixProperty()
        {
            const string json = """
            {
              "server": "example.com",
              "server_port": 443,
              "password": "secret",
              "method": "chacha20-ietf-poly1305",
              "remarks": "prefixed",
              "prefix": "\u0016\u0003\u0001"
            }
            """;

            var server = JsonSerializer.Deserialize<Server>(json);

            Assert.NotNull(server);
            Assert.Equal("\u0016\u0003\u0001", server!.SsconfPrefix);
        }

        [Fact]
        public void Serialize_WritesSsconfPrefixAsSip008PrefixProperty()
        {
            var server = new Server
            {
                Host = "example.com",
                Port = 443,
                Password = "secret",
                Method = "chacha20-ietf-poly1305",
                Name = "prefixed",
                SsconfPrefix = "CONNECT ",
            };

            var json = JsonSerializer.Serialize(server);

            Assert.Contains("\"prefix\":\"CONNECT \", json);
        }
    }
}
