using System.Collections.Generic;
using System.Text.Json;
using PaleLotus.Cryptomus.Net.Exceptions;

namespace PaleLotus.Cryptomus.Net.Internal.Errors;

internal static class CryptomusErrorMessageParser
{
    public static CryptomusErrorDetails Parse(string? rawMessage)
    {
        if (string.IsNullOrWhiteSpace(rawMessage))
            return new CryptomusErrorDetails(null, null, null);

        var trimmed = rawMessage.Trim();

        if (trimmed.Length > 0 && (trimmed[0] == '{' || trimmed[0] == '['))
        {
            try
            {
                using var document = JsonDocument.Parse(trimmed);
                return ParseJson(rawMessage, document.RootElement);
            }
            catch (JsonException)
            {
                return new CryptomusErrorDetails(rawMessage, null, new[] { trimmed });
            }
        }

        return new CryptomusErrorDetails(rawMessage, null, new[] { trimmed });
    }

    private static CryptomusErrorDetails ParseJson(string rawMessage, JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => ParseObject(rawMessage, element),
            JsonValueKind.Array => ParseArray(rawMessage, element),
            JsonValueKind.String => new CryptomusErrorDetails(rawMessage, null, new[] { element.GetString() ?? string.Empty }),
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False
                => new CryptomusErrorDetails(rawMessage, null, new[] { element.ToString() }),
            _ => new CryptomusErrorDetails(rawMessage, null, null),
        };
    }

    private static CryptomusErrorDetails ParseObject(string rawMessage, JsonElement element)
    {
        var fieldErrors = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        var messages = new List<string>();

        foreach (var property in element.EnumerateObject())
        {
            var extracted = ExtractMessages(property.Value);
            if (extracted.Count == 0)
                continue;

            if (IsGeneralMessageProperty(property.Name))
            {
                messages.AddRange(extracted);
            }
            else
            {
                fieldErrors[property.Name] = extracted.ToArray();
            }
        }

        return new CryptomusErrorDetails(
            rawMessage,
            fieldErrors.Count == 0 ? null : fieldErrors,
            messages.Count == 0 ? null : messages);
    }

    private static CryptomusErrorDetails ParseArray(string rawMessage, JsonElement element)
    {
        var messages = ExtractMessages(element);
        return new CryptomusErrorDetails(rawMessage, null, messages.Count == 0 ? null : messages);
    }

    private static List<string> ExtractMessages(JsonElement element)
    {
        var items = new List<string>();

        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                var str = element.GetString();
                if (!string.IsNullOrWhiteSpace(str))
                    items.Add(str!);
                break;

            case JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False:
                items.Add(element.ToString());
                break;

            case JsonValueKind.Array:
                foreach (var item in element.EnumerateArray())
                    items.AddRange(ExtractMessages(item));
                break;

            case JsonValueKind.Object:
                foreach (var property in element.EnumerateObject())
                    items.AddRange(ExtractMessages(property.Value));
                break;
        }

        return items;
    }

    private static bool IsGeneralMessageProperty(string propertyName)
        => string.Equals(propertyName, "message", StringComparison.OrdinalIgnoreCase)
            || string.Equals(propertyName, "error", StringComparison.OrdinalIgnoreCase)
            || string.Equals(propertyName, "errors", StringComparison.OrdinalIgnoreCase)
            || string.Equals(propertyName, "detail", StringComparison.OrdinalIgnoreCase);
}
