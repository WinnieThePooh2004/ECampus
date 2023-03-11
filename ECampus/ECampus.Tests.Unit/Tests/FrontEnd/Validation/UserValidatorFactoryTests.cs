using System.Reflection;
using ECampus.Domain.DataTransferObjects;
using ECampus.FrontEnd.Requests.Interfaces.Validation;
using ECampus.FrontEnd.Validation;
using FluentValidation;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Validation;

public class UserValidatorFactoryTests
{
    private readonly UserValidatorFactory _sut;

    private readonly IUpdateValidationRequests<UserDto> _updateValidationRequests =
        Substitute.For<IUpdateValidationRequests<UserDto>>();

    private readonly ICreateValidationRequests<UserDto> _createValidationRequests =
        Substitute.For<ICreateValidationRequests<UserDto>>();

    private readonly IValidator<UserDto> _baseValidator = Substitute.For<IValidator<UserDto>>();

    public UserValidatorFactoryTests()
    {
        _sut = new UserValidatorFactory(_baseValidator, _createValidationRequests, _updateValidationRequests);
    }

    [Fact]
    public void CreateValidator_ShouldReturnWithCreateValidationRequests()
    {
        var result = _sut.CreateValidator();
        var field = typeof(HttpCallingValidator<UserDto>).GetField("_validationRequests",
            BindingFlags.NonPublic | BindingFlags.Instance);

        field!.GetValue(result).Should().BeAssignableTo<ICreateValidationRequests<UserDto>>();
    }
    
    [Fact]
    public void UpdateValidator_ShouldReturnWithUpdateValidationRequests()
    {
        var result = _sut.UpdateValidator();
        var field = typeof(HttpCallingValidator<UserDto>).GetField("_validationRequests",
            BindingFlags.NonPublic | BindingFlags.Instance);

        field!.GetValue(result).Should().BeAssignableTo<IUpdateValidationRequests<UserDto>>();
    }
}