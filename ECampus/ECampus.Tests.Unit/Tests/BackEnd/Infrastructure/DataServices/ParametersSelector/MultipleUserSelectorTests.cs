using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleUserSelectorTests
{
    private readonly MultipleUserSelector _sut = new();
    private readonly List<User> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleUserSelectorTests()
    {
        _data = new List<User>
        {
            new() { Email = "abc", Username = "bcd" },
            new() { Email = "bde", Username = "abc" },
            new() { Email = "a", Username = "a" }
        };

        _context.Users = new DbSetMock<User>(_data);
    }

    [Fact]
    public async Task SelectData_ShouldReturnSuitedData()
    {
        var result = await _sut
            .SelectData(_context, new UserParameters { Username = "a", Email = "a", OrderBy = "Username" })
            .ToListAsync();

        result.Count.Should().Be(1);
        result.Should().Contain(_data[2]);
    }
}