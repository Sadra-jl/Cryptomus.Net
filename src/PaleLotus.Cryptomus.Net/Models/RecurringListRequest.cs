using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record RecurringListRequest(
    [property: JsonPropertyName("cursor")] string? Cursor = null,
    [property: JsonPropertyName("limit")] int? Limit = 100
);