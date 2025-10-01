namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiAuthenticationException : CryptomusApiException
{
    internal CryptomusApiAuthenticationException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}