using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.SingleItemSelectors;
using ECampus.Infrastructure.Repositories;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.Repositories;

public class TaskSubmissionRepositoryTests
{
    // private readonly SingleTaskSubmissionSelector _sut;
    // private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    //
    // public TaskSubmissionRepositoryTests()
    // {
    //     _sut = new SingleTaskSubmissionSelector(_context);
    // }
    //
    // [Fact]
    // public async Task UpdateContent_ShouldUpdateContent_WhenSubmissionExist()
    // {
    //     var submission = new TaskSubmission { Id = 20 };
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>(submission);
    //
    //     await _sut.UpdateContentAsync(20, "new content");
    //
    //     submission.SubmissionContent.Should().Be("new content");
    // }
    //
    // [Fact]
    // public async Task UpdateMark_ShouldUpdateMark_WhenSubmissionExist()
    // {
    //     var submission = new TaskSubmission { Id = 20 };
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>(submission);
    //
    //     await _sut.UpdateMarkAsync(20, 100);
    //
    //     submission.TotalPoints.Should().Be(100);
    // }
    //
    // [Fact]
    // public async Task UpdateContent_ShouldThrowException_WhenSubmissionNotFound()
    // {
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>();
    //
    //     await new Func<Task>(() => _sut.UpdateContentAsync(10, "")).Should()
    //         .ThrowAsync<ObjectNotFoundByIdException>()
    //         .WithMessage(new ObjectNotFoundByIdException(typeof(TaskSubmission), 10).Message);
    // }
    //
    // [Fact]
    // public async Task UpdateMark_ShouldThrowException_WhenSubmissionNotFound()
    // {
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>();
    //
    //     await new Func<Task>(() => _sut.UpdateMarkAsync(10, 10)).Should()
    //         .ThrowAsync<ObjectNotFoundByIdException>()
    //         .WithMessage(new ObjectNotFoundByIdException(typeof(TaskSubmission), 10).Message);
    // }
    //
    // [Fact]
    // public async Task GetByStudentAndCourse_ShouldReturnFoundSubmission_WhenExistInDb()
    // {
    //     var submission = new TaskSubmission { Id = 20, StudentId = 10, CourseTaskId = 220 };
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>(submission);
    //
    //     var result = await _sut.GetByStudentAndCourseAsync(10, 220);
    //
    //     result.Should().Be(submission);
    // }
    //
    // [Fact]
    // public async Task GetByStudentAndCourse_ShouldThrowException_WhenNoObjectFound()
    // {
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>();
    //
    //     await new Func<Task>(() => _sut.GetByStudentAndCourseAsync(10, 10)).Should()
    //         .ThrowAsync<InfrastructureExceptions>()
    //         .WithMessage("There is not any submissions with TaskId = 10 and StudentId = 10\nError code: 404");
    // }
    //
    // [Fact]
    // public async Task GetById_ShouldReturnFromDb_WhenExistInDb()
    // {
    //     var submission = new TaskSubmission { Id = 1 };
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>(submission);
    //
    //     var result = await _sut.GetByIdAsync(1);
    //
    //     result.Should().Be(submission);
    // }
    //
    // [Fact]
    // public async Task GetById_ShouldThrowException_WhenNotFoundInDb()
    // {
    //     _context.TaskSubmissions = new DbSetMock<TaskSubmission>();
    //
    //     await new Func<Task>(() => _sut.GetByIdAsync(1)).Should()
    //         .ThrowAsync<ObjectNotFoundByIdException>()
    //         .WithMessage(new ObjectNotFoundByIdException(typeof(TaskSubmission), 1).Message);
    // }
}