using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record RecurringItem(
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("amount")] string Amount,
    [property: JsonPropertyName("currency")] string Currency
);