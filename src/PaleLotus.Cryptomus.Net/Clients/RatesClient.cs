using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Exchange rates.</summary>
public sealed class RatesClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    private const string PaymentClientName = "CryptomusPayment";

    private string RequirePaymentKey() => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<ExchangeRatesResponse>> ListAsync(CancellationToken ct = default)
        => http.GetAsync<ApiResponse<ExchangeRatesResponse>>(PaymentClientName, "rates/list", RequirePaymentKey(), ct);
    public Task<ApiResponse<IReadOnlyList<ExchangeRateItem>>> ListAsync(string currency, CancellationToken ct = default) => 
        http.GetAsync<ApiResponse<IReadOnlyList<ExchangeRateItem>>>(
            PaymentClientName, 
            $"exchange-rate/{currency}/list", 
            RequirePaymentKey(), 
            ct);
    
}