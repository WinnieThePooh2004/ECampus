using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class ParametersRepositoryTests
{
    private readonly IBaseDataAccessFacade<Auditory> _baseDataAccessFacade =
        Substitute.For<IBaseDataAccessFacade<Auditory>>();

    private readonly ParametersDataAccessFacade<Auditory, AuditoryParameters> _dataAccessFacade;
    private readonly Fixture _fixture = new();
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();
    private readonly IAbstractFactory<Auditory> _factory;

    public ParametersRepositoryTests()
    {
        _factory = new AuditoryFactory();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _dataAccessFacade =
            new ParametersDataAccessFacade<Auditory, AuditoryParameters>(_context, _baseDataAccessFacade,
                new MultipleAuditorySelector());
    }

    [Fact]
    public async Task Create_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.CreateAsync(item).Returns(item);

        var result = await _dataAccessFacade.CreateAsync(item);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).CreateAsync(item);
    }

    [Fact]
    public async Task Update_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.UpdateAsync(item).Returns(item);

        var result = await _dataAccessFacade.UpdateAsync(item);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).UpdateAsync(item);
    }

    [Fact]
    public async Task Delete_ShouldCallBaseService()
    {
        await _dataAccessFacade.DeleteAsync(10);

        await _baseDataAccessFacade.Received(1).DeleteAsync(10);
    }
    
    [Fact]
    public async Task GetById_ReturnsFromBaseRepository_BaseRepositoryCalled()
    {
        var item = _factory.CreateModel(_fixture);
        _baseDataAccessFacade.GetByIdAsync(10).Returns(item);

        var result = await _dataAccessFacade.GetByIdAsync(10);

        result.Should().Be(item);
        await _baseDataAccessFacade.Received(1).GetByIdAsync(10);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromDb()
    {
        var data = _factory.CreateMany(_fixture, 10);
        var parameters = new AuditoryParameters { PageNumber = 0, PageSize = 5 };
        var set = new DbSetMock<Auditory>(data);
        _context.Set<Auditory>().Returns(set.Object);

        var result = await _dataAccessFacade.GetByParameters(parameters);

        result.Data.Should().BeEquivalentTo(data.OrderBy(item => item.Id).Take(parameters.PageSize).ToList(),
            opt => opt.ComparingByMembers<Auditory>());
    }
}