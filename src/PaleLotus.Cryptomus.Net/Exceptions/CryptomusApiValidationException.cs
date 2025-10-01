namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiValidationException : CryptomusApiException
{
    internal CryptomusApiValidationException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}