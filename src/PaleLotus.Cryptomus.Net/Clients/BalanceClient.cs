using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Balances.</summary>
public sealed class BalanceClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    private const string PaymentClientName = "CryptomusPayment";

    private string RequirePaymentKey() => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<BalanceResponse>> GetPersonalAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<BalanceResponse>>(PaymentClientName, "balance/personal", new { }, RequirePaymentKey(), ct);

    public Task<ApiResponse<BalanceResponse>> GetBusinessAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<BalanceResponse>>(PaymentClientName, "balance/business", new { }, RequirePaymentKey(), ct);
}