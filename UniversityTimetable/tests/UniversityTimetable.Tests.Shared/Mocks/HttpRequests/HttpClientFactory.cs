using System.Text.Json;

namespace UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

public class HttpClientFactory : IHttpClientFactory
{
    private static HttpClient? _client;

    public static HttpMessageHandlerMock MessageHandler { get; } = new();

    public static JsonSerializerOptions Options => new() { PropertyNameCaseInsensitive = true };

    public HttpClient CreateClient(string name) => _client ??= new HttpClient(MessageHandler) { BaseAddress = new Uri("https://google.com") };
}