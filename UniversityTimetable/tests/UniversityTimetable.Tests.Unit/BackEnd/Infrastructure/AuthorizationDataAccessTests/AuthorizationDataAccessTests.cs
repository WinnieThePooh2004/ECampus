using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Auth;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.AuthorizationDataAccessTests;

public class AuthorizationDataAccessTests
{
    private readonly AuthorizationDataAccess _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public AuthorizationDataAccessTests()
    {
        _sut = new AuthorizationDataAccess(_context);
    }

    [Fact]
    public async Task GetByEmail_ReturnsFromSet_IdObjectExists()
    {
        var expectedUser = new User{ Email = "abc@example.com"};
        var data = new List<User> { expectedUser };
        var set = new DbSetMock<User>(data).Object;
        _context.Users = set;

        var actualUser = await _sut.GetByEmailAsync("abc@example.com");

        actualUser.Should().Be(expectedUser);
    }

    [Fact]
    public async Task GetByEmail_ShouldThrowException_WhenQueryReturnsNull()
    {
        _context.Users = new DbSetMock<User>(new List<User>()).Object;

        await new Func<Task>(() => _sut.GetByEmailAsync("10")).Should().ThrowAsync<InfrastructureExceptions>()
            .WithMessage($"User with email 10 does not exist\nError code: 404");
    }
}