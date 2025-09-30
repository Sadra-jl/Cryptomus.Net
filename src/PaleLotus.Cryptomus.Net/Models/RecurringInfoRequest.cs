using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record RecurringInfoRequest(
    [property: JsonPropertyName("uuid")] string Uuid
);