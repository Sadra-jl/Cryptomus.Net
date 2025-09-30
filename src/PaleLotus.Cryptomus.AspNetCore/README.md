# PaleLotus.Cryptomus.AspNetCore

`PaleLotus.Cryptomus.AspNetCore` adds ASP.NET Core specific building blocks on top of the base Cryptomus SDK. It helps you wire the SDK into `WebApplicationBuilder`, expose ready-made health checks and protect your webhook endpoints with signature verification.

## Installation

```powershell
Install-Package PaleLotus.Cryptomus.AspNetCore
```

> This package depends on `PaleLotus.Cryptomus.Net` and references it automatically.

## Service registration

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCryptomusAspNet(options =>
{
    options.MerchantId = builder.Configuration["Cryptomus:MerchantId"]!;
    options.PaymentApiKey = builder.Configuration["Cryptomus:PaymentApiKey"];
    options.PayoutApiKey = builder.Configuration["Cryptomus:PayoutApiKey"];
});
```

`AddCryptomusAspNet` registers:

- The core SDK (`AddCryptomus`).
- `ProblemDetails` for consistent error responses.
- A `CryptomusHealthCheck` entry (`/health` ready/live tags).

## Webhook helpers

Minimal APIs can use the `MapCryptomusWebhook` extension to register endpoints that automatically validate the `sign` header.

```csharp
app.MapCryptomusWebhook<PaymentInfoResponse>("/webhooks/payments", async (http, payload) =>
{
    // Process the incoming payment notification
    return Results.Ok();
});
```

Behind the scenes the `WebhookVerificationFilter` buffers the request body, verifies the signature using the configured API key and short-circuits unauthorised calls with a `401` response.

Use the `WebhookType` parameter when mapping payout webhooks:

```csharp
app.MapCryptomusWebhook<PayoutInfoResponse>(
    "/webhooks/payouts",
    (http, payload) => Results.Ok(),
    WebhookType.Payout);
```

## Health checks

A dedicated `CryptomusHealthCheck` is registered under the `"cryptomus"` name. It performs a lightweight `ListServicesAsync` call to verify connectivity to the platform and is tagged with `ready` and `live` so that you can include it in liveness or readiness probes.

```csharp
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
});
```

## Sample

The [`samples/ConsoleSample`](../../samples/ConsoleSample) project demonstrates how to use the underlying SDK without ASP.NET Core. Combine the console sample with the ASP.NET extensions above to quickly build production-ready endpoints.
