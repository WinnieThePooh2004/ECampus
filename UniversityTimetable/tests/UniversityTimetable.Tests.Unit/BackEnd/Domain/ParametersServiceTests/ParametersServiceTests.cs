using AutoMapper;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.ParametersServiceTests;

public abstract class ParametersServiceTests<TDto, TParams, TModel>
    where TDto : class , IDataTransferObject, new()
    where TModel: class , IModel, new()
    where TParams : class, IQueryParameters<TModel>
{
    private readonly IAbstractFactory<TModel> _dataFactory;
    private readonly IBaseService<TDto> _baseService;
    private readonly IMapper _mapper;
    private readonly ParametersService<TDto, TParams, TModel> _service;
    private readonly IParametersDataAccessFacade<TModel, TParams> _dataAccessFacade;
    private readonly Fixture _fixture;

    protected ParametersServiceTests(IAbstractFactory<TModel> dataFactory)
    {
        _dataFactory = dataFactory;
        _mapper = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
        {
            new ClassProfile(),
            new AuditoryProfile(),
            new DepartmentProfile(),
            new GroupProfile(),
            new FacultyProfile(),
            new SubjectProfile(),
            new TeacherProfile(),
            new UserProfile()
        })).CreateMapper();

        _baseService = Substitute.For<IBaseService<TDto>>();
        _dataAccessFacade = Substitute.For<IParametersDataAccessFacade<TModel, TParams>>();
        _service = new ParametersService<TDto, TParams, TModel>(_dataAccessFacade, _mapper, _baseService);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    protected virtual async Task Delete_ShouldCallBaseService()
    {
        await _service.DeleteAsync(10);

        await _baseService.Received(1).DeleteAsync(10);
    }

    protected virtual async Task GetById_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<TDto>();
        _baseService.GetByIdAsync(10).Returns(item);

        (await _service.GetByIdAsync(10)).Should().Be(item);
        await _baseService.Received(1).GetByIdAsync(10);
    }
    
    protected virtual async Task Create_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<TDto>();
        _baseService.CreateAsync(item).Returns(item);

        (await _service.CreateAsync(item)).Should().Be(item);
        await _baseService.Received(1).CreateAsync(item);
    }
    
    protected virtual async Task Update_ReturnsFromBaseService_BaseServiceCalled()
    {
        var item = _fixture.Create<TDto>();
        _baseService.UpdateAsync(item).Returns(item);

        (await _service.UpdateAsync(item)).Should().Be(item);
        await _baseService.Received(1).UpdateAsync(item);
    }

    protected virtual async Task GetByParameters_ReturnsFromRepository()
    {
        var parameters = _fixture.Create<TParams>();
        var data = Enumerable.Range(0, 10).Select(_ => CreateModel()).ToList();
        var expected = new ListWithPaginationData<TModel> { Metadata = _fixture.Create<PaginationData>(), Data = data };
        _dataAccessFacade.GetByParameters(parameters).Returns(expected);
        
        var result = await _service.GetByParametersAsync(parameters);
        result.Metadata.Should().BeEquivalentTo(expected.Metadata);
        result.Data.Should().BeEquivalentTo(_mapper.Map<ListWithPaginationData<TDto>>(expected).Data);
    }

    private TModel CreateModel()
        => _dataFactory.CreateModel(_fixture);
}