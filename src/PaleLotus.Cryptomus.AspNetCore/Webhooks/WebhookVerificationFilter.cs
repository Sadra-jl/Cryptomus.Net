using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Webhook;

namespace PaleLotus.Cryptomus.AspNetCore.Webhooks;

/// <summary>Endpoint filter that verifies the Cryptomus webhook signature before invoking the handler.</summary>
public sealed class WebhookVerificationFilter(WebhookType type) : IEndpointFilter
{
    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync([NotNull]EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        if (!http.Request.Headers.TryGetValue("sign", out var signHeader) || string.IsNullOrWhiteSpace(signHeader))
            return Results.Unauthorized();

        http.Request.EnableBuffering();
        using var ms = new MemoryStream();
        await http.Request.Body.CopyToAsync(ms).ConfigureAwait(false);
        http.Request.Body.Position = 0;

        var raw = ms.ToArray();
        var options = http.RequestServices.GetRequiredService<IOptions<CryptomusOptions>>().Value;

        var apiKey = type == WebhookType.Payment ? options.PaymentApiKey : options.PayoutApiKey;
        if (string.IsNullOrEmpty(apiKey))
            return Results.Problem(statusCode: 500, title: "Cryptomus API key not configured for webhook verification.");

        var ok = WebhookVerifier.Verify(raw, apiKey!, signHeader.ToString());
        if (!ok)
            return Results.Unauthorized();

        return await next(context).ConfigureAwait(false);
    }
}