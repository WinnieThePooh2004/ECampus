using ECampus.Contracts.Services;
using ECampus.Core.Messages;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Messaging;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class TaskSubmissionMessagingServiceTests
{
    private readonly TaskSubmissionMessagingService _sut;
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();
    private readonly ITaskSubmissionService _baseService = Substitute.For<ITaskSubmissionService>();

    public TaskSubmissionMessagingServiceTests()
    {
        _sut = new TaskSubmissionMessagingService(_baseService, _snsMessenger);
    }

    [Fact]
    public async Task UpdateMark_ShouldSendMessage_WhenMarkUpdated()
    {
        var submission = new TaskSubmissionDto
        {
            CourseTask = new CourseTaskDto { MaxPoints = 15, Name = "name" },
            Student = new StudentDto { UserEmail = "email" },
            TotalPoints = 10
        };
        _baseService.UpdateMarkAsync(10, 20).Returns(submission);

        await _sut.UpdateMarkAsync(10, 20);

        await _snsMessenger.Received(1).PublishMessageAsync(Arg.Is<SubmissionMarked>(s
            => s.MaxPoints == 15 && s.ScoredPoints == 10 && s.UserEmail == "email" && s.TaskName == "name"));
    }
    
    [Fact]
    public async Task UpdateContent_ShouldSendMessage_WhenMarkUpdated()
    {
        var submission = new TaskSubmissionDto
        {
            CourseTask = new CourseTaskDto { Name = "name" },
            Student = new StudentDto { UserEmail = "email" }
        };
        _baseService.UpdateContentAsync(10, "").Returns(submission);

        await _sut.UpdateContentAsync(10, "");

        await _snsMessenger.Received(1).PublishMessageAsync(Arg.Is<SubmissionEdited>(s
            => s.UserEmail == "email" && s.TaskName == "name"));
    }

    [Fact]
    public async Task GetById_ShouldReturnFromBaseService()
    {
        var submission = new TaskSubmissionDto();
        _baseService.GetByIdAsync(10).Returns(submission);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(submission);
    }

    [Fact]
    public async Task GetByCourseTask_ShouldReturnFromBaseService()
    {
        var submission = new TaskSubmissionDto();
        _baseService.GetByCourseAsync(10).Returns(submission);

        var result = await _sut.GetByCourseAsync(10);

        result.Should().Be(submission);
    }
}