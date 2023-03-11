namespace ECampus.Domain.Metadata;

[AttributeUsage(AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string? Name { get; }
    
    public int DisplayOrder { get; }

    public DisplayNameAttribute(string? name = null, int displayOrder = int.MaxValue)
    {
        Name = name;
        DisplayOrder = displayOrder;
    }
}