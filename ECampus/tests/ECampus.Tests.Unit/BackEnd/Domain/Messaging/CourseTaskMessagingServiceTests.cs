using ECampus.Core.Messages;
using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Messaging;

namespace ECampus.Tests.Unit.BackEnd.Domain.Messaging;

public class CourseTaskMessagingServiceTests
{
    private readonly CourseTaskMessagingService _sut;
    private readonly IBaseService<CourseTaskDto> _baseService = Substitute.For<IBaseService<CourseTaskDto>>();
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();
    private readonly ICourseTaskMessageDataAccess _messageDataAccess = Substitute.For<ICourseTaskMessageDataAccess>();

    public CourseTaskMessagingServiceTests()
    {
        _sut = new CourseTaskMessagingService(_baseService, _snsMessenger, _messageDataAccess);
    }

    [Fact]
    public async Task Create_ShouldNotPublishMessage_WhenNoStudentEmailFound()
    {
        var task = new CourseTaskDto { CourseId = 10 };
        _baseService.CreateAsync(task).Returns(task);
        _messageDataAccess.LoadDataForSendMessage(10).Returns(("", new List<string>()));

        var result = await _sut.CreateAsync(task);

        result.Should().Be(task);
        await _baseService.Received(1).CreateAsync(task);
        await _snsMessenger.DidNotReceive().PublishMessageAsync(Arg.Any<IMessage>());
    }

    [Fact]
    public async Task Create_ShouldPublishMessage_WhenStudentEmailsFound()
    {
        var deadline = DateTime.Now;
        var task = new CourseTaskDto
            { CourseId = 10, MaxPoints = 100, Deadline = deadline, Name = "name", Type = TaskType.Classwork };
        _baseService.CreateAsync(task).Returns(task);
        _messageDataAccess.LoadDataForSendMessage(10).Returns(("courseName", new List<string> { "email" }));

        var result = await _sut.CreateAsync(task);

        result.Should().Be(task);
        await _baseService.Received(1).CreateAsync(task);
        await _snsMessenger.Received(1).PublishMessageAsync(Arg.Is<TaskCreated>(t =>
            t.CourseName == "courseName" && t.Deadline == deadline && t.StudentEmails.Count == 1 &&
            t.StudentEmails.Contains("email") && t.TaskType == nameof(TaskType.Classwork) && t.MaxPoints == 100));
    }

    [Fact]
    public async Task GetById_ShouldReturnFromBaseService()
    {
        var task = new CourseTaskDto();
        _baseService.GetByIdAsync(10).Returns(task);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(task);
        await _baseService.Received(1).GetByIdAsync(10);
    }

    [Fact]
    public async Task Delete_ShouldReturnFromBaseService()
    {
        var task = new CourseTaskDto();
        _baseService.DeleteAsync(10).Returns(task);

        var result = await _sut.DeleteAsync(10);

        result.Should().Be(task);
        await _baseService.Received(1).DeleteAsync(10);
    }

    [Fact]
    public async Task Update_ShouldReturnFromBaseService()
    {
        var task = new CourseTaskDto();
        _baseService.UpdateAsync(task).Returns(task);

        var result = await _sut.UpdateAsync(task);

        result.Should().Be(task);
        await _baseService.Received(1).UpdateAsync(task);
    }
}