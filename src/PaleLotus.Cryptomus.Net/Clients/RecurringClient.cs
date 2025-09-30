using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Recurring payments operations.</summary>
public sealed class RecurringClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    private const string PaymentClientName = "CryptomusPayment"; // recurring uses payment key

    private string RequirePaymentKey()
        => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<CreateRecurringResponse>> CreateAsync(CreateRecurringRequest request, CancellationToken ct = default)
        => http.PostAsync<CreateRecurringRequest, ApiResponse<CreateRecurringResponse>>(PaymentClientName, "recurring/create", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<RecurringInfoResponse>> GetInfoAsync(RecurringInfoRequest request, CancellationToken ct = default)
        => http.PostAsync<RecurringInfoRequest, ApiResponse<RecurringInfoResponse>>(PaymentClientName, "recurring/info", request, RequirePaymentKey(), ct);

    public async IAsyncEnumerable<RecurringItem> ListAsync(RecurringListRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var cursor = request.Cursor;
        while (true)
        {
            var resp = await http.PostAsync<RecurringListRequest, ApiResponse<CursorPage<RecurringItem>>>(
                PaymentClientName, "recurring/list", request with { Cursor = cursor }, RequirePaymentKey(), ct).ConfigureAwait(false);

            var page = resp.Result ?? new CursorPage<RecurringItem>([], null);
            foreach (var item in page.Items)
                yield return item;

            if (string.IsNullOrEmpty(page.NextCursor))
                yield break;

            cursor = page.NextCursor;
        }
    }

    public Task<ApiResponse<object>> CancelAsync(CancelRecurringRequest request, CancellationToken ct = default)
        => http.PostAsync<CancelRecurringRequest, ApiResponse<object>>(PaymentClientName, "recurring/cancel", request, RequirePaymentKey(), ct);
}