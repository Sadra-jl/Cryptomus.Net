# PaleLotus.Cryptomus.Net

`PaleLotus.Cryptomus.Net` is the core .NET client library for the Cryptomus REST API. It provides typed clients for every major API surface together with dependency injection helpers, request signing and strongly typed models.

## Features

- 🔐 Automatic signature generation for every request.
- 📦 Typed clients for payments, payouts, recurring payments, rates, discounts, balances and AML checks.
- 🔄 Async enumerable helpers that take care of cursor/pagination logic.
- 🧱 First-class integration with `IServiceCollection` via `AddCryptomus`.
- 🧪 Structured responses with `ApiResponse<T>` and pagination metadata.

## Installation

```powershell
Install-Package PaleLotus.Cryptomus.Net
```

## Configuration

Register the SDK inside your DI container and provide the merchant credentials exposed by Cryptomus:

```csharp
var services = new ServiceCollection()
    .AddCryptomus(options =>
    {
        options.MerchantId = configuration["Cryptomus:MerchantId"]!;
        options.PaymentApiKey = configuration["Cryptomus:PaymentApiKey"];
        options.PayoutApiKey = configuration["Cryptomus:PayoutApiKey"];
        options.JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };
    });
```

All outgoing requests automatically pick the correct API key and attach the `merchant` and `sign` headers.

## Usage examples

```csharp
var provider = services.BuildServiceProvider();
var cryptomus = provider.GetRequiredService<CryptomusClient>();

// Create an invoice
var invoiceResponse = await cryptomus.Payments.CreateInvoiceAsync(new CreateInvoiceRequest
{
    Amount = "10",
    Currency = InvoiceCurrency.Usd,
    OrderId = Guid.NewGuid().ToString(),
}).ConfigureAwait(false);

// Iterate the payment history using cursor pagination
await foreach (var payment in cryptomus.Payments.GetPaymentHistoryAsync(new PaymentHistoryRequest
{
    Limit = 50,
}))
{
    Console.WriteLine($"Payment {payment.Uuid} -> {payment.Status}");
}

// Retrieve the latest exchange rates for ETH pairs
var rates = await cryptomus.Rates.ListAsync("ETH").ConfigureAwait(false);
```

## Typed clients

- `PaymentsClient`
- `PayoutsClient`
- `RecurringClient`
- `RatesClient`
- `DiscountsClient`
- `BalanceClient`
- `AmlClient`

Each client is registered as a transient service and can also be requested individually from the service provider.

## Webhook helpers

The package also contains the `WebhookVerifier` utility that validates webhook signatures. For ASP.NET Core specific middleware refer to the [`PaleLotus.Cryptomus.AspNetCore` documentation](../PaleLotus.Cryptomus.AspNetCore/README.md).
