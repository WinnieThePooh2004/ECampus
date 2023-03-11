using System.Text.Json;

namespace ECampus.Tests.Shared.Mocks.HttpRequests;

public class HttpClientFactory : IHttpClientFactory
{
    private HttpClient? _client;

    public static JsonSerializerOptions Options => new() { PropertyNameCaseInsensitive = true };

    public HttpMessageHandlerMock MessageHandler { get; } = new();

    public HttpClient CreateClient(string name) => _client ??= new HttpClient(MessageHandler) { BaseAddress = new Uri("https://google.com") };
}