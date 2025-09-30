using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreateStaticWalletResponse(
    [property: JsonPropertyName("wallet_uuid")] string WalletUuid,
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("network")] string Network,
    [property: JsonPropertyName("url")] string Url
);