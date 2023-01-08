using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Domain.Validation.CreateValidators;
using UniversityTimetable.Domain.Validation.UniversalValidators;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Infrastructure.DataDeleteServices;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Shared.Attributes;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Extensions;

// ReSharper disable once InconsistentNaming
internal static class IServiceCollectionExtensions
{
    public static void AddDefaultDataServices(this IServiceCollection services, Assembly assembly)
    {
        var modelsFromAssembly = GetModels(assembly);
        foreach (var modelType in modelsFromAssembly)
        {
            services.AddScoped(typeof(IDataCreateService<>).MakeGenericType(modelType),
                typeof(DataCreateService<>).MakeGenericType(modelType));
            services.AddScoped(typeof(IDataDeleteService<>).MakeGenericType(modelType),
                typeof(DataDeleteService<>).MakeGenericType(modelType));
            services.AddScoped(typeof(IDataUpdateService<>).MakeGenericType(modelType),
                typeof(DataUpdateService<>).MakeGenericType(modelType));
        }
    }

    public static void DecorateDataServicesWithRelationshipsServices(this IServiceCollection services,
        Assembly assembly)
    {
        var relationModels = GetRelationModels(assembly);
        foreach (var relationModel in relationModels)
        {
            var relationInterfaces = relationModel.GetInterfaces()
                .Where(i => i.IsGenericOfType(typeof(IModelWithManyToManyRelations<,>)));
            foreach (var genericArgs in relationInterfaces.Select(model => model.GenericTypeArguments))
            {
                services.Decorate(typeof(IDataUpdateService<>).MakeGenericType(relationModel),
                    typeof(DataUpdateServiceWithRelationships<,,>).MakeGenericType(relationModel, genericArgs[0],
                        genericArgs[1]));
                services.Decorate(typeof(IDataCreateService<>).MakeGenericType(relationModel),
                    typeof(DataCreateServiceWithRelationships<,,>).MakeGenericType(relationModel, genericArgs[0],
                        genericArgs[1]));
            }
        }
    }

