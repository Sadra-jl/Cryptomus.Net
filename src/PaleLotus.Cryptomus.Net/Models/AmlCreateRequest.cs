using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlCreateRequest(
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("currency")] string Currency
);