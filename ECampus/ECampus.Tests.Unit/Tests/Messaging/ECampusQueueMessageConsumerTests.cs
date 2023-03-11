using Amazon.SQS;
using Amazon.SQS.Model;
using ECampus.Core.Messages;
using ECampus.Messaging.MessagingServices;
using ECampus.Messaging.Options;
using ECampus.Tests.Shared.Mocks;
using MediatR;
using Newtonsoft.Json;
using Serilog;

namespace ECampus.Tests.Unit.Tests.Messaging;

public class ECampusQueueMessageConsumerTests
{
    private readonly ECampusQueueMessageConsumer _sut;
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger _logger = Substitute.For<ILogger>();
    private readonly IAmazonSQS _amazonSqs = Substitute.For<IAmazonSQS>();

    public ECampusQueueMessageConsumerTests()
    {
        Options<QueueSettings> options = new(new QueueSettings
        {
            MessagesNamespace = typeof(PasswordChanged).Namespace!,
            Name = "QueueName",
            Delay = 10
        });
        _sut = new ECampusQueueMessageConsumer(_amazonSqs, options, _mediator, _logger);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLog_WhenNoMessagesFound()
    {
        _amazonSqs.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ReceiveMessageResponse());
        _amazonSqs.GetQueueUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new GetQueueUrlResponse { QueueUrl = "url" });

        await _sut.StartAsync(CancellationToken.None);
        await Task.Delay(50);
        await _sut.StopAsync(CancellationToken.None);

        _logger.Received().Information("No messages found in queue {Name}", "QueueName");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLogWhenTypeIsNull()
    {
        _amazonSqs.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ReceiveMessageResponse { Messages = new List<Message> { CreateMessage(10) } });
        _amazonSqs.GetQueueUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new GetQueueUrlResponse { QueueUrl = "url" });

        await _sut.StartAsync(CancellationToken.None);
        await Task.Delay(50);
        await _sut.StopAsync(CancellationToken.None);

        _logger.Received(1).Warning("Unknown message type: {MessageType}", nameof(Int32));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldLog_WhenMediatorThrowsException()
    {
        _amazonSqs.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ReceiveMessageResponse
                { Messages = new List<Message> { CreateMessage(new PasswordChanged { Email = "", Username = "" }) } });
        _amazonSqs.GetQueueUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new GetQueueUrlResponse { QueueUrl = "url" });
        var exception = new Exception("message");
        await _mediator.Send(Arg.Do<PasswordChanged>(_ => throw exception),
            Arg.Any<CancellationToken>());

        await _sut.StartAsync(CancellationToken.None);
        await Task.Delay(50);
        await _sut.StopAsync(CancellationToken.None);

        _logger.Received(1).Error(exception, "Exception occured during sending request with message of type {Type}",
            typeof(PasswordChanged));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldDeleteMessage_WhenItWasSuccessfullyProcessed()
    {
        _amazonSqs.ReceiveMessageAsync(Arg.Any<ReceiveMessageRequest>(), Arg.Any<CancellationToken>())
            .Returns(new ReceiveMessageResponse
                { Messages = new List<Message> { CreateMessage(new PasswordChanged { Email = "", Username = "" }) } });
        _amazonSqs.GetQueueUrlAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new GetQueueUrlResponse { QueueUrl = "url" });

        await _sut.StartAsync(CancellationToken.None);
        await Task.Delay(50);
        await _sut.StopAsync(CancellationToken.None);

        await _amazonSqs.Received(1).DeleteMessageAsync("url", "message", Arg.Any<CancellationToken>());
    }

    private static Message CreateMessage<TMessage>(TMessage message)
    {
        return new Message
        {
            Body = JsonConvert.SerializeObject(message),
            ReceiptHandle = nameof(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                ["MessageType"] = new()
                {
                    StringValue = typeof(TMessage).Name
                }
            }
        };
    }
}