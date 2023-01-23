using ECampus.Infrastructure.DataCreateServices;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Infrastructure.Relationships;
using ECampus.Shared;
using ECampus.Shared.Installers;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Metadata;
using ECampus.Shared.Metadata.Relationships;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Infrastructure.Installers;

public class RelationshipsServicesInstaller : IInstaller
{
    public int InstallOrder => 10;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var relationModels = typeof(SharedAssemblyMarker).Assembly.GetTypes()
            .Where(type => type.GetCustomAttributes(typeof(ManyToManyAttribute), false).Any() &&
                           !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any()).ToList();

        foreach (var modelWithRelations in relationModels)
        {
            var relationAttributes =
                (ManyToManyAttribute[])modelWithRelations.GetCustomAttributes(typeof(ManyToManyAttribute), false);
            DecorateCurrentServices(services, relationAttributes, modelWithRelations);
        }
    }

    private static void DecorateCurrentServices(IServiceCollection services, IEnumerable<ManyToManyAttribute> relationAttributes,
        Type modelWithRelations)
    {
        foreach (var attribute in relationAttributes)
        {
            DecorateUpdateService(services, modelWithRelations, attribute);
            DecorateCreateService(services, modelWithRelations, attribute);
            AddRelationHelperServices(services, modelWithRelations, attribute);
        }
    }

    private static void AddRelationHelperServices(IServiceCollection services, Type modelWithRelations,
        ManyToManyAttribute attribute)
    {
        services.AddSingleton(
            typeof(IRelationshipsHandler<,,>).MakeGenericType(modelWithRelations, attribute.RelatedModel,
                attribute.RelationModel),
            typeof(RelationshipsHandler<,,>).MakeGenericType(modelWithRelations, attribute.RelatedModel,
                attribute.RelationModel));

        services.AddSingleton(
            typeof(IRelationsDataAccess<,,>).MakeGenericType(modelWithRelations, attribute.RelatedModel,
                attribute.RelationModel),
            typeof(RelationsDataAccess<,,>).MakeGenericType(modelWithRelations, attribute.RelatedModel,
                attribute.RelationModel));
    }

    private static void DecorateCreateService(IServiceCollection services, Type modelWithRelations,
        ManyToManyAttribute attribute)
    {
        services.Decorate(typeof(IDataCreateService<>).MakeGenericType(modelWithRelations),
            typeof(ManyToManyRelationshipsCreate<,,>).MakeGenericType(modelWithRelations,
                attribute.RelatedModel, attribute.RelationModel));
    }

    private static void DecorateUpdateService(IServiceCollection services, Type modelWithRelations,
        ManyToManyAttribute attribute)
    {
        services.Decorate(typeof(IDataUpdateService<>).MakeGenericType(modelWithRelations),
            typeof(ManyToManyRelationshipsUpdate<,,>).MakeGenericType(modelWithRelations,
                attribute.RelatedModel, attribute.RelationModel));
    }
}