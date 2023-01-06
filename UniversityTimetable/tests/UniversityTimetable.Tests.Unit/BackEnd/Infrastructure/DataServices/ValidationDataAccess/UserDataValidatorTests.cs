using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.ValidationDataAccess;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.ValidationDataAccess;

public class UserDataValidatorTests
{
    private readonly UserDataValidator _sut;
    private readonly ApplicationDbContext _context;
    private readonly User _testModel = new() { Id = 10, Username = "username", Email = "email" };

    public UserDataValidatorTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new UserDataValidator(_context);
        _context.Users = new DbSetMock<User>(_testModel);
    }

    [Fact]
    public async Task LoadRequiredDataForCreateAsync_ShouldThrowExceptions_WhenObjectNotFound()
    {
        await new Func<Task>(() => _sut.LoadRequiredDataForCreateAsync(new User())).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    [Fact]
    public async Task LoadRequiredDataForUpdateAsync_ShouldThrowExceptions_WhenObjectNotFound()
    {
        await new Func<Task>(() => _sut.LoadRequiredDataForUpdateAsync(new User())).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    [Fact]
    public async Task LoadRequiredDataForUpdateAsync_ReturnsFromDb_WhenObjectExists()
    {
        (await _sut.LoadRequiredDataForUpdateAsync(new User { Id = 10 })).Should().Be(_testModel);
    }

    [Fact]
    public async Task LoadRequiredDataForCreateAsync_ReturnsFromDb_WhenObjectExists()
    {
        (await _sut.LoadRequiredDataForCreateAsync(new User { Id = 10 })).Should().Be(_testModel);
    }

    [Fact]
    public async Task ValidateCreate_ShouldReturnValidationErrors()
    {
        _context.Users = new DbSetMock<User>(_testModel,
            new User { Email = _testModel.Email, Username = _testModel.Username });

        var errors = await _sut.ValidateCreate(_testModel);

        errors.Should()
            .ContainEquivalentOf(KeyValuePair.Create(nameof(_testModel.Email), "This email is already user"));
        errors.Should()
            .ContainEquivalentOf(KeyValuePair.Create(nameof(_testModel.Username), "This username is already user"));
    }

    [Fact]
    public async Task ValidateUpdate_ShouldReturnValidationErrors()
    {
        _context.Users = new DbSetMock<User>(_testModel, new User { Username = _testModel.Username });

        var errors = await _sut.ValidateUpdate(_testModel);

        errors.Should()
            .ContainEquivalentOf(KeyValuePair.Create(nameof(_testModel.Username), "This username is already used"));
    }
}