using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Shared.Messaging.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ECampus.Domain.Messaging;

public class SnsMessenger : ISnsMessenger
{
    private string? _topicArn;

    private readonly IAmazonSimpleNotificationService _amazonSns;
    private readonly IOptions<NotificationsSettings> _options;

    public SnsMessenger(IAmazonSimpleNotificationService amazonSns, IOptions<NotificationsSettings> options)
    {
        _amazonSns = amazonSns;
        _options = options;
    }

    public async Task<PublishResponse> PublishMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage
    {
        var url = await GetQueueUrl();
        var sendMessageRequest = new PublishRequest
        {
            TopicArn = url,
            Message = JsonConvert.SerializeObject(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                ["MessageType"] = new()
                {
                    DataType = "String",
                    StringValue = typeof(TMessage).Name
                }
            }
        };

        return await _amazonSns.PublishAsync(sendMessageRequest);
    }

    private async Task<string> GetQueueUrl() =>
        _topicArn ??= (await _amazonSns.FindTopicAsync(_options.Value.Name)).TopicArn;
}