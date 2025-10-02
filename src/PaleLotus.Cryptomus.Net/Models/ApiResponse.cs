using System.Text.Json.Serialization;
using PaleLotus.Cryptomus.Net.Internal.Errors;

namespace PaleLotus.Cryptomus.Net.Models;

public sealed record ApiResponse<T>(
    [property: JsonPropertyName("state")] int State,
    [property: JsonPropertyName("result")] T? Result,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("paginate")] Pagination? Paginate
) : ICryptomusApiResponse
{
    [JsonIgnore]
    public bool IsSuccess => CryptomusErrorRegistry.Resolve(State).IsSuccess;

    public ApiResponse<T> EnsureSuccess()
    {
        CryptomusApiExceptionFactory.ThrowIfError(State, Message);
        return this;
    }
}