namespace PaleLotus.Cryptomus.Net.Exceptions;

/// <summary>Groups Cryptomus error codes into high level categories.</summary>
public enum CryptomusErrorCategory
{
    Success,
    Authentication,
    Authorization,
    Validation,
    NotFound,
    Conflict,
    Limit,
    Service,
    Aml,
    Unknown,
}
