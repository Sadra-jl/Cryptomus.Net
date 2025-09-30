using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record DiscountItem(
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("percent")] decimal Percent
);