namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiAmlException : CryptomusApiException
{
    internal CryptomusApiAmlException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}