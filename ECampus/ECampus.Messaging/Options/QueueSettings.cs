namespace ECampus.Messaging.Options;

public class QueueSettings
{
    public const string Key = "Queue";
    public required string Name { get; init; }
    public required string MessagesNamespace { get; init; }
    
    public required int Delay { get; init; }
}