using System;
using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public record PaymentDetails
{
    [JsonPropertyName("uuid")]
    public required string Uuid { get; init; }

    [JsonPropertyName("order_id")]
    public required string OrderId { get; init; }

    [JsonPropertyName("amount")]
    public required string Amount { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("payment_status")]
    public required string PaymentStatus { get; init; }

    [JsonPropertyName("status")]
    public required string Status { get; init; }

    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("payment_amount")]
    public string? PaymentAmount { get; init; }

    [JsonPropertyName("payment_amount_usd")]
    public string? PaymentAmountUsd { get; init; }

    [JsonPropertyName("payer_amount")]
    public string? PayerAmount { get; init; }

    [JsonPropertyName("payer_amount_exchange_rate")]
    public string? PayerAmountExchangeRate { get; init; }

    [JsonPropertyName("discount_percent")]
    public string? DiscountPercent { get; init; }

    [JsonPropertyName("discount")]
    public string? Discount { get; init; }

    [JsonPropertyName("payer_currency")]
    public string? PayerCurrency { get; init; }

    [JsonPropertyName("merchant_amount")]
    public string? MerchantAmount { get; init; }

    [JsonPropertyName("commission")]
    public string? Commission { get; init; }

    [JsonPropertyName("network")]
    public string? Network { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("from")]
    public string? From { get; init; }

    [JsonPropertyName("txid")]
    public string? TransactionId { get; init; }

    [JsonPropertyName("expired_at")]
    public long? ExpiredAt { get; init; }

    [JsonPropertyName("is_final")]
    public bool? IsFinal { get; init; }

    [JsonPropertyName("additional_data")]
    public string? AdditionalData { get; init; }

    [JsonPropertyName("comments")]
    public string? Comments { get; init; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }
}
