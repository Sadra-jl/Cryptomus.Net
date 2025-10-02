namespace PaleLotus.Cryptomus.Net.Internal.Errors;

internal interface ICryptomusApiResponse
{
    int State { get; }

    string? Message { get; }
}
