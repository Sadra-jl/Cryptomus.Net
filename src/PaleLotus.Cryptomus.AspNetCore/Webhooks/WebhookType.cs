namespace PaleLotus.Cryptomus.AspNetCore.Webhooks;

/// <summary>
/// Identifies the Cryptomus webhook category so the correct API key can be used for verification.
/// </summary>
public enum WebhookType
{
    /// <summary>Indicates a payment webhook using the payment API key.</summary>
    Payment,

    /// <summary>Indicates a payout webhook using the payout API key.</summary>
    Payout,
}