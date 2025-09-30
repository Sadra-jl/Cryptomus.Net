using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record RefundPaymentRequest(
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("is_subtract")] bool IsSubtract,
    [property: JsonPropertyName("uuid")] string? Uuid = null,
    [property: JsonPropertyName("order_id")] string? OrderId = null
);