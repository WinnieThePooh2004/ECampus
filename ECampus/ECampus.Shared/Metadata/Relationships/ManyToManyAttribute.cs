namespace ECampus.Shared.Metadata.Relationships;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ManyToManyAttribute : Attribute
{
    public Type RelatedModel { get; }
    public Type RelationModel { get; }

    public ManyToManyAttribute(Type relatedModel, Type relationModel)
    {
        RelatedModel = relatedModel;
        RelationModel = relationModel;
    }
}