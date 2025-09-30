using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record ExchangeRatesResponse(
    [property: JsonPropertyName("rates")] IReadOnlyDictionary<string, decimal> Rates
);
public sealed record ExchangeRateItem(
    [property: JsonPropertyName("from")] string From,
    [property: JsonPropertyName("to")] string To,
    [property: JsonPropertyName("course")] string Course
);