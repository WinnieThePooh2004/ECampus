using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Requests.TaskSubmission;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.TaskSubmission;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services.ValidationServices;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class ServiceWithParametersValidationTests
{
    private readonly HandlerWithGetByParametersValidation<MultipleTaskSubmissionResponse, TaskSubmissionParameters> _sut;

    private readonly IGetByParametersHandler<MultipleTaskSubmissionResponse, TaskSubmissionParameters> _baseHandler =
        Substitute.For<IGetByParametersHandler<MultipleTaskSubmissionResponse, TaskSubmissionParameters>>();

    private readonly IParametersValidator<TaskSubmissionParameters> _parametersValidator =
        Substitute.For<IParametersValidator<TaskSubmissionParameters>>();

    private readonly Fixture _fixture = new();

    public ServiceWithParametersValidationTests()
    {
        _sut = new HandlerWithGetByParametersValidation<MultipleTaskSubmissionResponse, TaskSubmissionParameters>(_baseHandler,
            _parametersValidator);
    }

    [Fact]
    public async Task GetByParameters_ShouldReturnFromBaseService_WhenParametersAreValid()
    {
        var parameters = _fixture.Create<TaskSubmissionParameters>();
        _parametersValidator.ValidateAsync(parameters).Returns(new ValidationResult());
        var expected = new ListWithPaginationData<MultipleTaskSubmissionResponse>();
        _baseHandler.GetByParametersAsync(parameters).Returns(expected);

        var result = await _sut.GetByParametersAsync(parameters);

        result.Should().Be(expected);
    }
    
    [Fact]
    public async Task GetByParameters_ShouldThrowException_WhenParametersAreNotValid()
    {
        var parameters = _fixture.Create<TaskSubmissionParameters>();
        var validationResult = new ValidationResult(new ValidationError("property", "message"));
        _parametersValidator.ValidateAsync(parameters).Returns(validationResult);

        await new Func<Task>(() => _sut.GetByParametersAsync(parameters)).Should()
            .ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(TaskSubmissionParameters), validationResult).Message);
    }
}