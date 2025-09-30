using System.Text.Json.Serialization;

namespace PaleLotus.Cryptomus.Net.Models;

public record ServiceInfo
{
    [JsonPropertyName("network")]
    public required string Network { get; init; }

    [JsonPropertyName("currency")]
    public required string Currency { get; init; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; init; }

    [JsonPropertyName("limit")]
    public ServiceLimit? Limit { get; init; }

    [JsonPropertyName("commision")]
    public ServiceCommission? Commission { get; init; }
}

public sealed record PaymentServiceInfo : ServiceInfo;

public sealed record PayoutServiceInfo : ServiceInfo;

public sealed record ServiceLimit(
    [property: JsonPropertyName("minAmount")] string? MinAmount,
    [property: JsonPropertyName("maxAmount")] string? MaxAmount
);

public sealed record ServiceCommission(
    [property: JsonPropertyName("feeAmount")] string? FeeAmount,
    [property: JsonPropertyName("percent")] string? Percent
);
