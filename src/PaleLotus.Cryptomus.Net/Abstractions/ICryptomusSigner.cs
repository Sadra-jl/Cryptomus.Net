using System.Text.Json;

namespace PaleLotus.Cryptomus.Net.Abstractions;

/// <summary>Computes request/webhook signatures.</summary>
public interface ICryptomusSigner
{
    /// <summary>
    /// Compute signature as MD5( Base64(UTF8(JSON)) + apiKey ).
    /// Accepts raw JSON, JsonElement/JsonDocument, or an arbitrary object (which will be serialized).
    /// </summary>
    string ComputeSign(object? payload, string apiKey, JsonSerializerOptions? jsonOptions = null);
}