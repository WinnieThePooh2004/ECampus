using Microsoft.Extensions.DependencyInjection;

namespace UniversityTimetable.Shared.Metadata;

[AttributeUsage(AttributeTargets.Class)]
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