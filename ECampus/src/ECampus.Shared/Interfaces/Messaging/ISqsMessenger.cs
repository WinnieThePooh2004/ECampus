using Amazon.SQS.Model;
using ECampus.Shared.Messaging;

namespace ECampus.Shared.Interfaces.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage;
}