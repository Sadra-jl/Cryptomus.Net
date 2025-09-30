# Cryptomus.Net SDK

Cryptomus.Net is an **Unofficial**  .NET SDK that streamlines access to the [Cryptomus](https://cryptomus.com/) payment platform. The repository contains a core HTTP client as well as ASP.NET Core helpers that make it easy to integrate payment, payout, recurring, discount and AML workflows into your applications.

## Repository layout

| Project | Description |
| --- | --- |
| [`PaleLotus.Cryptomus.Net`](src/PaleLotus.Cryptomus.Net/README.md) | Core SDK with typed clients for every Cryptomus API surface and utilities for signature validation. |
| [`PaleLotus.Cryptomus.AspNetCore`](src/PaleLotus.Cryptomus.AspNetCore/README.md) | ASP.NET Core extensions that plug the SDK into dependency injection, health checks and webhook verification. |
| [`samples/ConsoleSample`](samples/ConsoleSample) | Minimal console application showing how to request exchange rates. |

## Getting started

Install the packages from NuGet (package IDs match the project names):

```powershell
Install-Package PaleLotus.Cryptomus.Net
Install-Package PaleLotus.Cryptomus.AspNetCore
```

Configure the services in your application. The following example shows a minimal API configured with webhook verification and health checks:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCryptomusAspNet(options =>
{
    options.MerchantId = builder.Configuration["Cryptomus:MerchantId"]!;
    options.PaymentApiKey = builder.Configuration["Cryptomus:PaymentApiKey"];
    options.PayoutApiKey = builder.Configuration["Cryptomus:PayoutApiKey"];
});

var app = builder.Build();

app.MapCryptomusWebhook<PaymentInfoResponse>("/webhooks/payments", async (http, payload) =>
{
    // Process the webhook payload here
    return Results.Ok();
});

app.MapHealthChecks("/health");

app.Run();
```

For non-web scenarios you can register the base SDK directly:

```csharp
var services = new ServiceCollection()
    .AddCryptomus(options =>
    {
        options.MerchantId = "<merchant-uuid>";
        options.PaymentApiKey = "<payment-api-key>";
        options.PayoutApiKey = "<payout-api-key>";
    })
    .BuildServiceProvider();

var cryptomus = services.GetRequiredService<CryptomusClient>();
var invoice = await cryptomus.Payments.CreateInvoiceAsync(request, ct);
```

## Additional resources

* Read the [core SDK README](src/PaleLotus.Cryptomus.Net/README.md) for a detailed feature overview.
* Review the [ASP.NET Core README](src/PaleLotus.Cryptomus.AspNetCore/README.md) for middleware and webhook guidance.
* Explore the [`samples`](samples) folder for runnable examples.
