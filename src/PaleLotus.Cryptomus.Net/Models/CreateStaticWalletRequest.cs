using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreateStaticWalletRequest(
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("network")] string Network,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("url_callback")] string? UrlCallback = null,
    [property: JsonPropertyName("from_referral_code")] string? FromReferralCode = null
);