    public static void AddDataValidator(this IServiceCollection services, Assembly validatorAssembly)
    {
        var validationDataAccess = validatorAssembly.GetTypes().Where(type =>
            type.IsClass && type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IValidationDataAccess<>)))).ToList();
        foreach (var type in validationDataAccess)
        {
            var modelType = type.GetInterfaces().Single(i => i.IsGenericOfType(typeof(IValidationDataAccess<>)))
                .GenericTypeArguments;

            services.AddScoped(typeof(IValidationDataAccess<>).MakeGenericType(modelType[0]), type);
        }

        var dataValidators = validatorAssembly.GetTypes().Where(type =>
            type.IsClass && type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IDataValidator<>)))).ToList();
        foreach (var dataValidator in dataValidators)
        {
            var modelType = dataValidator.GetInterfaces()
                .Single(i => i.IsGenericOfType(typeof(IDataValidator<>))).GenericTypeArguments;

            services.AddScoped(typeof(IDataValidator<>).MakeGenericType(modelType), dataValidator);
        }
    }

    public static void AddMultipleDataSelectors(this IServiceCollection services, Assembly assembly)
    {
        var selectors = GetMultipleItemSelectors(assembly);
        foreach (var selector in selectors)
        {
            var selectorParameters = selector.GetInterfaces().Single(i =>
                                         i.IsGenericOfType(typeof(IMultipleItemSelector<,>))).GetGenericArguments()
                                     ?? throw new UnreachableException();
            services.AddScoped(typeof(IMultipleItemSelector<,>).MakeGenericType(selectorParameters), selector);
        }
    }

    public static void AddDefaultFacades(this IServiceCollection services, Assembly modelsAssembly)
    {
        var modelsFromAssembly = GetModels(modelsAssembly);
        var dataTransferObjects = GetDataTransferObjects(modelsAssembly);
        var queryParameters = GetQueryParameters(modelsAssembly);
        foreach (var modelType in modelsFromAssembly)
        {
            services.AddScoped(typeof(IBaseDataAccessFacade<>).MakeGenericType(modelType),
                typeof(BaseDataAccessFacade<>).MakeGenericType(modelType));
            var modelDto = dataTransferObjects.Single(dto => Attribute.GetCustomAttributes(dto).Any(attribute =>
                attribute.GetType().IsGenericType &&
                attribute.GetType() == typeof(DtoAttribute<>).MakeGenericType(modelType)));
            services.AddScoped(typeof(IBaseService<>).MakeGenericType(modelDto),
                typeof(BaseService<,>).MakeGenericType(modelDto, modelType));
            var modelParameters = queryParameters.SingleOrDefault(parameters => parameters.GetInterfaces().Any(i =>
                i.IsGenericType && i == typeof(IQueryParameters<>).MakeGenericType(modelType)));
            if (modelParameters is null)
            {
                continue;
            }

            services.AddScoped(typeof(IParametersDataAccessFacade<,>).MakeGenericType(modelType, modelParameters),
                typeof(ParametersDataAccessFacade<,>).MakeGenericType(modelType, modelParameters));
            services.AddScoped(typeof(IParametersService<,>).MakeGenericType(modelDto, modelParameters),
                typeof(ParametersService<,,>).MakeGenericType(modelDto, modelParameters, modelType));
        }
    }

    public static void AddSingleItemSelectors(this IServiceCollection services, Assembly modelsAssembly,
        Assembly selectorAssembly)
    {
        var selectors = selectorAssembly.GetTypes().Where(type =>
            type.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISingleItemSelector<>))).ToList();

        var models = GetModels(modelsAssembly);
        foreach (var modelType in models)
        {
            var modelSelector = selectors.SingleOrDefault(selector => selector.GetInterfaces()
                .Any(i => i == typeof(ISingleItemSelector<>).MakeGenericType(modelType)));
            if (modelSelector is null)
            {
                services.AddScoped(typeof(ISingleItemSelector<>).MakeGenericType(modelType),
                    typeof(SingleItemSelector<>).MakeGenericType(modelType));
                continue;
            }

            services.AddScoped(typeof(ISingleItemSelector<>).MakeGenericType(modelType), modelSelector);
        }
    }

    public static void AddDefaultDomainServices(this IServiceCollection services, Assembly dataTransferObjectsAssembly,
        Assembly servicesAssembly)
    {
        var types = TypesRequiresValidation(dataTransferObjectsAssembly);

        foreach (var modelType in types)
        {
            var typeValidation = (ValidationAttribute?)modelType.GetCustomAttributes()
                .FirstOrDefault(attribute => attribute.GetType() == typeof(ValidationAttribute));
            if (typeValidation is null || typeValidation.ValidationType == ValidationTypes.None)
            {
                continue;
            }

            AddBaseValidators(services, typeValidation, modelType);

            LoadValidatorTypes(servicesAssembly, modelType, out var createValidator,
                out var universalValidator, out var updateValidator);

            if (typeValidation.RequireService)
            {
                AddServicesWithValidation(services, typeValidation, modelType);
            }

            if (universalValidator is not null)
            {
                services.Decorate(typeof(IUniversalValidator<>).MakeGenericType(modelType), universalValidator);
                continue;
            }

            if (createValidator is not null)
            {
                services.Decorate(typeof(ICreateValidator<>).MakeGenericType(modelType), createValidator);
            }

            if (updateValidator is not null)
            {
                services.Decorate(typeof(IUpdateValidator<>).MakeGenericType(modelType), updateValidator);
            }
        }
    }

    private static void LoadValidatorTypes(Assembly servicesAssembly, Type modelType, out Type? createValidator,
        out Type? universalValidator, out Type? updateValidator)
    {
        updateValidator = servicesAssembly.GetTypes().FirstOrDefault(type =>
            type.IsClass && type.GetInterfaces()
                .Any(i => i == typeof(IUpdateValidator<>).MakeGenericType(modelType)));
        createValidator = servicesAssembly.GetTypes().FirstOrDefault(type =>
            type.IsClass && type.GetInterfaces()
                .Any(i => i == typeof(ICreateValidator<>).MakeGenericType(modelType)));
        universalValidator = servicesAssembly.GetTypes().FirstOrDefault(type =>
            type.IsClass && type.GetInterfaces()
                .Any(i => i == typeof(IUniversalValidator<>).MakeGenericType(modelType)));
    }

    private static void AddServicesWithValidation(IServiceCollection services, ValidationAttribute typeValidation,
        Type modelType)
    {
        if (typeValidation.ValidationType == ValidationTypes.None)
        {
            return;
        }

        if (typeValidation.UseUniversalValidator)
        {
            services.Decorate(typeof(IBaseService<>).MakeGenericType(modelType),
                typeof(ServiceWithUniversalValidation<>).MakeGenericType(modelType));
            return;
        }

        if (typeValidation.ValidationType == ValidationTypes.CreateOnly)
        {
            services.Decorate(typeof(IBaseService<>).MakeGenericType(modelType),
                typeof(ServiceWithCreateValidation<>).MakeGenericType(modelType));
            return;
        }

        services.Decorate(typeof(IBaseService<>).MakeGenericType(modelType),
            typeof(ServiceWithUpdateValidation<>).MakeGenericType(modelType));
    }

    private static void AddBaseValidators(IServiceCollection services, ValidationAttribute typeValidation,
        Type modelType)
    {
        if (typeValidation.ValidationType == ValidationTypes.None)
        {
            return;
        }

        if (typeValidation.UseUniversalValidator)
        {
            services.AddScoped(typeof(IUniversalValidator<>).MakeGenericType(modelType),
                typeof(UniversalValidator<>).MakeGenericType(modelType));
            return;
        }

        if (typeValidation.ValidationType != ValidationTypes.UpdateOnly)
        {
            services.TryAddScoped(typeof(ICreateValidator<>).MakeGenericType(modelType),
                typeof(CreateValidator<>).MakeGenericType(modelType));
        }

        if (typeValidation.ValidationType != ValidationTypes.CreateOnly)
        {
            services.TryAddScoped(typeof(IUpdateValidator<>).MakeGenericType(modelType),
                typeof(UpdateValidator<>).MakeGenericType(modelType));
        }
    }


    private static List<Type> GetModels(Assembly assembly)
        => assembly.GetTypes().Where(type => type.GetInterfaces().Any(i => i == typeof(IModel))).ToList();

    private static List<Type> GetDataTransferObjects(Assembly assembly)
        => assembly.GetTypes().Where(type => type.GetInterfaces().Any(i => i == typeof(IDataTransferObject))).ToList();

    private static List<Type> GetMultipleItemSelectors(Assembly assembly)
        => assembly.GetTypes().Where(type => type.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMultipleItemSelector<,>))).ToList();

    private static List<Type> GetQueryParameters(Assembly assembly)
        => assembly.GetTypes().Where(type => type.BaseType == typeof(QueryParameters)).ToList();

    private static List<Type> TypesRequiresValidation(Assembly assembly)
        => assembly.GetTypes().Where(type => type.GetCustomAttributes()
            .Any(attribute => attribute.GetType() == typeof(ValidationAttribute))).ToList();

    private static List<Type> GetRelationModels(Assembly assembly)
        => assembly.GetTypes().Where(type =>
            type.GetInterfaces().Any(i => i.IsGenericOfType(typeof(IModelWithManyToManyRelations<,>)))).ToList();

    private static bool IsGenericOfType(this Type type, Type genericType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
}