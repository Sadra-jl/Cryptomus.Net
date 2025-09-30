using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record GenerateWalletQrRequest(
    [property: JsonPropertyName("wallet_address_uuid")] string WalletAddressUuid
);
