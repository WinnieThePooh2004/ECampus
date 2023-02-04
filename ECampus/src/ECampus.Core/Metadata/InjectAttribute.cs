﻿using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Core.Metadata;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InjectAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }
    
    public Type ServiceType { get; }
    
    public Func<IServiceProvider, object>? Factory { get; init; }

    public InjectAttribute(Type serviceType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ServiceType = serviceType;
        ServiceLifetime = serviceLifetime;
    }
}