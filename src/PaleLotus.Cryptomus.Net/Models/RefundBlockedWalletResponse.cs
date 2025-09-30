using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record RefundBlockedWalletResponse(
    [property: JsonPropertyName("commission")] string Commission,
    [property: JsonPropertyName("amount")] string Amount
);