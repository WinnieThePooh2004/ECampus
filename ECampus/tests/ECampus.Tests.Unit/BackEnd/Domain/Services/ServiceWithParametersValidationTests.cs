using ECampus.Domain.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class ServiceWithParametersValidationTests
{
    private readonly ServiceWithParametersValidation<TaskSubmissionDto, TaskSubmissionParameters> _sut;

    private readonly IParametersService<TaskSubmissionDto, TaskSubmissionParameters> _baseService =
        Substitute.For<IParametersService<TaskSubmissionDto, TaskSubmissionParameters>>();

    private readonly IParametersValidator<TaskSubmissionParameters> _parametersValidator =
        Substitute.For<IParametersValidator<TaskSubmissionParameters>>();

    private readonly Fixture _fixture = new();

    public ServiceWithParametersValidationTests()
    {
        _sut = new ServiceWithParametersValidation<TaskSubmissionDto, TaskSubmissionParameters>(_baseService,
            _parametersValidator);
    }

    [Fact]
    public async Task GetByParameters_ShouldReturnFromBaseService_WhenParametersAreValid()
    {
        var parameters = _fixture.Create<TaskSubmissionParameters>();
        _parametersValidator.ValidateAsync(parameters).Returns(new ValidationResult());
        var expected = new ListWithPaginationData<TaskSubmissionDto>();
        _baseService.GetByParametersAsync(parameters).Returns(expected);

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