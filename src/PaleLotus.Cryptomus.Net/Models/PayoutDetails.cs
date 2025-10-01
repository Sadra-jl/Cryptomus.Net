using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public record PayoutDetails
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; init; }

    [JsonPropertyName("amount")]
    public required string Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("address")]
    public required string Address { get; init; }

    [JsonPropertyName("network")]
    public string? Network { get; init; }

    [JsonPropertyName("txid")]
    public string? TransactionId { get; init; }

    [JsonPropertyName("status")]
    public string? Status { get; init; }

    [JsonPropertyName("is_final")]
    public bool? IsFinal { get; init; }

    [JsonPropertyName("balance")]
    public string? Balance { get; init; }

    [JsonPropertyName("payer_currency")]
    public string? PayerCurrency { get; init; }

    [JsonPropertyName("payer_amount")]
    public string? PayerAmount { get; init; }

    [JsonPropertyName("commission")]
    public string? Commission { get; init; }

    [JsonPropertyName("merchant_amount")]
    public string? MerchantAmount { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }
}
