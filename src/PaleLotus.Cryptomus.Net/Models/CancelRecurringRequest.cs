using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CancelRecurringRequest(
    [property: JsonPropertyName("uuid")] string Uuid
);