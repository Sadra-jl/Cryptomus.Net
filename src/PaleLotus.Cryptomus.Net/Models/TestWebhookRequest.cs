using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record TestWebhookRequest(
    [property: JsonPropertyName("url_callback")] string UrlCallback,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("network")] string Network,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("uuid")] string? Uuid = null,
    [property: JsonPropertyName("order_id")] string? OrderId = null
);
