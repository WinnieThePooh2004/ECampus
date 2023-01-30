using ECampus.Infrastructure;
using ECampus.Infrastructure.ValidationDataAccess;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.Validation;

public class TaskSubmissionValidationDataAccessTests
{
    private readonly TaskSubmissionValidationDataAccess _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public TaskSubmissionValidationDataAccessTests()
    {
        _sut = new TaskSubmissionValidationDataAccess(_context);
    }

    [Fact]
    public async Task LoadRequiredDataForCreateAsync_ShouldReturnDefault()
    {
        var @default = new TaskSubmission();
        var result = await _sut.LoadRequiredDataForCreateAsync(@default);

        result.Should().BeEquivalentTo(@default);
        result.Should().NotBe(@default);
    }

    [Fact]
    public async Task LoadRequiredDataForUpdateAsync_ShouldReturnFromContext_IfModelExist()
    {
        var submissions = new TaskSubmission { Id = 1 };
        _context.FindAsync<TaskSubmission>(1).Returns(submissions);

        var result = await _sut.LoadRequiredDataForUpdateAsync(submissions);

        result.Should().Be(submissions);
    }

    [Fact]
    public async Task LoadRequiredDataForUpdateAsync_ShouldThrowException_WhenFountNull()
    {
        await new Func<Task>(() => _sut.LoadRequiredDataForUpdateAsync(new TaskSubmission())).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(TaskSubmission), 0).Message);
    }
}