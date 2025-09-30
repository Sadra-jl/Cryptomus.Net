using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreateRecurringResponse(
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("status")] string Status
);