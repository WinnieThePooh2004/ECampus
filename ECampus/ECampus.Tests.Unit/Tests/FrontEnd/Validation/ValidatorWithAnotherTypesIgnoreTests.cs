using ECampus.FrontEnd.Validation;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using FluentValidation.Results;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Validation;

public class ValidatorWithAnotherTypesIgnoreTests
{
    private readonly ValidatorWithAnotherTypesIgnore<CourseDto> _sut;
    private readonly IValidator<CourseDto> _baseValidator = Substitute.For<IValidator<CourseDto>>();

    public ValidatorWithAnotherTypesIgnoreTests()
    {
        _sut = new ValidatorWithAnotherTypesIgnore<CourseDto>(_baseValidator);
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
    public async Task ValidateAsync_ShouldReturnFromBaseValidator_WhenInstancePassed()
    {
        var instance = new CourseDto();
        var expected = new ValidationResult();
        _baseValidator.ValidateAsync(instance).Returns(expected);

        var actual = await _sut.ValidateAsync(instance);

        actual.Should().Be(expected);
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnFromBaseValidator_WhenContextPassed()
    {
        var context = new ValidationContext<CourseDto>(new CourseDto());
        var expected = new ValidationResult();
        _baseValidator.ValidateAsync(context).Returns(expected);

        var actual = await _sut.ValidateAsync(context);

        actual.Should().Be(expected);
    }
    
    [Fact]
    public async Task ValidateAsync_ShouldReturnFromBaseValidator_WhenInvalidTypeContextPassed()
    {
        var context = new ValidationContext<TeacherDto>(new TeacherDto());
        var expected = new ValidationResult();
        _baseValidator.ValidateAsync(context).Returns(expected);

        var actual = await _sut.ValidateAsync(context);

        actual.Errors.Should().BeEmpty();
        await _baseValidator.DidNotReceive().ValidateAsync(context, Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public void CanValidateObjectsOfType_ShouldReturnTrue()
    {
        _sut.CanValidateInstancesOfType(typeof(Type)).Should().BeTrue();
    }
}