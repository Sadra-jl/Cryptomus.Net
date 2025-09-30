using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlReportRequest(
    [property: JsonPropertyName("request_id")] string RequestId
);