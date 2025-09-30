using Microsoft.Extensions.DependencyInjection;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Clients;
using PaleLotus.Cryptomus.Net.Internal;

namespace PaleLotus.Cryptomus.Net;

/// <summary>
/// Extension methods for wiring the Cryptomus SDK into a dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the Cryptomus HTTP clients and typed client facades in the supplied <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <param name="configure">Callback used to configure <see cref="CryptomusOptions"/>.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    public static IServiceCollection AddCryptomus(this IServiceCollection services, Action<CryptomusOptions> configure)
    {
        services.AddOptions<CryptomusOptions>().Configure(configure);

        services.AddSingleton<ICryptomusSigner, DefaultCryptomusSigner>();
        services.AddSingleton<CryptomusHttp>();

        // Payment client
        services.AddHttpClient(PaymentsClient.PaymentClientName)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            //.AddPolicyHandler(GetRetryPolicy());

        // Payout client
        services.AddHttpClient(PayoutsClient.PayoutClientName)
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            //.AddPolicyHandler(GetRetryPolicy());

        services.AddTransient<PaymentsClient>();
        services.AddTransient<PayoutsClient>();
        services.AddTransient<RecurringClient>();
        services.AddTransient<RatesClient>();
        services.AddTransient<DiscountsClient>();
        services.AddTransient<BalanceClient>();
        services.AddTransient<AmlClient>();

        services.AddSingleton<CryptomusClient>();

        return services;
    }

    // private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    //     HttpPolicyExtensions
    //         .HandleTransientHttpError()
    //         .OrResult(msg => (int)msg.StatusCode == 429)
    //         .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
}