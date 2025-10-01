namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiServiceException : CryptomusApiException
{
    internal CryptomusApiServiceException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}