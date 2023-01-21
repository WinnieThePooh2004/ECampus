namespace ECampus.FrontEnd.Requests.Interfaces.Validation;

/// <summary>
/// this interface is needed is avoid ambiguous during DI
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICreateValidationRequests<in T> : IValidationRequests<T>
{
    
}