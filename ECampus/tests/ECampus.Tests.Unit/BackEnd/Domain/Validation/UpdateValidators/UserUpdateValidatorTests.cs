using AutoMapper;
using ECampus.Domain.Mapping;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class UserUpdateValidatorTests
{
    private readonly UserUpdateValidator _sut;
    private readonly IDataValidator<User> _dataValidator;
    private readonly IUpdateValidator<UserDto> _baseValidator;
    private readonly IValidationDataAccess<User> _validationDataAccess = Substitute.For<IValidationDataAccess<User>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly Fixture _fixture = new();

    public UserUpdateValidatorTests()
    {
        _dataValidator = Substitute.For<IDataValidator<User>>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()).CreateMapper();
        _baseValidator = Substitute.For<IUpdateValidator<UserDto>>();

        _sut = new UserUpdateValidator(_baseValidator, mapper, _dataValidator, _validationDataAccess,
            _httpContextAccessor);
    }

    [Fact]
    public async Task Validate_ReturnsFromBaseValidatorAndDataValidator()
    {
        var baseErrors = _fixture.CreateMany<ValidationError>(10).ToList();
        var dataErrors = _fixture.CreateMany<ValidationError>(10).ToList();
        var user = new UserDto { Email = "abc@example.com", Password = "Password" };
        var userFromDb = new User { Email = "", Password = "" };
        _validationDataAccess.LoadRequiredDataForUpdateAsync(Arg.Any<User>()).Returns(userFromDb);
        _dataValidator.ValidateUpdate(Arg.Any<User>()).Returns(new ValidationResult(dataErrors));
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult(baseErrors));

        var actualErrors = (await _sut.ValidateAsync(user)).ToList().ToList();

        actualErrors.Should().Contain(baseErrors);
        actualErrors.Should().Contain(dataErrors);
        actualErrors.Should().Contain(new ValidationError("Email", "You cannot change email"));
        actualErrors.Should()
            .Contain(new ValidationError("Password", "To change password use action 'Users/ChangePassword'"));
    }
}