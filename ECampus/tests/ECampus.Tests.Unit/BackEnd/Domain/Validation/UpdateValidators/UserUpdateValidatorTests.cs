using System.Security.Claims;
using AutoMapper;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Mapping;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class UserUpdateValidatorTests
{
    // private readonly UserUpdateValidator _sut;
    // private readonly IDataValidator<User> _dataValidator;
    // private readonly IUpdateValidator<UserDto> _baseValidator;
    // private readonly IValidationDataAccess<User> _validationDataAccess = Substitute.For<IValidationDataAccess<User>>();
    // private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    // private readonly Fixture _fixture = new();
    //
    // public UserUpdateValidatorTests()
    // {
    //     _dataValidator = Substitute.For<IDataValidator<User>>();
    //     var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()).CreateMapper();
    //     _baseValidator = Substitute.For<IUpdateValidator<UserDto>>();
    //
    //     _sut = new UserUpdateValidator(_baseValidator, mapper,
    //         _httpContextAccessor);
    // }
    //
    // [Fact]
    // public async Task Validate_ReturnsFromBaseValidatorAndDataValidator()
    // {
    //     var baseErrors = _fixture.CreateMany<ValidationError>(10).ToList();
    //     var dataErrors = _fixture.CreateMany<ValidationError>(10).ToList();
    //     var user = new UserDto { Email = "abc@example.com", Password = "Password" };
    //     var userFromDb = new User { Email = "", Password = "" };
    //     _validationDataAccess.LoadRequiredDataForUpdateAsync(Arg.Any<User>()).Returns(userFromDb);
    //     _dataValidator.ValidateUpdate(Arg.Any<User>()).Returns(new ValidationResult(dataErrors));
    //     _baseValidator.ValidateAsync(user).Returns(new ValidationResult(baseErrors));
    //
    //     var actualErrors = (await _sut.ValidateAsync(user)).ToList();
    //
    //     actualErrors.Should().Contain(baseErrors);
    //     actualErrors.Should().Contain(dataErrors);
    //     actualErrors.Should().Contain(new ValidationError("Email", "You cannot change email"));
    //     actualErrors.Should()
    //         .Contain(new ValidationError("Password", "To change password use action 'Users/ChangePassword'"));
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldAddError_WhenAdminTryToChangeHisRole()
    // {
    //     var user = new UserDto { Id = 10, Role = UserRole.Guest, Email = "email", Password = "password" };
    //     var userFromDb = MapperFactory.Mapper.Map<User>(user);
    //     userFromDb.Role = UserRole.Admin;
    //     _httpContextAccessor.HttpContext!.User.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
    //     _validationDataAccess.LoadRequiredDataForUpdateAsync(Arg.Any<User>()).Returns(userFromDb);
    //     _dataValidator.ValidateUpdate(Arg.Any<User>()).Returns(new ValidationResult());
    //     _baseValidator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult());
    //
    //     var result = await _sut.ValidateAsync(user);
    //
    //     result.ToList().Should()
    //         .Contain(new ValidationError(nameof(user.Role), "Admin cannon change role for him/herself"));
    // }
    //
    // [Fact]
    // public async Task Validate_ShouldAdd_WhenCurrentUserIsNotAdminAndRoleIsChanged()
    // {
    //     var user = new UserDto { Id = 10, Role = UserRole.Guest, Email = "email", Password = "password" };
    //     var userFromDb = MapperFactory.Mapper.Map<User>(user);
    //     userFromDb.Role = UserRole.Student;
    //     _httpContextAccessor.HttpContext!.User.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
    //     _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Role)
    //         .Returns(new Claim("", nameof(UserRole.Guest)));
    //     _validationDataAccess.LoadRequiredDataForUpdateAsync(Arg.Any<User>()).Returns(userFromDb);
    //     _dataValidator.ValidateUpdate(Arg.Any<User>()).Returns(new ValidationResult());
    //     _baseValidator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult());
    //
    //     var result = await _sut.ValidateAsync(user);
    //
    //     result.ToList().Should().Contain(new ValidationError(nameof(user.Role), "Only admins can change user`s role"));
    // }
}