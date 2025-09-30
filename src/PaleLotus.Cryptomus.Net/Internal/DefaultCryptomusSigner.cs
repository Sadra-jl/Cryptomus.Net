using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PaleLotus.Cryptomus.Net.Abstractions;

namespace PaleLotus.Cryptomus.Net.Internal;

internal sealed class DefaultCryptomusSigner : ICryptomusSigner
{
    public string ComputeSign(object? payload, string apiKey, JsonSerializerOptions? jsonOptions = null)
    {
        var json = payload switch
        {
            null => string.Empty,
            string s => s,
            JsonElement el => el.GetRawText(),
            JsonDocument doc => doc.RootElement.GetRawText(),
            _ => JsonSerializer.Serialize(payload, jsonOptions ?? new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            })
        };

        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
#pragma warning disable CA5351
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(base64 + apiKey));
#pragma warning restore CA5351
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}