namespace ECampus.Shared.Extensions;

public static class TypeExtensions
{
    public static bool IsGenericOfType(this Type type, Type genericType)
        => type.IsGenericType && type.GetGenericTypeDefinition() == genericType;

}