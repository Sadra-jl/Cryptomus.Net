using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PaleLotus.Cryptomus.Net.Abstractions;
using PaleLotus.Cryptomus.Net.Exceptions;
using PaleLotus.Cryptomus.Net.Internal.Errors;

namespace PaleLotus.Cryptomus.Net.Internal;

/// <summary>
/// Small helper that hides the HTTP plumbing required to call the Cryptomus REST API.
/// </summary>
public sealed class CryptomusHttp(IHttpClientFactory factory, IOptions<CryptomusOptions> opts, ICryptomusSigner signer)
{
    private Uri BuildUri(string path)
    {
        var baseUri = opts.Value.BaseUrl.EndsWith('/') ? opts.Value.BaseUrl : $"{opts.Value.BaseUrl}/";
        return new Uri(new Uri(baseUri), path.StartsWith('/') ? path[1..] : path);
    }

    /// <summary>
    /// Sends a GET request using the named <see cref="HttpClient"/> and deserializes the JSON payload.
    /// </summary>
    /// <typeparam name="TResp">Type the JSON response should be deserialized into.</typeparam>
    /// <param name="httpClientName">The logical name of the configured <see cref="HttpClient"/>.</param>
    /// <param name="path">Relative path appended to the configured base URL.</param>
    /// <param name="apiKey">API key used for the <c>sign</c> header.</param>
    /// <param name="ct">Cancellation token that aborts the HTTP call.</param>
    /// <returns>The deserialized response payload.</returns>
    public Task<TResp> GetAsync<TResp>(string httpClientName, string path, string apiKey, CancellationToken ct)
        => SendAsync<object?, TResp>(httpClientName, HttpMethod.Get, path, null, apiKey, ct);

    /// <summary>
    /// Sends a POST request using the named <see cref="HttpClient"/> and deserializes the JSON payload.
    /// </summary>
    /// <typeparam name="TReq">Type of the request body.</typeparam>
    /// <typeparam name="TResp">Type the JSON response should be deserialized into.</typeparam>
    /// <param name="httpClientName">The logical name of the configured <see cref="HttpClient"/>.</param>
    /// <param name="path">Relative path appended to the configured base URL.</param>
    /// <param name="body">Request payload that will be serialized to JSON.</param>
    /// <param name="apiKey">API key used for the <c>sign</c> header.</param>
    /// <param name="ct">Cancellation token that aborts the HTTP call.</param>
    /// <returns>The deserialized response payload.</returns>
    public Task<TResp> PostAsync<TReq, TResp>(string httpClientName, string path, TReq body, string apiKey, CancellationToken ct)
        => SendAsync<TReq, TResp>(httpClientName, HttpMethod.Post, path, body, apiKey, ct);

    /// <summary>
    /// Core request pipeline that signs the body, applies the Cryptomus headers and returns the JSON response.
    /// </summary>
    /// <typeparam name="TReq">Type of the request payload.</typeparam>
    /// <typeparam name="TResp">Type of the response payload.</typeparam>
    /// <param name="httpClientName">The logical name of the configured <see cref="HttpClient"/>.</param>
    /// <param name="method">HTTP method to use.</param>
    /// <param name="path">Relative path appended to the configured base URL.</param>
    /// <param name="body">Optional request payload.</param>
    /// <param name="apiKey">API key used for the <c>sign</c> header.</param>
    /// <param name="ct">Cancellation token that aborts the HTTP call.</param>
    /// <returns>The deserialized response.</returns>
    private async Task<TResp> SendAsync<TReq, TResp>(string httpClientName, HttpMethod method, string path, TReq? body, string apiKey, CancellationToken ct)
    {
        var jsonOpts = opts.Value.JsonSerializerOptions;
        var uri = BuildUri(path);
        using var request = new HttpRequestMessage(method, uri);

        // Compute signature on JSON (or empty for GET/no body)
        var json = body is null ? null : JsonSerializer.Serialize(body, jsonOpts);
        var sign = signer.ComputeSign(json ?? string.Empty, apiKey, jsonOpts);

        request.Headers.TryAddWithoutValidation("merchant", opts.Value.MerchantId);
        request.Headers.TryAddWithoutValidation("sign", sign);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (json is not null && method != HttpMethod.Get)
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var client = factory.CreateClient(httpClientName);
        using var resp = await client.SendAsync(request, ct).ConfigureAwait(false);
        var payload = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        if (!resp.IsSuccessStatusCode)
            throw new CryptomusHttpException(resp.StatusCode, payload);

        var result = JsonSerializer.Deserialize<TResp>(payload, jsonOpts ?? new());
        if (result is ICryptomusApiResponse apiResponse)
            CryptomusApiExceptionFactory.ThrowIfError(apiResponse.State, apiResponse.Message);

        return result!;
    }
}