using System;

namespace Shadowsocks.Util
{
    public static class SubscriptionUrls
    {
        private const string SsconfScheme = "ssconf";

        public static bool IsSupported(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return false;

            try
            {
                Parse(raw);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Uri Parse(string raw)
        {
            if (!Uri.TryCreate(raw.Trim(), UriKind.Absolute, out var uri))
                throw new UriFormatException("Invalid subscription URL.");

            switch (uri.Scheme.ToLowerInvariant())
            {
                case "http":
                case "https":
                    return uri;
                case SsconfScheme:
                    return ConvertSsconfToHttps(uri);
                default:
                    throw new UriFormatException("Unsupported subscription URL scheme: " + uri.Scheme);
            }
        }

        private static Uri ConvertSsconfToHttps(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Host) && string.IsNullOrEmpty(uri.Authority))
                throw new UriFormatException("ssconf URL must include a host.");

            var builder = new UriBuilder(uri)
            {
                Scheme = Uri.UriSchemeHttps,
            };

            if (uri.IsDefaultPort)
                builder.Port = -1;

            return builder.Uri;
        }
    }
}
