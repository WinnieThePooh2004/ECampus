using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Infrastructure.Interfaces;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class ParametersDataAccessManagerTests
{
    private readonly ParametersDataAccessManager _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    private readonly IMultipleItemSelector<Auditory, AuditoryParameters> _selector =
        Substitute.For<IMultipleItemSelector<Auditory, AuditoryParameters>>();

    public ParametersDataAccessManagerTests()
    {
        _sut = new ParametersDataAccessManager(_context, _serviceProvider);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromDb()
    {
        var data = Enumerable.Range(0, 10).Select(i => new Auditory { Id = 1 }).ToList();
        var parameters = new AuditoryParameters { PageNumber = 1, PageSize = 5 };
        var set = new DbSetMock<Auditory>(data).Object;
        _serviceProvider.GetService(typeof(IMultipleItemSelector<Auditory, AuditoryParameters>)).Returns(_selector);
        _selector.SelectData(_context, parameters).Returns(set);

        var result = await _sut.GetByParameters<Auditory, AuditoryParameters>(parameters).ToListAsync();

        result.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void GetByParameters_ShouldThrowException_WhenNoRegisteredSelectorFound()
    {
        new Action(() => _sut.GetByParameters<Group, GroupParameters>(new GroupParameters())).Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("There is not any registered services for type " +
                         $"{typeof(IMultipleItemSelector<Group, GroupParameters>)}");
    }
}