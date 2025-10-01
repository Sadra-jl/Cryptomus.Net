namespace PaleLotus.Cryptomus.Net.Exceptions;

/// <summary>Base type for all exceptions thrown by the Cryptomus client library.</summary>
public abstract class CryptomusException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
}
