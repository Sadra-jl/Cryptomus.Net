using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record GenerateQrResponse(
    [property: JsonPropertyName("image")] string Base64Png
);