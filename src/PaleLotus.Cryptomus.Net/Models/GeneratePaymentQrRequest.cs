using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record GeneratePaymentQrRequest(
    [property: JsonPropertyName("merchant_payment_uuid")] string MerchantPaymentUuid
);
