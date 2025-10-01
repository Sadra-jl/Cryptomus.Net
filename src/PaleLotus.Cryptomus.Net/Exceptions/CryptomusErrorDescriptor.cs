namespace PaleLotus.Cryptomus.Net.Exceptions;

/// <summary>Rich metadata describing a Cryptomus error state.</summary>
public sealed record CryptomusErrorDescriptor(
    int State,
    CryptomusErrorCode Code,
    string Title,
    string Description,
    CryptomusErrorCategory Category)
{
    public bool IsSuccess => Category == CryptomusErrorCategory.Success;
}
