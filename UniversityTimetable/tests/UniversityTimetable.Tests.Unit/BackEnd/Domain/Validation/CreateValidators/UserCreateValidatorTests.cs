using AutoMapper;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Validation.CreateValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.CreateValidators;

public class UserCreateValidatorTests
{
    private readonly UserCreateValidator _sut;
    private readonly IDataValidator<User> _dataValidator;
    private readonly ICreateValidator<UserDto> _baseValidator;
    private readonly Fixture _fixture = new();

    public UserCreateValidatorTests()
    {
        _dataValidator = Substitute.For<IDataValidator<User>>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()).CreateMapper();
        _baseValidator = Substitute.For<ICreateValidator<UserDto>>();

        _sut = new UserCreateValidator(mapper, _dataValidator, _baseValidator);
    }

    [Fact]
    public async Task Validate_ReturnsFromBaseValidatorAndDataValidator()
    {
        var baseErrors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var dataErrors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto();
        _dataValidator.ValidateCreate(Arg.Any<User>()).Returns(dataErrors);
        _baseValidator.ValidateAsync(user).Returns(baseErrors);

        var actualErrors = await _sut.ValidateAsync(user);

        actualErrors.GetAllErrors().Should().Contain(baseErrors.GetAllErrors());
        actualErrors.GetAllErrors().Should().Contain(dataErrors.GetAllErrors());
    }
}