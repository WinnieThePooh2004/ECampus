namespace ECampus.Shared.Messaging;

/// <summary>
/// marker interface, only objects implementing it can be used as generic parameter of ISqsMessenger,
/// also implementing it to type TMessage will also inject ISqsMessenger of TMessage
/// </summary>
public interface IMessage
{
    
}