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

    public async IAsyncEnumerable<PayoutHistoryItem> GetPayoutHistoryAsync(PayoutHistoryRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var cursor = request.Cursor;
        int? page = request.Page is > 0 ? request.Page : 1;

        while (true)
        {
            var payload = request with { Cursor = cursor, Page = page };
            var response = await http.PostAsync<PayoutHistoryRequest, ApiResponse<IReadOnlyList<PayoutHistoryItem>>>(
                PayoutClientName,
                "payout/list",
                payload,
                RequirePayoutKey(),
                ct).ConfigureAwait(false);

            var items = response.Result ?? Array.Empty<PayoutHistoryItem>();
            foreach (var item in items)
                yield return item;

            var paginate = response.Paginate;
            if (paginate is null)
                yield break;

            if (!string.IsNullOrEmpty(paginate.NextCursor))
            {
                cursor = paginate.NextCursor;
                page = null;
                continue;
            }

            if (paginate.HasPages == true)
            {
                var currentPage = page is > 0 ? page.Value : 1;
                var perPage = paginate.PerPage ?? payload.Limit ?? items.Count;
                if (perPage <= 0)
                    perPage = items.Count == 0 ? 1 : items.Count;

                if (paginate.Count is int count && perPage > 0)
                {
                    var totalPages = (int)Math.Ceiling(count / (double)perPage);
                    if (currentPage >= totalPages)
                        yield break;
                }
                else if (items.Count == 0)
                {
                    yield break;
                }

                page = currentPage + 1;
                continue;
            }

            if (items.Count == 0)
                yield break;

            break;
        }
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
