namespace PaleLotus.Cryptomus.Net.Exceptions;

/// <summary>Structured information parsed from the response message payload.</summary>
public sealed record CryptomusErrorDetails(
    string? RawMessage,
    IReadOnlyDictionary<string, string[]>? FieldErrors,
    IReadOnlyList<string>? Messages)
{
    public bool HasFieldErrors => FieldErrors is { Count: > 0 };

    public bool HasMessages => Messages is { Count: > 0 };
}
