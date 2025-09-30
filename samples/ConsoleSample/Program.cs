using Microsoft.Extensions.DependencyInjection;
using PaleLotus.Cryptomus.Net;
using PaleLotus.Cryptomus.Net.Clients;

var services = CreateServices();

var cryptomus = services.GetRequiredService<CryptomusClient>();
var rates = await cryptomus.Rates.ListAsync("ETH").ConfigureAwait(false);
foreach (var exchangeRateItem in rates.Result)
{
    Console.WriteLine($"From: {exchangeRateItem.From}, To: {exchangeRateItem.To}, Course: {exchangeRateItem.Course}");
}

return;

ServiceProvider CreateServices()
{
    var serviceProvider = new ServiceCollection()
        .AddCryptomus(o =>
            {
                o.MerchantId = "<merchant-uuid>";
                o.PaymentApiKey = "<payment-key>";
            }
        );

    return serviceProvider.BuildServiceProvider();
}