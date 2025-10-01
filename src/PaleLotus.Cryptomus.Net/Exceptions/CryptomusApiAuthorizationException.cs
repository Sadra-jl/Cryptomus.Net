namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiAuthorizationException : CryptomusApiException
{
    internal CryptomusApiAuthorizationException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}