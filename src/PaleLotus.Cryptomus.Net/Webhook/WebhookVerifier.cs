using System.Security.Cryptography;
using System.Text;

namespace PaleLotus.Cryptomus.Net.Webhook;

/// <summary>Helpers to verify Cryptomus webhook signatures.</summary>
public static class WebhookVerifier
{
    /// <summary>
    /// Verifies awebhook signature using MD5(Base64(json) + apiKey) with constant-time comparison.
    /// </summary>
    public static bool Verify(ReadOnlySpan<byte> rawJsonUtf8, string apiKey, ReadOnlySpan<char> providedSignLowerHex)
    {
        var base64 = Convert.ToBase64String(rawJsonUtf8);
#pragma warning disable CA5351
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(base64 + apiKey));
        var expected = Convert.ToHexString(hash).ToLowerInvariant();
#pragma warning restore CA5351

        // Constant-time compare
        var a = Encoding.ASCII.GetBytes(expected);
        var b = Encoding.ASCII.GetBytes(providedSignLowerHex.ToString());
        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    /// <summary>Convenience for string JSON bodies.</summary>
    public static bool Verify(string rawJson, string apiKey, string providedSignLowerHex)
        => Verify(Encoding.UTF8.GetBytes(rawJson), apiKey, providedSignLowerHex);
}