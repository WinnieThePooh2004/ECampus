using System.Diagnostics;
using System.Reflection;
using ECampus.Domain.Services;
using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.DataDeleteServices;
using ECampus.Infrastructure.DataSelectors.SingleItemSelectors;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Metadata.Relationships;
using ECampus.Shared.QueryParameters;

namespace ECampus.Api.Extensions;

// ReSharper disable once InconsistentNaming
internal static class IServiceCollectionExtensions
{
    public static void AddUniqueServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddRange(assemblies.SelectMany(assembly => assembly.GetTypes().Where(type =>
                type.GetCustomAttributes().Any(attribute => attribute.GetType() == typeof(InjectAttribute))))
            .SelectMany(type => type.GetCustomAttributes().OfType<InjectAttribute>().Select(attribute =>
                new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceLifetime))));
    }

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
        var relationModels = GetModelsWithManyToManyRelationships(assembly);
        foreach (var relationModel in relationModels)
        {
            var relationAttributes = relationModel.GetCustomAttributes(false).OfType<ManyToManyAttribute>();
            foreach (var attribute in relationAttributes)
            {
                services.Decorate(typeof(IDataUpdateService<>).MakeGenericType(relationModel),
                    typeof(DataUpdateServiceWithRelationships<,,>).MakeGenericType(relationModel, attribute.RelatedModel,
                        attribute.RelationModel));
                services.Decorate(typeof(IDataCreateService<>).MakeGenericType(relationModel),
                    typeof(DataCreateWithRelationships<,,>).MakeGenericType(relationModel,  attribute.RelatedModel,
                        attribute.RelationModel));
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

    public static void AddFluentValidationWrappers<TDto>(this IServiceCollection services, bool addServices = true)
        where TDto : class, IDataTransferObject
    {
        services.AddScoped<IUpdateValidator<TDto>, FluentValidatorWrapper<TDto>>();
        services.AddScoped<ICreateValidator<TDto>, FluentValidatorWrapper<TDto>>();
        if (!addServices)
        {
            return;
        }
        services.Decorate<IBaseService<TDto>, ServiceWithCreateValidation<TDto>>();
        services.Decorate<IBaseService<TDto>, ServiceWithUpdateValidation<TDto>>();
    }

    private static void AddRange(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            services.Add(descriptor);
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

    private static List<Type> GetModelsWithManyToManyRelationships(Assembly assembly)
        => assembly.GetTypes().Where(type => type.GetCustomAttributes(false).OfType<ManyToManyAttribute>().Any()).ToList();

    private static bool IsGenericOfType(this Type type, Type genericType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
}