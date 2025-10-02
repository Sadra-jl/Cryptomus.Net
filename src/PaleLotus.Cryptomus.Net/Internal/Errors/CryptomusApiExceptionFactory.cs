using PaleLotus.Cryptomus.Net.Exceptions;

namespace PaleLotus.Cryptomus.Net.Internal.Errors;

internal static class CryptomusApiExceptionFactory
{
    public static void ThrowIfError(int state, string? rawMessage)
    {
        var descriptor = CryptomusErrorRegistry.Resolve(state);
        if (descriptor.IsSuccess)
            return;

        var details = CryptomusErrorMessageParser.Parse(rawMessage);
        throw Create(descriptor, rawMessage, details);
    }

    private static CryptomusApiException Create(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        => descriptor.Category switch
        {
            CryptomusErrorCategory.Success => new CryptomusApiUnknownException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Authentication => new CryptomusApiAuthenticationException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Authorization => new CryptomusApiAuthorizationException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Validation => new CryptomusApiValidationException(descriptor, rawMessage, details),
            CryptomusErrorCategory.NotFound => new CryptomusApiNotFoundException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Conflict => new CryptomusApiConflictException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Limit => new CryptomusApiLimitException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Service => new CryptomusApiServiceException(descriptor, rawMessage, details),
            CryptomusErrorCategory.Aml => new CryptomusApiAmlException(descriptor, rawMessage, details),
            _ => new CryptomusApiUnknownException(descriptor, rawMessage, details),
        };
}
