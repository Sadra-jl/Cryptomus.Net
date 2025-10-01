namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiLimitException : CryptomusApiException
{
    internal CryptomusApiLimitException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}