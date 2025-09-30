using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaleLotus.Cryptomus.Net.Clients;

namespace PaleLotus.Cryptomus.AspNetCore.Health;

/// <summary>
/// Lightweight health check that verifies the Cryptomus API can be reached using the configured credentials.
/// </summary>
internal sealed class CryptomusHealthCheck(CryptomusClient client) : IHealthCheck
{
    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(5)); // fast check
            await client.Payments.ListServicesAsync(cts.Token).ConfigureAwait(false);
            return HealthCheckResult.Healthy("Cryptomus reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Cryptomus check failed", ex);
        }
    }
}