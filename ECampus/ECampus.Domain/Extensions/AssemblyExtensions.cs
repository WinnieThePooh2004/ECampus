using System.Reflection;
using ECampus.Core.Installers;
using ECampus.Domain.Data;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetEntities(this Assembly assembly) =>
        assembly.GetTypes().Where(type =>
            type.IsAssignableTo(typeof(IEntity)) && type is { IsAbstract: false, IsClass: true } &&
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

    public static IEnumerable<Type> GetDataTransferObjects(this Assembly assembly) =>
        assembly.GetTypes().Where(type =>
            type.GetCustomAttributes(typeof(DtoAttribute), true).Any() &&
            type is { IsAbstract: false, IsClass: true } && 
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());
}