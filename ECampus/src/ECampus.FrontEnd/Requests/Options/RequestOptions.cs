namespace ECampus.FrontEnd.Requests.Options;

public class RequestOptions : IRequestOptions
{
    public const string ClientName = "UTApi";
    private readonly Dictionary<string, string> _controllerNames;

    public RequestOptions(IConfiguration configuration)
    {
        _controllerNames = configuration.GetSection("Requests").Get<Dictionary<string, string>>()!;
    }

    public string GetControllerName(Type objectType) => _controllerNames[objectType.Name];
}