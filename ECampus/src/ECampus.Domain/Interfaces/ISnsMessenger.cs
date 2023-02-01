using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Messages;

namespace ECampus.Domain.Interfaces;

public interface ISnsMessenger
{
    Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}