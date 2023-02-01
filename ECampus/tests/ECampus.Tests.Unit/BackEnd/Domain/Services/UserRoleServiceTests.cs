using AutoMapper;
using ECampus.Domain.Interfaces;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserRoleServiceTests
{
    private readonly UserRolesService _sut;
    private readonly IUserRolesRepository _repository = Substitute.For<IUserRolesRepository>();
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly IUpdateValidator<UserDto> _updateValidator = Substitute.For<IUpdateValidator<UserDto>>();
    private readonly ICreateValidator<UserDto> _createValidator = Substitute.For<ICreateValidator<UserDto>>();
    private readonly Fixture _fixture = new();

    public UserRoleServiceTests()
    {
        _sut = new UserRolesService(_repository, _mapper, _updateValidator, _createValidator);
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenValidationErrorOccured()
    {
        var user = new UserDto { Id = 10 };
        _updateValidator.ValidateAsync(Arg.Any<UserDto>())
            .Returns(new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList()));

        await new Func<Task>(() => _sut.UpdateAsync(user)).Should().ThrowAsync<ValidationException>();

        await _repository.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }
    
    [Fact]
    public async Task Create_ShouldThrowException_WhenValidationErrorOccured()
    {
        var user = new UserDto { Id = 10 };
        _createValidator.ValidateAsync(Arg.Any<UserDto>())
            .Returns(new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList()));

        await new Func<Task>(() => _sut.CreateAsync(user)).Should().ThrowAsync<ValidationException>();

        await _repository.DidNotReceive().CreateAsync(Arg.Any<User>());
    }
    
    [Fact]
    private async Task Create_ShouldReturnFromService_WhenNoValidationExceptions()
    {
        var item = TestModel;
        _createValidator.ValidateAsync(item).Returns(new ValidationResult());
        User? createdUser = null;
        _repository.CreateAsync(Arg.Do<User>(a => createdUser = a)).Returns(_mapper.Map<User>(item));

        var result = await _sut.CreateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(_mapper.Map<User>(item)),
            opt => opt.ComparingByMembers<UserDto>());
        createdUser.Should().BeEquivalentTo(_mapper.Map<User>(item),
            opt => opt.ComparingByMembers<User>());
        await _repository.Received(1).CreateAsync(Arg.Any<User>());
    }

    [Fact]
    private async Task Update_ReturnsFromService_WhenNoValidationExceptions()
    {
        var item = TestModel;
        _updateValidator.ValidateAsync(item).Returns(new ValidationResult());
        User? updatedUser = null;
        _repository.UpdateAsync(Arg.Do<User>(a => updatedUser = a)).Returns(_mapper.Map<User>(item));

        var result = await _sut.UpdateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(_mapper.Map<User>(item)),
            opt => opt.ComparingByMembers<UserDto>());
        updatedUser.Should().BeEquivalentTo(_mapper.Map<User>(item),
            opt => opt.ComparingByMembers<User>());
        await _repository.Received(1).UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    private async Task Delete_ShouldReturnFromService_WhenIdIsNotNull()
    {
        var item = TestModel;
        _repository.DeleteAsync(10).Returns(_mapper.Map<User>(item));

        var result = await _sut.DeleteAsync(10);
        
        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(_mapper.Map<User>(item)),
            opt => opt.ComparingByMembers<UserDto>());
        await _repository.Received(1).DeleteAsync(10);
    }

    [Fact]
    private async Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull()
    {
        var item = TestModel;
        _repository.GetByIdAsync(10).Returns(_mapper.Map<User>(item));

        var result = await _sut.GetByIdAsync(10);
        
        result.Should().BeEquivalentTo(_mapper.Map<UserDto>(_mapper.Map<User>(item)),
            opt => opt.ComparingByMembers<UserDto>());
        await _repository.Received(1).GetByIdAsync(10);
    }

    private UserDto TestModel => _fixture.Build<UserDto>()
        .Without(u => u.Teacher)
        .Without(u => u.Student)
        .Without(u => u.SavedAuditories)
        .Without(u => u.SavedGroups)
        .Without(u => u.SavedTeachers)
        .Create();
}