using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreateRecurringRequest(
    [property: JsonPropertyName("amount")] string Amount,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("interval")] string Interval, // e.g., "monthly", "weekly"
    [property: JsonPropertyName("order_id")] string OrderId
);