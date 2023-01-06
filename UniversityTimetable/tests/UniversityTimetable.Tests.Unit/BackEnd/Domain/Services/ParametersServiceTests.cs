using AutoMapper;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public sealed class ParametersServiceTests
{
    private readonly IAbstractFactory<Auditory> _dataFactory;
    private readonly IBaseService<AuditoryDto> _baseService;
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

        _baseService = Substitute.For<IBaseService<AuditoryDto>>();
        _dataAccessFacade = Substitute.For<IParametersDataAccessFacade<Auditory, AuditoryParameters>>();
        _service = new ParametersService<AuditoryDto, AuditoryParameters, Auditory>(_dataAccessFacade, _mapper, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Delete_ShouldCallBaseService()
    {
        await _service.DeleteAsync(10);

        await _baseService.Received(1).DeleteAsync(10);
    }

    [Fact]
    public async Task GetById_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<AuditoryDto>();
        _baseService.GetByIdAsync(10).Returns(item);

        (await _service.GetByIdAsync(10)).Should().Be(item);
        await _baseService.Received(1).GetByIdAsync(10);
    }
    
    [Fact]
    public async Task Create_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<AuditoryDto>();
        _baseService.CreateAsync(item).Returns(item);

        (await _service.CreateAsync(item)).Should().Be(item);
        await _baseService.Received(1).CreateAsync(item);
    }
    
    [Fact]
    public async Task Update_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<AuditoryDto>();
        _baseService.UpdateAsync(item).Returns(item);

        (await _service.UpdateAsync(item)).Should().Be(item);
        await _baseService.Received(1).UpdateAsync(item);
    }

    [Fact]
    public async Task GetByParameters_ReturnsFromRepository()
    {
        var parameters = _fixture.Create<AuditoryParameters>();
        var data = Enumerable.Range(0, 10).Select(_ => CreateModel()).ToList();
        var expected = new ListWithPaginationData<Auditory> { Metadata = _fixture.Create<PaginationData>(), Data = data };
        _dataAccessFacade.GetByParameters(parameters).Returns(expected);
        
        var result = await _service.GetByParametersAsync(parameters);
        result.Metadata.Should().BeEquivalentTo(expected.Metadata);
        result.Data.Should().BeEquivalentTo(_mapper.Map<ListWithPaginationData<AuditoryDto>>(expected).Data);
    }

    private Auditory CreateModel()
        => _dataFactory.CreateModel(_fixture);
}