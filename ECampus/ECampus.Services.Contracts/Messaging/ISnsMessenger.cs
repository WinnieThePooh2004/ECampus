using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Messages;

namespace ECampus.Services.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}