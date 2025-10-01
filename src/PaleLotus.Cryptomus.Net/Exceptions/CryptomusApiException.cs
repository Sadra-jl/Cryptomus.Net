using System.Text;

namespace PaleLotus.Cryptomus.Net.Exceptions;

/// <summary>Base type for all documented Cryptomus API exceptions.</summary>
public abstract class CryptomusApiException(
    CryptomusErrorDescriptor descriptor,
    string? rawMessage,
    CryptomusErrorDetails details)
    : CryptomusException(BuildMessage(descriptor, rawMessage, details))
{
    public CryptomusErrorDescriptor Descriptor { get; } = descriptor;

    public int State => Descriptor.State;

    public CryptomusErrorCode Code => Descriptor.Code;

    public CryptomusErrorCategory Category => Descriptor.Category;

    public string? RawMessage { get; } = rawMessage;

    public CryptomusErrorDetails Details { get; } = details;

    private static string BuildMessage(CryptomusErrorDescriptor descriptor, string? rawMessage, CryptomusErrorDetails details)
    {
        var builder = new StringBuilder();
        builder.Append("Cryptomus API returned state ");
        builder.Append($"{descriptor.State} ({descriptor.Code}): {descriptor.Title}");

        if (!string.IsNullOrWhiteSpace(descriptor.Description))
        {
            builder.Append($". {descriptor.Description}");       
        }

        if (details.HasMessages)
        {
            builder.Append(" Details: ");
            builder.Append(string.Join("; ", details.Messages!));
        }

        if (details.HasFieldErrors)
        {
            builder.Append(" Field errors: ");
            builder.Append(string.Join("; ", details.FieldErrors!
                .Select(pair => $"{pair.Key}: {string.Join(", ", pair.Value)}")));
        }
        else if (!details.HasMessages && !string.IsNullOrWhiteSpace(rawMessage))
        {
            builder.Append(" Raw message: ");
            builder.Append(rawMessage);
        }

        return builder.ToString();
    }
}