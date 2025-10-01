using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Payouts operations.</summary>
public sealed class PayoutsClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    internal const string PayoutClientName = "CryptomusPayout";

    private string RequirePayoutKey()
        => opts.Value.PayoutApiKey ?? throw new InvalidOperationException("PayoutApiKey is not configured.");

    public Task<ApiResponse<CreatePayoutResponse>> CreatePayoutAsync(CreatePayoutRequest request, CancellationToken ct = default)
        => http.PostAsync<CreatePayoutRequest, ApiResponse<CreatePayoutResponse>>(PayoutClientName, "payout", request, RequirePayoutKey(), ct);

    public Task<ApiResponse<PayoutInfoResponse>> GetPayoutInfoAsync(PayoutInfoRequest request, CancellationToken ct = default)
        => http.PostAsync<PayoutInfoRequest, ApiResponse<PayoutInfoResponse>>(PayoutClientName, "payout/info", request, RequirePayoutKey(), ct);

    public IAsyncEnumerable<PayoutHistoryItem> GetPayoutHistoryAsync(
        PayoutHistoryRequest request,
        CancellationToken ct = default)
    {
        return PaginationHelper.Iterate<PayoutHistoryRequest, PayoutHistoryItem>(
            initialRequest: request,
            fetch: (req, token) => http.PostAsync<PayoutHistoryRequest, ApiResponse<IReadOnlyList<PayoutHistoryItem>>>(
                PayoutClientName,
                "payout/list",
                req,
                RequirePayoutKey(),
                token),

            withPage: static (req, page) => req with { Page = page, Cursor = null },
            withCursor: static (req, cursor) => req with { Cursor = cursor, Page = null },
            getFirstPage: static req => req.Page is > 0 ? req.Page.Value : 1,
            hasExplicitCursor: static req => !string.IsNullOrEmpty(req.Cursor),
            getCursor: static req => req.Cursor,

            ct: ct
        );
    }
    public Task<ApiResponse<PayoutStatusesResponse>> GetPayoutStatusesAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<PayoutStatusesResponse>>(PayoutClientName, "payout/statuses", new { }, RequirePayoutKey(), ct);

    public Task<ApiResponse<IReadOnlyList<PayoutServiceInfo>>> ListServicesAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<IReadOnlyList<PayoutServiceInfo>>>(PayoutClientName, "payout/services", new { }, RequirePayoutKey(), ct);

    public Task<ApiResponse<object>> TestWebhookAsync(TestWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<TestWebhookRequest, ApiResponse<object>>(PayoutClientName, "test-webhook/payout", request, RequirePayoutKey(), ct);

    public Task<ApiResponse<object>> ResendWebhookAsync(ResendWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<ResendWebhookRequest, ApiResponse<object>>(PayoutClientName, "payout/resend", request, RequirePayoutKey(), ct);
}
