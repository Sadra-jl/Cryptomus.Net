using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record ResendWebhookRequest(
    [property: JsonPropertyName("uuid")] string? Uuid = null,
    [property: JsonPropertyName("order_id")] string? OrderId = null
);