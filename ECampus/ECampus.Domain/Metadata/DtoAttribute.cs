﻿using ECampus.Domain.Data;

namespace ECampus.Domain.Metadata;

/// <summary>
/// use this attribute to show DI that BaseService and ParametersService of this Dto use TModel as model
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
// ReSharper disable once UnusedTypeParameter
public class DtoAttribute: Attribute
{
    public Type ModelType { get; }
    public bool InjectBaseService { get; init; } = true;

    protected DtoAttribute(Type modelType)
    {
        ModelType = modelType;
    }
}

public class DtoAttribute<TModel> : DtoAttribute
    where TModel : class, IEntity
{
    public DtoAttribute() : base(typeof(TModel))
    {
    }
}