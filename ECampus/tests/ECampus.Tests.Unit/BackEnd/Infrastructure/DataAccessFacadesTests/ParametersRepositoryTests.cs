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

public class ParametersRepositoryTests
{
    // private readonly ParametersDataAccessFacade<Auditory, AuditoryParameters> _dataAccessFacade;
    // private readonly Fixture _fixture = new();
    // private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    // private readonly IAbstractFactory<Auditory> _factory;
    //
    // private readonly IMultipleItemSelector<Auditory, AuditoryParameters> _selector =
    //     Substitute.For<IMultipleItemSelector<Auditory, AuditoryParameters>>();
    //
    // public ParametersRepositoryTests()
    // {
    //     _factory = new AuditoryFactory();
    //     _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    //     _dataAccessFacade = new ParametersDataAccessFacade<Auditory, AuditoryParameters>(_context, _selector);
    // }
    //
    // [Fact]
    // public async Task GetByParameters_ReturnsFromDb()
    // {
    //     var data = _factory.CreateMany(_fixture, 10).ToList();
    //     var parameters = new AuditoryParameters { PageNumber = 1, PageSize = 5 };
    //     var set = new DbSetMock<Auditory>(data).Object;
    //     _selector.SelectData(_context, parameters).Returns(set);
    //
    //     var result = await _dataAccessFacade.GetByParameters(parameters).ToListAsync();
    //
    //     result.Should().BeEquivalentTo(data);
    // }
}