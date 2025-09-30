using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record DiscountListResponse(
    [property: JsonPropertyName("items")] IReadOnlyList<DiscountItem> Items
);