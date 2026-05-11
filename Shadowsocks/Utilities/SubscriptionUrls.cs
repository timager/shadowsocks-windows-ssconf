using System;

namespace Shadowsocks.Utilities
{
    /// <summary>
    /// Helpers for normalizing online configuration subscription URLs.
    /// </summary>
    public static class SubscriptionUrls
    {
        private const string SsconfScheme = "ssconf";

        /// <summary>
        /// Returns whether the input is a supported online configuration URL.
        /// </summary>
        public static bool IsSupported(string? raw)
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

        /// <summary>
        /// Parses HTTP(S) and ssconf online configuration URLs.
        /// ssconf:// links are transformed into HTTPS URLs while preserving authority,
        /// path, query, and fragment. This matches the Android ssconf client behavior.
        /// </summary>
        public static Uri Parse(string raw)
        {
            if (!Uri.TryCreate(raw.Trim(), UriKind.Absolute, out var uri))
                throw new UriFormatException("Invalid subscription URL.");

            return uri.Scheme.ToLowerInvariant() switch
            {
                "http" or "https" => uri,
                SsconfScheme => ConvertSsconfToHttps(uri),
                _ => throw new UriFormatException($"Unsupported subscription URL scheme: {uri.Scheme}"),
            };
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
