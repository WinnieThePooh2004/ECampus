using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserRoleServiceTests
{
    private readonly UserRolesService _sut;
    private readonly IUserRolesRepository _repository = Substitute.For<IUserRolesRepository>();
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly Fixture _fixture = new();

    public UserRoleServiceTests()
    {
        _sut = new UserRolesService(_repository, _mapper);
    }

    [Fact]
    private async Task Create_ShouldReturnFromService_WhenNoValidationExceptions()
    {
        var item = TestModel;
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