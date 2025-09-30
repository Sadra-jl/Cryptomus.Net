using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreatePayoutRequest(
    [property: JsonPropertyName("amount")] string Amount,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("is_subtract")] bool IsSubtract,
    [property: JsonPropertyName("network")] string? Network = null,
    [property: JsonPropertyName("url_callback")] string? UrlCallback = null,
    [property: JsonPropertyName("to_currency")] string? ToCurrency = null,
    [property: JsonPropertyName("course_source")] string? CourseSource = null,
    [property: JsonPropertyName("from_currency")] string? FromCurrency = null,
    [property: JsonPropertyName("priority")] string? Priority = null,
    [property: JsonPropertyName("memo")] string? Memo = null
);