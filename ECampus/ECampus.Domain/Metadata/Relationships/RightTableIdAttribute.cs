namespace ECampus.Domain.Metadata.Relationships;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RightTableIdAttribute : Attribute
{
    public Type RightTableType { get; }

    public RightTableIdAttribute(Type rightTableType)
    {
        RightTableType = rightTableType;
    }
}