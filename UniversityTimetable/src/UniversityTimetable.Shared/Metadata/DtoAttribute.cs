using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Metadata;

/// <summary>
/// use this attribute to show DI that BaseService and ParametersService of this Dto use TModel as model
/// </summary>
/// <typeparam name="TModel"></typeparam>
[AttributeUsage(AttributeTargets.Class)]
// ReSharper disable once UnusedTypeParameter
public class DtoAttribute<TModel>: Attribute
    where TModel : class, IModel
{
    
}