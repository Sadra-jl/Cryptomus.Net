using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;

namespace PaleLotus.Cryptomus.Net.Clients;

/// <summary>Facade client that groups operations.</summary>
public sealed class CryptomusClient
{
    public PaymentsClient Payments { get; }
    public PayoutsClient Payouts { get; }
    public RecurringClient Recurring { get; }
    public RatesClient Rates { get; }
    public DiscountsClient Discounts { get; }
    public BalanceClient Balance { get; }
    public AmlClient Aml { get; }

    public CryptomusClient(
        PaymentsClient payments,
        PayoutsClient payouts,
        RecurringClient recurring,
        RatesClient rates,
        DiscountsClient discounts,
        BalanceClient balance,
        AmlClient aml,
        IOptions<CryptomusOptions> _)
    {
        Payments = payments;
        Payouts = payouts;
        Recurring = recurring;
        Rates = rates;
        Discounts = discounts;
        Balance = balance;
        Aml = aml;
    }
}