using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Payments operations.</summary>
public sealed class PaymentsClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    internal const string PaymentClientName = "CryptomusPayment";

    private string RequirePaymentKey()
        => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<CreateInvoiceResponse>> CreateInvoiceAsync(CreateInvoiceRequest request, CancellationToken ct = default)
        => http.PostAsync<CreateInvoiceRequest, ApiResponse<CreateInvoiceResponse>>(PaymentClientName, "payment", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<PaymentInfoResponse>> GetPaymentInfoAsync(PaymentInfoRequest request, CancellationToken ct = default)
        => http.PostAsync<PaymentInfoRequest, ApiResponse<PaymentInfoResponse>>(PaymentClientName, "payment/info", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<CreateStaticWalletResponse>> CreateStaticWalletAsync(CreateStaticWalletRequest request, CancellationToken ct = default)
        => http.PostAsync<CreateStaticWalletRequest, ApiResponse<CreateStaticWalletResponse>>(PaymentClientName, "wallet", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<BlockStaticWalletResponse>> BlockStaticWalletAsync(BlockStaticWalletRequest request, CancellationToken ct = default)
        => http.PostAsync<BlockStaticWalletRequest, ApiResponse<BlockStaticWalletResponse>>(PaymentClientName, "wallet/block-address", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<RefundBlockedWalletResponse>> RefundBlockedWalletAsync(RefundBlockedWalletRequest request, CancellationToken ct = default)
        => http.PostAsync<RefundBlockedWalletRequest, ApiResponse<RefundBlockedWalletResponse>>(PaymentClientName, "wallet/blocked-address-refund", request, RequirePaymentKey(), ct);

    [Obsolete("Use GenerateWalletQrAsync instead.")]
    public Task<ApiResponse<GenerateQrResponse>> GenerateQrAsync(GenerateWalletQrRequest request, CancellationToken ct = default)
        => GenerateWalletQrAsync(request, ct);

    public Task<ApiResponse<GenerateQrResponse>> GenerateWalletQrAsync(GenerateWalletQrRequest request, CancellationToken ct = default)
        => http.PostAsync<GenerateWalletQrRequest, ApiResponse<GenerateQrResponse>>(PaymentClientName, "wallet/qr", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<GenerateQrResponse>> GeneratePaymentQrAsync(GeneratePaymentQrRequest request, CancellationToken ct = default)
        => http.PostAsync<GeneratePaymentQrRequest, ApiResponse<GenerateQrResponse>>(PaymentClientName, "payment/qr", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<IReadOnlyList<string>>> RefundPaymentAsync(RefundPaymentRequest request, CancellationToken ct = default)
        => http.PostAsync<RefundPaymentRequest, ApiResponse<IReadOnlyList<string>>>(PaymentClientName, "payment/refund", request, RequirePaymentKey(), ct);

    public async IAsyncEnumerable<PaymentHistoryItem> GetPaymentHistoryAsync(PaymentHistoryRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    {
        var cursor = request.Cursor;
        int? page = request.Page is > 0 ? request.Page : 1;

        while (true)
        {
            var payload = request with { Cursor = cursor, Page = page };
            var response = await http.PostAsync<PaymentHistoryRequest, ApiResponse<IReadOnlyList<PaymentHistoryItem>>>(
                PaymentClientName,
                "payment/list",
                payload,
                RequirePaymentKey(),
                ct).ConfigureAwait(false);

            var items = response.Result ?? Array.Empty<PaymentHistoryItem>();
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

    public Task<ApiResponse<IReadOnlyList<PaymentServiceInfo>>> ListServicesAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<IReadOnlyList<PaymentServiceInfo>>>(PaymentClientName, "payment/services", new { }, RequirePaymentKey(), ct);

    public Task<ApiResponse<object>> ResendWebhookAsync(ResendWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<ResendWebhookRequest, ApiResponse<object>>(PaymentClientName, "payment/resend", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<object>> TestWebhookAsync(TestWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<TestWebhookRequest, ApiResponse<object>>(PaymentClientName, "test-webhook/payment", request, RequirePaymentKey(), ct);
}
