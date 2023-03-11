using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Messages;

namespace ECampus.Services.Contracts.Messaging;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}