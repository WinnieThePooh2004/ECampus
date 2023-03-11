namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Messaging;

public class SnsMessengerTests
{
    // private readonly SnsMessenger _sut;
    // private readonly IAmazonSimpleNotificationService _amazonSns = Substitute.For<IAmazonSimpleNotificationService>();
    // private readonly Options<NotificationsSettings> _options;
    //
    // public SnsMessengerTests()
    // {
    //     _options = new Options<NotificationsSettings>(new NotificationsSettings { Name = "name" });
    //     _sut = new SnsMessenger(_amazonSns, _options);
    // }
    //
    // [Fact]
    // public async Task SendMessage_ShouldReturnFromAmazonService()
    // {
    //     _amazonSns.FindTopicAsync(_options.Value.Name).Returns(new Topic { TopicArn = "topicArn" });
    //     PublishRequest? request = null;
    //     await _amazonSns.PublishAsync(Arg.Do<PublishRequest>(r => request = r));
    //     var userDeleted = new UserDeleted { Email = "Email", Username = "username" };
    //     var response = new PublishResponse();
    //     _amazonSns.PublishAsync(Arg.Any<PublishRequest>()).Returns(response);
    //
    //     var result = await _sut.PublishMessageAsync(userDeleted);
    //
    //     result.Should().Be(response);
    //     request.Should().NotBeNull();
    //     request!.TopicArn.Should().Be("topicArn");
    //     request.MessageAttributes.Should().ContainEquivalentOf(KeyValuePair.Create("MessageType", new MessageAttributeValue
    //     {
    //         DataType = "String",
    //         StringValue = nameof(UserDeleted)
    //     }));
    //     request.Message.Should().Be(JsonConvert.SerializeObject(userDeleted));
    // }
    //
    // [Fact]
    // public async Task SendMessage_ShouldCallAmazonServiceOnce_WhenSendMessageTwice()
    // {
    //     _amazonSns.FindTopicAsync(_options.Value.Name).Returns(new Topic { TopicArn = "topicArn" });
    //
    //     await _sut.PublishMessageAsync(new UserDto().ToUserDeletedMessage());
    //     await _sut.PublishMessageAsync(new UserDto().ToCreatedUserMessage());
    //
    //     await _amazonSns.Received(1).FindTopicAsync(Arg.Any<string>());
    // }
}