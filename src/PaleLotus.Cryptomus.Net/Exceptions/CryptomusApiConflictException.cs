namespace PaleLotus.Cryptomus.Net.Exceptions;

public sealed class CryptomusApiConflictException : CryptomusApiException
{
    internal CryptomusApiConflictException(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
        : base(descriptor, rawMessage, details)
    {
    }
}