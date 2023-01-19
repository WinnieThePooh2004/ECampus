namespace UniversityTimetable.Shared.Metadata.Relationships;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class LeftTableIdAttribute : Attribute
{
    public Type LeftTableType { get; }
    public Type RightTableType { get; }

    public LeftTableIdAttribute(Type leftTableType, Type rightTableType)
    {
        LeftTableType = leftTableType;
        RightTableType = rightTableType;
    }
}