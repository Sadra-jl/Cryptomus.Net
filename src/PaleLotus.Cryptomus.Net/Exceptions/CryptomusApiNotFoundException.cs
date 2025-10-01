namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiNotFoundException : CryptomusApiException
{
    internal CryptomusApiNotFoundException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}