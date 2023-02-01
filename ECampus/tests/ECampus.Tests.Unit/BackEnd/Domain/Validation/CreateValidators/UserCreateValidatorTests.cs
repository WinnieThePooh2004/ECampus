using System.Security.Claims;
using AutoMapper;
using ECampus.Contracts.DataValidation;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Mapping;
using ECampus.Domain.Validation.CreateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.CreateValidators;

public class UserCreateValidatorTests
{
    private readonly UserCreateValidator _sut;
    private readonly IDataValidator<User> _dataValidator;
    private readonly ICreateValidator<UserDto> _baseValidator;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly Fixture _fixture = new();

    public UserCreateValidatorTests()
    {
        _dataValidator = Substitute.For<IDataValidator<User>>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>()).CreateMapper();
        _baseValidator = Substitute.For<ICreateValidator<UserDto>>();

        _sut = new UserCreateValidator(mapper, _dataValidator, _baseValidator, _httpContextAccessor);
    }

    [Fact]
    public async Task Validate_ReturnsFromBaseValidatorAndDataValidator()
    {
        _httpContextAccessor.HttpContext.Returns(Substitute.For<HttpContext>());
        var baseErrors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var dataErrors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto { Role = UserRole.Guest };
        _dataValidator.ValidateCreate(Arg.Any<User>()).Returns(dataErrors);
        _baseValidator.ValidateAsync(user).Returns(baseErrors);

        var actualErrors = await _sut.ValidateAsync(user);

        actualErrors.ToList().Should().Contain(baseErrors.ToList());
        actualErrors.ToList().Should().Contain(dataErrors.ToList());
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext.Returns((HttpContext)null!);
        _baseValidator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult());
        await new Func<Task>(() => _sut.ValidateAsync(new UserDto())).Should()
            .ThrowAsync<HttpContextNotFoundExceptions>();
    }

    [Fact]
    public async Task Validate_ShouldAddMessage_WhenUserRoleIsNotGuestAndHttpContextUserIsNotAdmin()
    {
        _httpContextAccessor.HttpContext.Returns(Substitute.For<HttpContext>());
        _httpContextAccessor.HttpContext!.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Role, nameof(UserRole.Guest))
        })));
        var baseErrors = new ValidationResult();
        var dataErrors = new ValidationResult();
        var user = new UserDto { Role = UserRole.Admin };
        _dataValidator.ValidateCreate(Arg.Any<User>()).Returns(dataErrors);
        _baseValidator.ValidateAsync(user).Returns(baseErrors);

        var errors = await _sut.ValidateAsync(user);

        errors.ToList().Should().Contain(new ValidationError(nameof(User.Role),
            $"Only admin can create user with roles different from {nameof(UserRole.Guest)}"));
    }
}