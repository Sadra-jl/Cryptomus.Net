namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiUnknownException : CryptomusApiException
{
    internal CryptomusApiUnknownException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}