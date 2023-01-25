using Amazon.SimpleNotificationService.Model;
using ECampus.Shared.Messaging;

namespace ECampus.Shared.Interfaces.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> SendMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}