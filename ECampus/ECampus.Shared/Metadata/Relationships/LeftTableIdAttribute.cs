namespace ECampus.Shared.Metadata.Relationships;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class LeftTableIdAttribute : Attribute
{
    public Type LeftTableType { get; }

    public LeftTableIdAttribute(Type leftTableType)
    {
        LeftTableType = leftTableType;
    }
}