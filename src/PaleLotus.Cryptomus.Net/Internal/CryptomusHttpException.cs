using System.Net;

namespace PaleLotus.Cryptomus.Net.Internal;

/// <summary>Represents an unsuccessful HTTP response from Cryptomus.</summary>
public sealed class CryptomusHttpException(HttpStatusCode statusCode, string responseBody)
    : Exception($"Cryptomus API returned {(int)statusCode} {statusCode}: {responseBody}")
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public string ResponseBody { get; } = responseBody;
}