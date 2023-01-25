using Amazon.SQS;
using Amazon.SQS.Model;
using ECampus.Shared.Interfaces.Messaging;
using ECampus.Shared.Messaging;
using ECampus.Shared.Messaging.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ECampus.Domain.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private static string? _queueUrl;

    private readonly IAmazonSQS _amazonSqs;
    private readonly IOptions<QueueSettings> _options;

    public SqsMessenger(IAmazonSQS amazonSqs, IOptions<QueueSettings> options)
    {
        _amazonSqs = amazonSqs;
        _options = options;
    }

    public async Task<SendMessageResponse> SendMessageAsync<TMessage>(TMessage message)
        where TMessage : IMessage
    {
        var url = await GetQueueUrl();
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = url,
            MessageBody = JsonConvert.SerializeObject(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                ["MessageType"] = new()
                {
                    DataType = "String",
                    StringValue = typeof(TMessage).Name
                }
            }
        };

        return await _amazonSqs.SendMessageAsync(sendMessageRequest);
    }

    private async Task<string> GetQueueUrl() =>
        _queueUrl ??= (await _amazonSqs.GetQueueUrlAsync(_options.Value.Name)).QueueUrl;
}