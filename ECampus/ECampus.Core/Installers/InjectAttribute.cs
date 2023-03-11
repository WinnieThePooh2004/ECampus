using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Core.Installers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InjectAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }
    
    public Type ServiceType { get; }
    
    public InjectAttribute(Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ServiceType = serviceType;
        ServiceLifetime = serviceLifetime;
    }
}