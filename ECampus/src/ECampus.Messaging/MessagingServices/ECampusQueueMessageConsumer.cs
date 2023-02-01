using Amazon.SQS;
using Amazon.SQS.Model;
using ECampus.Core.Messages;
using ECampus.Messaging.Options;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessagingServices;

public class ECampusQueueMessageConsumer : BackgroundService
{
    private readonly IAmazonSQS _amazonSqs;
    private readonly IOptions<QueueSettings> _options;
    private string? _queueUrl;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ECampusQueueMessageConsumer(IAmazonSQS amazonSqs, IOptions<QueueSettings> options, IMediator mediator, ILogger logger)
    {
        _amazonSqs = amazonSqs;
        _options = options;
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = await GetQueueUrl(stoppingToken),
            AttributeNames = new List<string> { "All" },
            MessageAttributeNames = new List<string> { "All" }
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            await HandleResponse(receiveMessageRequest, stoppingToken);
        }
    }

    private async Task HandleResponse(ReceiveMessageRequest receiveMessageRequest, CancellationToken stoppingToken)
    {
        var response = await _amazonSqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
        if (response.Messages.Count == 0)
        {
            _logger.Information("No messages found in queue {Name}", _options.Value.Name);
            await Task.Delay(30000, stoppingToken);
            return;
        }
        foreach (var message in response.Messages)
        {
            await HandleMessage(message, stoppingToken);
        }

        await Task.Delay(1000, stoppingToken);
    }

    private async Task HandleMessage(Message message, CancellationToken stoppingToken)
    {
        var messageType = message.MessageAttributes["MessageType"].StringValue;
        var typeName = $"{_options.Value.MessagesNamespace}.{messageType}, ECampus.Core";
        var type = Type.GetType(typeName);
        if (type is null)
        {
            _logger.Warning("Unknown message type: {MessageType}", messageType);
            return;
        }

        await ProcessMessage(message, type, stoppingToken);
    }

    private async Task ProcessMessage(Message message, Type type, CancellationToken stoppingToken)
    {
        var typedMessage = (ISqsMessage)JsonConvert.DeserializeObject(message.Body, type)!;
        try
        {
            await _mediator.Send(typedMessage, stoppingToken);
        }
        catch (Exception e)
        {
            _logger.Error(e,
                "Exception occured during sending request with message of type {Type}", type);
            return;
        }

        await _amazonSqs.DeleteMessageAsync(_queueUrl!, message.ReceiptHandle, stoppingToken);
    }

    private async Task<string> GetQueueUrl(CancellationToken stoppingToken) => 
        _queueUrl ??= (await _amazonSqs.GetQueueUrlAsync(_options.Value.Name, stoppingToken)).QueueUrl;
}