using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record PayoutStatusesResponse(
    [property: JsonPropertyName("statuses")] IReadOnlyList<string> Statuses
);