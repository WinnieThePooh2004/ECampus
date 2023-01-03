using AutoMapper;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class UserUpdateValidatorTests
{
    private readonly UserUpdateValidator _sut;
    private readonly IDataValidator<User> _dataValidator;
    private readonly IUpdateValidator<UserDto> _baseValidator;
    private readonly Fixture _fixture = new();

    public UserUpdateValidatorTests()
    {
        _dataValidator = Substitute.For<IDataValidator<User>>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()).CreateMapper();
        _baseValidator = Substitute.For<IUpdateValidator<UserDto>>();

        _sut = new UserUpdateValidator(_baseValidator, mapper, _dataValidator);
    }

    [Fact]
    public async Task Validate_ReturnsFromBaseValidatorAndDataValidator()
    {
        var baseErrors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        var dataErrors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        var user = new UserDto{ Email = "abc@example.com", Password = "Password" };
        var userFromDb = new User { Email = "", Password = "" };
        _dataValidator.LoadRequiredDataForUpdate(Arg.Any<User>()).Returns(userFromDb);
        _dataValidator.ValidateUpdate(Arg.Any<User>()).Returns(dataErrors);
        _baseValidator.ValidateAsync(user).Returns(baseErrors);

        var actualErrors = await _sut.ValidateAsync(user);

        actualErrors.Should().Contain(baseErrors);
        actualErrors.Should().Contain(dataErrors);
        actualErrors.Should().Contain(KeyValuePair.Create("Email", "You cannot change email"));
        actualErrors.Should().Contain(KeyValuePair.Create("Password", "To change password use action 'Users/ChangePassword'"));
    }
}