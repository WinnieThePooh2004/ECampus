using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class ParametersRepositoryTests
{
    private readonly ParametersDataAccessFacade<Auditory, AuditoryParameters> _dataAccessFacade;
    private readonly Fixture _fixture = new();
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IAbstractFactory<Auditory> _factory;

    public ParametersRepositoryTests()
    {
        _factory = new AuditoryFactory();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _dataAccessFacade =
            new ParametersDataAccessFacade<Auditory, AuditoryParameters>(_context,
                new MultipleAuditorySelector());
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromDb()
    {
        var data = _factory.CreateMany(_fixture, 10);
        var parameters = new AuditoryParameters { PageNumber = 0, PageSize = 5 };
        var set = new DbSetMock<Auditory>(data);
        _context.Auditories = set;

        var result = await _dataAccessFacade.GetByParameters(parameters);

        result.Data.Should().BeEquivalentTo(data.Take(parameters.PageSize).ToList(),
            opt => opt.ComparingByMembers<Auditory>());
    }
}