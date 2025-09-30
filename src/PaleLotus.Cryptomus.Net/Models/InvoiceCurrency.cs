using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record InvoiceCurrency(
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("network")] string? Network = null
);
