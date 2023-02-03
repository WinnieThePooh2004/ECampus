namespace ECampus.Core.Extensions;

public static class ServiceProvideExtensions
{
    public static TService GetServiceOfType<TService>(this IServiceProvider serviceProvider)
    {
        return (TService?)serviceProvider.GetService(typeof(TService))
               ?? throw new InvalidOperationException(
                   $"There is not any registered services for type {typeof(TService)}");
    }
}