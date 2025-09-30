using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record Pagination(
    [property: JsonPropertyName("count")] int? Count,
    [property: JsonPropertyName("hasPages")] bool? HasPages,
    [property: JsonPropertyName("nextCursor")] string? NextCursor,
    [property: JsonPropertyName("previousCursor")] string? PreviousCursor,
    [property: JsonPropertyName("perPage")] int? PerPage
);
