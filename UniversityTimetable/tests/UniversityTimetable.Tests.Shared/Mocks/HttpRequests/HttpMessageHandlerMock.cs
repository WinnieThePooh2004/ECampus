namespace UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

public class HttpMessageHandlerMock : HttpMessageHandler
{
    public Dictionary<HttpRequestMessage, HttpResponseMessage> Responses { get; } = new(new HttpMessageComparer());
    
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Responses[request]);
    }

    private class HttpMessageComparer : IEqualityComparer<HttpRequestMessage>
    {
        public bool Equals(HttpRequestMessage? x, HttpRequestMessage? y)
        {
            if (x is null || y is null)
            {
                return ReferenceEquals(x, y);
            }

            return x.RequestUri == y.RequestUri && x.Method == y.Method;
        }

        public int GetHashCode(HttpRequestMessage obj)
        {
            return obj.RequestUri?.ToString().GetHashCode() ?? obj.GetHashCode();
        }
    }
}