using Microsoft.Extensions.DependencyInjection;
using PaleLotus.Cryptomus.Net;
using PaleLotus.Cryptomus.Net.Clients;
using PaleLotus.Cryptomus.Net.Models;

var services = CreateServices();

var cryptomus = services.GetRequiredService<CryptomusClient>();
// var rates = await cryptomus.Rates.ListAsync("ETH").ConfigureAwait(false);
// foreach (var exchangeRateItem in rates.Result)
// {
//     Console.WriteLine($"From: {exchangeRateItem.From}, To: {exchangeRateItem.To}, Course: {exchangeRateItem.Course}");
// }
var request = new CreateInvoiceRequest("1", "USD", "1666");
var invoice = await cryptomus.Payments.CreateInvoiceAsync(request).ConfigureAwait(false);

Console.WriteLine(invoice.Result);

return;

ServiceProvider CreateServices()
{
    var serviceProvider = new ServiceCollection()
        .AddCryptomus(o =>
            {
                o.MerchantId = "e1bd5e52-0a0a-4ec6-849c-ddf12cab9f8c";
                o.PaymentApiKey = "TfUWKfkVa859NXRZ2C8GMJ6irwSzvRXCWrVGBxpMpwsl90mUMbkx6kU7gcMg9KihyDpRDz6bU4pGYyfgkqjNxkIT8R3gzCqvgBnMfpIrYjnWhVEEtmr6WyB1OO7nThuE";
            }
        );

    return serviceProvider.BuildServiceProvider();
}