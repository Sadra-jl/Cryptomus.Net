using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Discounts management.</summary>
public sealed class DiscountsClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    private const string PaymentClientName = "CryptomusPayment";

    private string RequirePaymentKey() => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<DiscountListResponse>> ListAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<DiscountListResponse>>(PaymentClientName, "discount/list", new { }, RequirePaymentKey(), ct);

    public Task<ApiResponse<object>> SetAsync(SetDiscountRequest request, CancellationToken ct = default)
        => http.PostAsync<SetDiscountRequest, ApiResponse<object>>(PaymentClientName, "discount/set", request, RequirePaymentKey(), ct);
}