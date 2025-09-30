using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record PayoutHistoryRequest(
    [property: JsonPropertyName("cursor")] string? Cursor = null,
    [property: JsonPropertyName("page")] int? Page = 1,
    [property: JsonPropertyName("limit")] int? Limit = 100,
    [property: JsonPropertyName("date_from")] string? DateFrom = null,
    [property: JsonPropertyName("date_to")] string? DateTo = null
);
