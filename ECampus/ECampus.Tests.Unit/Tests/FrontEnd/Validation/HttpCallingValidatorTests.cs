using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Validation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Validation;

public class HttpCallingValidatorTests
{
    private readonly HttpCallingValidator<CourseDto> _sut;

    private readonly IValidationRequests<CourseDto> _validationRequests =
        Substitute.For<IValidationRequests<CourseDto>>();

    private readonly IValidator<CourseDto> _baseValidator = Substitute.For<IValidator<CourseDto>>();

    public HttpCallingValidatorTests()
    {
        _sut = new HttpCallingValidator<CourseDto>(_baseValidator, _validationRequests);
    }

    [Fact]
    public void CreateDescriptor_ShouldReturnFromBaseValidator()
    {
        var expected = new ValidatorDescriptor<CourseDto>(new List<IValidationRule>());
        _baseValidator.CreateDescriptor().Returns(expected);

        var actual = _sut.CreateDescriptor();

        actual.Should().Be(expected);
    }

    [Fact]
    public void Validate_ShouldReturnFromBaseValidator_WhenInstancePassed()
    {
        var instance = new CourseDto();
        var expected = new ValidationResult();
        _baseValidator.Validate(instance).Returns(expected);

        var actual = _sut.Validate(instance);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Validate_ShouldReturnFromBaseValidator_WhenContextPassed()
    {
        var context = new ValidationContext<CourseDto>(new CourseDto());
        var expected = new ValidationResult();
        _baseValidator.Validate(context).Returns(expected);

        var actual = _sut.Validate(context);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task ValidateAsync_ShouldNotCallRequests_WhenBaseValidatorReturnsErrors()
    {
        var instance = new CourseDto();
        var expected = new ValidationResult(new List<ValidationFailure> { new() });
        _baseValidator.ValidateAsync(instance, Arg.Any<CancellationToken>()).Returns(expected);

        var actual = await _sut.ValidateAsync(instance);

        actual.Should().Be(expected);
        await _validationRequests.DidNotReceive().ValidateAsync(instance);
    }

    [Fact]
    public async Task ValidateAsync_ShouldCallRequests_WhenBaseValidatorNotReturnErrors()
    {
        var instance = new CourseDto();
        var expected = new ValidationResult();
        var expectedErrors = new List<ValidationError> { new("b", "a"), new("a", "b") };
        _validationRequests.ValidateAsync(instance)
            .Returns(new ECampus.Shared.Validation.ValidationResult(expectedErrors));
        _baseValidator.ValidateAsync(instance, Arg.Any<CancellationToken>()).Returns(expected);

        var actual = await _sut.ValidateAsync(instance);

        actual.Should().Be(expected);
        actual.Errors.Select(e => (e.PropertyName, e.ErrorMessage)).Should()
            .BeEquivalentTo(expectedErrors.Select(e => (e.PropertyName, e.ErrorMessage)));
        await _validationRequests.Received(1).ValidateAsync(instance);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnEmptyResult_WhenAnotherTypePassed()
    {
        var context = new ValidationContext<AuditoryDto>(new AuditoryDto());

        var result = await _sut.ValidateAsync(context);

        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnFromBaseValidator_WhenCorrectInstancePassed()
    {
        var context = new ValidationContext<CourseDto>(new CourseDto());
        var expected = new ValidationResult(new List<ValidationFailure> { new() });
        _baseValidator.ValidateAsync(context.InstanceToValidate, Arg.Any<CancellationToken>()).Returns(expected);

        var actual = await _sut.ValidateAsync(context);

        actual.Should().Be(expected);
    }

    [Fact]
    public void CanValidateObjectsOfType_ShouldReturnFromBase()
    {
        _baseValidator.CanValidateInstancesOfType(typeof(Type)).Returns(true);
        
        _sut.CanValidateInstancesOfType(typeof(Type)).Returns(true);
    }
}