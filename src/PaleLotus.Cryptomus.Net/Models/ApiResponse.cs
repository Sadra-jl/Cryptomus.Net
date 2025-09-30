using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record ApiResponse<T>(
    [property: JsonPropertyName("state")] int State,
    [property: JsonPropertyName("result")] T? Result,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("paginate")] Pagination? Paginate
);
