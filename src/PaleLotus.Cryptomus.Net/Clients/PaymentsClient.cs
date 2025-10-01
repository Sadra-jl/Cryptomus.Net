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

    public IAsyncEnumerable<PaymentHistoryItem> GetPaymentHistoryAsync(
        PaymentHistoryRequest request,
        CancellationToken ct = default)
    {
        return PaginationHelper.Iterate<PaymentHistoryRequest, PaymentHistoryItem>(
            initialRequest: request,
            fetch: (req, token) => http.PostAsync<PaymentHistoryRequest, ApiResponse<IReadOnlyList<PaymentHistoryItem>>>(
                PaymentClientName,
                "payment/list",
                req,
                RequirePaymentKey(),
                token),

            withPage: static (req, page) => req with { Page = page, Cursor = null },
            withCursor: static (req, cursor) => req with { Cursor = cursor, Page = null },
            getFirstPage: static req => req.Page is > 0 ? req.Page.Value : 1,
            hasExplicitCursor: static req => !string.IsNullOrEmpty(req.Cursor),
            getCursor: static req => req.Cursor,

            ct: ct
        );
    }

    public Task<ApiResponse<IReadOnlyList<PaymentServiceInfo>>> ListServicesAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<IReadOnlyList<PaymentServiceInfo>>>(PaymentClientName, "payment/services", new { }, RequirePaymentKey(), ct);

    public Task<ApiResponse<object>> ResendWebhookAsync(ResendWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<ResendWebhookRequest, ApiResponse<object>>(PaymentClientName, "payment/resend", request, RequirePaymentKey(), ct);

    public Task<ApiResponse<object>> TestWebhookAsync(TestWebhookRequest request, CancellationToken ct = default)
        => http.PostAsync<TestWebhookRequest, ApiResponse<object>>(PaymentClientName, "test-webhook/payment", request, RequirePaymentKey(), ct);
}
