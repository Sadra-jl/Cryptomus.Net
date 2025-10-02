using System.Collections.Concurrent;
using PaleLotus.Cryptomus.Net.Exceptions;

namespace PaleLotus.Cryptomus.Net.Internal.Errors;

internal static class CryptomusErrorRegistry
{
    private static readonly Dictionary<int, CryptomusErrorDescriptor> KnownStates = new Dictionary<int, CryptomusErrorDescriptor>
    {
        [0] = new(0, CryptomusErrorCode.UnknownError, "Error", "The Cryptomus API returned an unspecified error.", CryptomusErrorCategory.Unknown),
        [1] = new(1, CryptomusErrorCode.Success, "Success", "The request has completed successfully.", CryptomusErrorCategory.Success),
        [2] = new(2, CryptomusErrorCode.InvalidRequest, "Validation error", "The payload failed validation on the Cryptomus side.", CryptomusErrorCategory.Validation),
        [3] = new(3, CryptomusErrorCode.InvalidSignature, "Invalid signature", "The request signature does not match the body.", CryptomusErrorCategory.Authentication),
        [4] = new(4, CryptomusErrorCode.MerchantNotFound, "Merchant not found", "The merchant identifier could not be resolved.", CryptomusErrorCategory.Authentication),
        [5] = new(5, CryptomusErrorCode.MerchantBlocked, "Merchant blocked", "The merchant is blocked and cannot perform API operations.", CryptomusErrorCategory.Authorization),
        [6] = new(6, CryptomusErrorCode.InvalidApiKey, "Invalid API key", "The provided API key is invalid or revoked.", CryptomusErrorCategory.Authentication),
        [7] = new(7, CryptomusErrorCode.AccessDenied, "Access denied", "The merchant does not have permission to access this resource.", CryptomusErrorCategory.Authorization),
        [8] = new(8, CryptomusErrorCode.RateLimitExceeded, "Too many requests", "The request rate limit has been exceeded.", CryptomusErrorCategory.Limit),
        [9] = new(9, CryptomusErrorCode.OperationNotAvailable, "Operation not available", "The requested operation is disabled for this merchant.", CryptomusErrorCategory.Authorization),
        [10] = new(10, CryptomusErrorCode.ServiceUnavailable, "Service temporarily unavailable", "Cryptomus is temporarily unable to process the request.", CryptomusErrorCategory.Service),
        [11] = new(11, CryptomusErrorCode.InsufficientBalance, "Insufficient balance", "The merchant balance is not sufficient to complete the operation.", CryptomusErrorCategory.Limit),
        [12] = new(12, CryptomusErrorCode.InvoiceNotFound, "Invoice not found", "The specified invoice identifier does not exist.", CryptomusErrorCategory.NotFound),
        [13] = new(13, CryptomusErrorCode.InvoiceAlreadyPaid, "Invoice already paid", "The invoice has already been paid.", CryptomusErrorCategory.Conflict),
        [14] = new(14, CryptomusErrorCode.InvoiceExpired, "Invoice expired", "The invoice is expired and cannot be processed.", CryptomusErrorCategory.Conflict),
        [15] = new(15, CryptomusErrorCode.InvoiceCancelled, "Invoice cancelled", "The invoice has been cancelled.", CryptomusErrorCategory.Conflict),
        [16] = new(16, CryptomusErrorCode.InvoiceStatusConflict, "Invoice state conflict", "The invoice is in a state that does not allow this action.", CryptomusErrorCategory.Conflict),
        [17] = new(17, CryptomusErrorCode.InvoiceAmountMismatch, "Amount mismatch", "The amount does not match the invoice amount.", CryptomusErrorCategory.Validation),
        [18] = new(18, CryptomusErrorCode.InvoiceCurrencyMismatch, "Currency mismatch", "The currency does not match the invoice currency.", CryptomusErrorCategory.Validation),
        [19] = new(19, CryptomusErrorCode.PayoutNotFound, "Payout not found", "The payout identifier does not exist.", CryptomusErrorCategory.NotFound),
        [20] = new(20, CryptomusErrorCode.PayoutAlreadyProcessed, "Payout already processed", "The payout has already been processed.", CryptomusErrorCategory.Conflict),
        [21] = new(21, CryptomusErrorCode.PayoutBlocked, "Payout blocked", "The payout is blocked for the merchant or wallet.", CryptomusErrorCategory.Authorization),
        [22] = new(22, CryptomusErrorCode.WalletNotFound, "Wallet not found", "The wallet address was not found.", CryptomusErrorCategory.NotFound),
        [23] = new(23, CryptomusErrorCode.WalletBlocked, "Wallet blocked", "The wallet is blocked and cannot be used.", CryptomusErrorCategory.Authorization),
        [24] = new(24, CryptomusErrorCode.UnsupportedCurrency, "Unsupported currency", "The currency is not supported by Cryptomus.", CryptomusErrorCategory.Validation),
        [25] = new(25, CryptomusErrorCode.UnsupportedNetwork, "Unsupported network", "The blockchain network is not supported.", CryptomusErrorCategory.Validation),
        [26] = new(26, CryptomusErrorCode.AmountBelowMinimum, "Amount below minimum", "The amount is lower than the allowed minimum.", CryptomusErrorCategory.Validation),
        [27] = new(27, CryptomusErrorCode.AmountAboveMaximum, "Amount above maximum", "The amount exceeds the allowed maximum.", CryptomusErrorCategory.Validation),
        [28] = new(28, CryptomusErrorCode.DuplicateRequest, "Duplicate request", "A request with the same identifier already exists.", CryptomusErrorCategory.Conflict),
        [29] = new(29, CryptomusErrorCode.StaticWalletAlreadyExists, "Static wallet already exists", "A static wallet for the specified currency already exists.", CryptomusErrorCategory.Conflict),
        [30] = new(30, CryptomusErrorCode.RecurringNotFound, "Recurring payment not found", "The recurring payment identifier could not be found.", CryptomusErrorCategory.NotFound),
        [31] = new(31, CryptomusErrorCode.RecurringAlreadyCancelled, "Recurring payment already cancelled", "The recurring payment has already been cancelled.", CryptomusErrorCategory.Conflict),
        [32] = new(32, CryptomusErrorCode.RecurringAlreadyExists, "Recurring payment already exists", "A recurring payment with the supplied identifier already exists.", CryptomusErrorCategory.Conflict),
        [33] = new(33, CryptomusErrorCode.DiscountNotFound, "Discount not found", "The discount identifier does not exist.", CryptomusErrorCategory.NotFound),
        [34] = new(34, CryptomusErrorCode.DiscountAlreadyExists, "Discount already exists", "A discount with the provided identifier already exists.", CryptomusErrorCategory.Conflict),
        [35] = new(35, CryptomusErrorCode.AmlRejected, "Rejected by AML", "The operation was rejected by the AML service.", CryptomusErrorCategory.Aml),
        [36] = new(36, CryptomusErrorCode.AmlVerificationRequired, "AML verification required", "Additional AML verification is required before the operation can continue.", CryptomusErrorCategory.Aml),
        [37] = new(37, CryptomusErrorCode.WebhookNotFound, "Webhook not found", "The webhook identifier could not be found.", CryptomusErrorCategory.NotFound),
        [38] = new(38, CryptomusErrorCode.WebhookSignatureInvalid, "Invalid webhook signature", "The webhook signature is invalid.", CryptomusErrorCategory.Validation),
        [39] = new(39, CryptomusErrorCode.WebhookAlreadySent, "Webhook already sent", "The webhook has already been dispatched.", CryptomusErrorCategory.Conflict),
        [40] = new(40, CryptomusErrorCode.TestModeOnly, "Test mode only", "The operation is only available in test mode.", CryptomusErrorCategory.Validation),
        [41] = new(41, CryptomusErrorCode.Maintenance, "Maintenance", "The API is under maintenance.", CryptomusErrorCategory.Service),
        [500] = new(500, CryptomusErrorCode.InternalError, "Internal error", "Cryptomus encountered an internal error.", CryptomusErrorCategory.Service),
    };

    private static readonly ConcurrentDictionary<int, CryptomusErrorDescriptor> UnknownStates = new();

    public static CryptomusErrorDescriptor Resolve(int state)
    {
        if (KnownStates.TryGetValue(state, out var descriptor))
            return descriptor;

        return UnknownStates.GetOrAdd(state, s => new CryptomusErrorDescriptor(
            s,
            CryptomusErrorCode.UnknownError,
            $"Unknown state {s}",
            "Cryptomus returned an undocumented error state.",
            CryptomusErrorCategory.Unknown));
    }
}
