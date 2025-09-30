using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record AmlHistoryItem(
    [property: JsonPropertyName("request_id")] string RequestId,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("result")] string Result
);