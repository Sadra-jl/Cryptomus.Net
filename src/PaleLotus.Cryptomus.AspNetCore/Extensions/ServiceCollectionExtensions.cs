using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaleLotus.Cryptomus.AspNetCore.Health;
using PaleLotus.Cryptomus.AspNetCore.Webhooks;
using PaleLotus.Cryptomus.Net;
using PaleLotus.Cryptomus.Net.Abstractions;

namespace PaleLotus.Cryptomus.AspNetCore.Extensions;

/// <summary>
/// ASP.NET Core specific helpers for configuring the Cryptomus SDK.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the core Cryptomus SDK and ASP.NET helpers (health checks, ProblemDetails).
    /// </summary>
    /// <param name="services">The service collection to register components with.</param>
    /// <param name="configure">Delegate used to configure <see cref="CryptomusOptions"/>.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddCryptomusAspNet(this IServiceCollection services, Action<CryptomusOptions> configure)
    {
        services.AddCryptomus(configure);

        services.AddProblemDetails();

        services.AddHealthChecks().AddCheck<CryptomusHealthCheck>(
            "cryptomus",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["ready", "live"]);

        return services;
    }

    /// <summary>
    /// Adds a convenient mapping for a verified webhook endpoint (Minimal API style).
    /// </summary>
    /// <typeparam name="TPayload">The strongly typed payload bound from the JSON webhook body.</typeparam>
    /// <param name="app">The endpoint route builder to register the route against.</param>
    /// <param name="pattern">The route pattern for the webhook endpoint.</param>
    /// <param name="handler">Delegate executed after the webhook signature has been validated.</param>
    /// <param name="webhookType">Determines which API key should be used during signature verification.</param>
    /// <returns>The configured <see cref="RouteHandlerBuilder"/>.</returns>
    public static RouteHandlerBuilder MapCryptomusWebhook<TPayload>(
        this IEndpointRouteBuilder app,
        string pattern,
        Func<HttpContext, TPayload, Task<IResult>> handler,
        WebhookType webhookType = WebhookType.Payment)
    {
        return app.MapPost(pattern, async (HttpContext http, TPayload payload) =>
            {
                // The filter/middleware should already verify signature if attached.
                return await handler(http, payload).ConfigureAwait(false);
            })
            .AddEndpointFilter(new WebhookVerificationFilter(webhookType));
    }
}