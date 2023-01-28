using AutoMapper;
using ECampus.Domain.Mapping;
using ECampus.Domain.Services;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Extensions;

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
        _mapper = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
        {
            new AuditoryProfile(),
        })).CreateMapper();

        Substitute.For<IBaseService<AuditoryDto>>();
        _dataAccessFacade = Substitute.For<IParametersDataAccessFacade<Auditory, AuditoryParameters>>();
        _service = new ParametersService<AuditoryDto, AuditoryParameters, Auditory>(_dataAccessFacade, _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    
    [Fact]
    public async Task GetByParameters_ReturnsFromRepository()
    {
        var parameters = _fixture.Create<AuditoryParameters>();
        var data = _dataFactory.CreateMany(_fixture, 10);
        var expected = new ListWithPaginationData<Auditory> { Metadata = _fixture.Create<PaginationData>(), Data = data };
        _dataAccessFacade.GetByParameters(parameters).Returns(expected);
        
        var result = await _service.GetByParametersAsync(parameters);
        
        result.Metadata.Should().BeEquivalentTo(expected.Metadata);
        result.Data.Should().BeEquivalentTo(_mapper.Map<ListWithPaginationData<AuditoryDto>>(expected).Data);
    }
}