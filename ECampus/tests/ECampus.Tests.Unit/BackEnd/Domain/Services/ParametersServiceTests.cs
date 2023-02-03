using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Domain.Mapping;
using ECampus.Services.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public sealed class ParametersServiceTests
{
    private readonly IAbstractFactory<Auditory> _dataFactory;
    private readonly IMapper _mapper;
    private readonly ParametersService<AuditoryDto, AuditoryParameters, Auditory> _service;
    private readonly IParametersDataAccessFacade<Auditory, AuditoryParameters> _dataAccessFacade;
    private readonly Fixture _fixture;

    public ParametersServiceTests()
    {
        _dataFactory = new AuditoryFactory();
        _mapper = MapperFactory.Mapper;

        Substitute.For<IBaseService<AuditoryDto>>();
        _dataAccessFacade = Substitute.For<IParametersDataAccessFacade<Auditory, AuditoryParameters>>();
        _service = new ParametersService<AuditoryDto, AuditoryParameters, Auditory>(_dataAccessFacade, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromRepository()
    {
        var parameters = new AuditoryParameters { PageSize = 10, PageNumber = 1 };
        var data = _dataFactory.CreateMany(_fixture, 10);
        var expected = new DbSetMock<Auditory>(data).Object;
        _dataAccessFacade.GetByParameters(parameters).Returns(expected);

        var result = await _service.GetByParametersAsync(parameters);

        result.Metadata.Should().BeEquivalentTo(new PaginationData { TotalCount = 10, PageNumber = 1, PageSize = 10 });
        result.Data.Select(a => a.Id).Should().BeEquivalentTo(data.Select(d => d.Id));
    }
}