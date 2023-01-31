using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Messages;
using ECampus.Shared.Messaging;

namespace ECampus.Shared.Interfaces.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}