using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record BlockStaticWalletRequest(
    [property: JsonPropertyName("uuid")] string? Uuid = null,
    [property: JsonPropertyName("order_id")] string? OrderId = null,
    [property: JsonPropertyName("is_force_refund")] bool? IsForceRefund = null
);