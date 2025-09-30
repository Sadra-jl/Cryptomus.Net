using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlAvailableChecksResponse(
    [property: JsonPropertyName("checks")] IReadOnlyList<string> Checks
);