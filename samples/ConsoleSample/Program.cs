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
//var invoice = await cryptomus.Payments.CreateInvoiceAsync(request).ConfigureAwait(false);

var i = 0;
Console.WriteLine(i++);
Console.WriteLine(i);
Console.WriteLine(++i);
Console.WriteLine(i);

//Console.WriteLine(invoice.Result);

return;

ServiceProvider CreateServices()
{
    var serviceProvider = new ServiceCollection()
        .AddCryptomus(o =>
            {
                o.MerchantId = "<merchant-uuid>";
                o.PaymentApiKey = "<payment-key>";            }
        );

    return serviceProvider.BuildServiceProvider();
}