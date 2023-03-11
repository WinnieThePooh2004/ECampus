using System.Reflection;
using ECampus.Core.Installers;
using ECampus.Shared.Data;
using ECampus.Shared.Metadata;

namespace ECampus.Shared.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetModels(this Assembly assembly) =>
        assembly.GetTypes().Where(type =>
            type.IsAssignableTo(typeof(IModel)) && type is { IsAbstract: false, IsClass: true } &&
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());

    public static IEnumerable<Type> GetDataTransferObjects(this Assembly assembly) =>
        assembly.GetTypes().Where(type =>
            type.GetCustomAttributes(typeof(DtoAttribute), true).Any() &&
            type is { IsAbstract: false, IsClass: true } && 
            !type.GetCustomAttributes(typeof(InstallerIgnoreAttribute), false).Any());
}