using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record BalanceResponse(
    [property: JsonPropertyName("balances")] IReadOnlyDictionary<string, string> Balances
);