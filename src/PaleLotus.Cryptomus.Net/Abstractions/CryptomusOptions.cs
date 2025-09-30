using System.Text.Json;

namespace PaleLotus.Cryptomus.Net.Abstractions;

/// <summary>Options for configuring the Cryptomus SDK.</summary>
public sealed class CryptomusOptions
{
    /// <summary>Merchant UUID (required).</summary>
    public string MerchantId { get; set; } = string.Empty;

    /// <summary>Payment API key (used for payment endpoints and payment webhooks).</summary>
    public string? PaymentApiKey { get; set; }

    /// <summary>Payout API key (used for payout endpoints and payout webhooks).</summary>
    public string? PayoutApiKey { get; set; }

    /// <summary>Base URL for the API. Defaults to https://api.cryptomus.com/v1/</summary>
    public string BaseUrl { get; set; } = "https://api.cryptomus.com/v1/";

    /// <summary>Optional JSON options used for serialization.</summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}