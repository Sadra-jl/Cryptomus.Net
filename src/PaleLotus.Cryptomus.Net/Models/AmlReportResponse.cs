using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlReportResponse(
    [property: JsonPropertyName("score")] decimal Score,
    [property: JsonPropertyName("details")] string Details
);