using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record CreateInvoiceRequest(
    [property: JsonPropertyName("amount")] string Amount,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("network")] string? Network = null,
    [property: JsonPropertyName("url_return")] string? UrlReturn = null,
    [property: JsonPropertyName("url_success")] string? UrlSuccess = null,
    [property: JsonPropertyName("url_callback")] string? UrlCallback = null,
    [property: JsonPropertyName("is_payment_multiple")] bool? IsPaymentMultiple = null,
    [property: JsonPropertyName("lifetime")] int? LifetimeSeconds = null,
    [property: JsonPropertyName("to_currency")] string? ToCurrency = null,
    [property: JsonPropertyName("subtract")] int? SubtractPercent = null,
    [property: JsonPropertyName("accuracy_payment_percent")] decimal? AccuracyPaymentPercent = null,
    [property: JsonPropertyName("additional_data")] string? AdditionalData = null,
    [property: JsonPropertyName("currencies")] IReadOnlyList<InvoiceCurrency>? Currencies = null,
    [property: JsonPropertyName("except_currencies")] IReadOnlyList<InvoiceCurrency>? ExceptCurrencies = null,
    [property: JsonPropertyName("course_source")] string? CourseSource = null,
    [property: JsonPropertyName("from_referral_code")] string? FromReferralCode = null,
    [property: JsonPropertyName("discount_percent")] int? DiscountPercent = null,
    [property: JsonPropertyName("is_refresh")] bool? IsRefresh = null
);