namespace ECampus.FrontEnd.Requests.Options;

public interface IRequestOptions
{
    string GetControllerName(Type objectType);
}