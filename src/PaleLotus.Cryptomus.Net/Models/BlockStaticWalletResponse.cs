using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record BlockStaticWalletResponse(
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("status")] StaticWalletStatus Status
);