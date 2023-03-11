using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class UserServiceTests
{
    private readonly UserProfileService _sut;
    private readonly Fixture _fixture = new();
    private readonly ICreateValidator<UserDto> _createValidator = Substitute.For<ICreateValidator<UserDto>>();
    private readonly IUpdateValidator<UserDto> _updateValidator = Substitute.For<IUpdateValidator<UserDto>>();

    public UserServiceTests()
    {
        Substitute.For<IBaseService<UserDto>>();
        _sut = new UserProfileService(_updateValidator, _createValidator, Substitute.For<IMapper>(),
            Substitute.For<IDataAccessFacade>());
    }

    [Fact]
    private async Task ValidateCreate_ShouldReturnFromValidationFacade()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto();
        _createValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateCreateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _createValidator.Received(1).ValidateAsync(user);
    }

    [Fact]
    private async Task ValidateUpdate_ShouldReturnFromValidationFacade()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto();
        _updateValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateUpdateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _updateValidator.Received(1).ValidateAsync(user);
    }
}