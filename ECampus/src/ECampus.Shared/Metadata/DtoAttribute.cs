namespace ECampus.Shared.Metadata;

/// <summary>
/// use this attribute to show DI that BaseService and ParametersService of this Dto use TModel as model
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
// ReSharper disable once UnusedTypeParameter
public class DtoAttribute: Attribute
{
    public DtoAttribute(Type modelType)
    {
        ModelType = modelType;
    }

    public Type ModelType { get; }
}