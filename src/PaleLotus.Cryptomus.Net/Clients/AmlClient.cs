using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Internal;
using PaleLotus.Cryptomus.Net.Models;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>AML checks.</summary>
public sealed class AmlClient(CryptomusHttp http, IOptions<CryptomusOptions> opts)
{
    private const string PaymentClientName = "CryptomusPayment";

    private string RequirePaymentKey() => opts.Value.PaymentApiKey ?? throw new InvalidOperationException("PaymentApiKey is not configured.");

    public Task<ApiResponse<AmlAvailableChecksResponse>> AvailableAsync(CancellationToken ct = default)
        => http.PostAsync<object, ApiResponse<AmlAvailableChecksResponse>>(PaymentClientName, "aml/available", new { }, RequirePaymentKey(), ct);

    public Task<ApiResponse<AmlCreateResponse>> CreateAsync(AmlCreateRequest request, CancellationToken ct = default)
        => http.PostAsync<AmlCreateRequest, ApiResponse<AmlCreateResponse>>(PaymentClientName, "aml/create", request, RequirePaymentKey(), ct);

    public async IAsyncEnumerable<AmlHistoryItem> HistoryAsync(AmlHistoryRequest request, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        var cursor = request.Cursor;
        while (true)
        {
            var resp = await http.PostAsync<AmlHistoryRequest, ApiResponse<CursorPage<AmlHistoryItem>>>(
                PaymentClientName, "aml/history", request with { Cursor = cursor }, RequirePaymentKey(), ct).ConfigureAwait(false);

            var page = resp.Result ?? new CursorPage<AmlHistoryItem>([], null);
            foreach (var item in page.Items)
                yield return item;

            if (string.IsNullOrEmpty(page.NextCursor))
                yield break;

            cursor = page.NextCursor;
        }
    }

    public Task<ApiResponse<AmlReportResponse>> ReportAsync(AmlReportRequest request, CancellationToken ct = default)
        => http.PostAsync<AmlReportRequest, ApiResponse<AmlReportResponse>>(PaymentClientName, "aml/report", request, RequirePaymentKey(), ct);
}