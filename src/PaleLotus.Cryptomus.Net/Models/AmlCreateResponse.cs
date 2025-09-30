using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlCreateResponse(
    [property: JsonPropertyName("request_id")] string RequestId
);