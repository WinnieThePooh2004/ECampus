using ECampus.Core.Messages;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Services.Contracts.Messaging;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services.Messaging;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Messaging;

public class CourseTaskMessagingServiceTests
{
    private readonly CourseTaskService _sut;
    private readonly IBaseService<CourseTaskDto> _baseService = Substitute.For<IBaseService<CourseTaskDto>>();
    private readonly ISnsMessenger _snsMessenger = Substitute.For<ISnsMessenger>();
    private readonly IDataAccessFacade _dataAccess = Substitute.For<IDataAccessFacade>();

    public CourseTaskMessagingServiceTests()
    {
        _sut = new CourseTaskService(_baseService, _snsMessenger, _dataAccess, MapperFactory.Mapper);
    }

    [Fact]
    public async Task Create_ShouldNotPublishMessage_WhenNoStudentEmailFound()
    {
        var task = new CourseTaskDto { CourseId = 10 };
        _baseService.CreateAsync(task).Returns(task);
        _dataAccess.GetByIdOrDefaultAsync<Course>(10).Returns(new Course());
        var returnData = new DbSetMock<Student>().Object;
        _dataAccess.GetByParameters<Student, StudentsByCourseParameters>(Arg.Any<StudentsByCourseParameters>())
            .Returns(returnData);

        await _sut.CreateAsync(task);

        await _snsMessenger.DidNotReceive().PublishMessageAsync(Arg.Any<IMessage>());
    }

    [Fact]
    public async Task Create_ShouldPublishMessage_WhenStudentEmailsFound()
    {
        var deadline = DateTime.Now;
        var task = new CourseTaskDto
            { CourseId = 10, MaxPoints = 100, Deadline = deadline, Name = "name", Type = TaskType.Classwork };
        _baseService.CreateAsync(task).Returns(task);
        var courses = new DbSetMock<Course>(new Course { Name = "courseName" }).Object;
        _dataAccess.GetByParameters<Course, PureByIdParameters<Course>>(Arg.Any<PureByIdParameters<Course>>())
            .Returns(courses);
        var returnData = new DbSetMock<Student>(new Student { UserEmail = "email" }).Object;
        _dataAccess.GetByParameters<Student, StudentsByCourseParameters>(Arg.Any<StudentsByCourseParameters>())
            .Returns(returnData);

        await _sut.CreateAsync(task);

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