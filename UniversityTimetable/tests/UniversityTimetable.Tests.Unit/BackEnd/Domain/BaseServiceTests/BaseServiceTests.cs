using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.BaseServiceTests;

public abstract class BaseServiceTests<TDto, TModel>
    where TDto : class, IDataTransferObject, new()
    where TModel : class, IModel, new()
{
    private readonly BaseService<TDto, TModel> _service;
    private readonly IBaseDataAccessFacade<TModel> _dataAccessFacade;
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;
    private readonly IValidationFacade<TDto> _validation;
    protected BaseServiceTests()
    {
        _dataAccessFacade = Substitute.For<IBaseDataAccessFacade<TModel>>();
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
        _validation = Substitute.For<IValidationFacade<TDto>>();
        _service = new BaseService<TDto, TModel>(_dataAccessFacade,
            Substitute.For<ILogger<BaseService<TDto, TModel>>>(),
            _mapper, _validation);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    protected virtual async Task Create_ReturnsFromService_ServiceCalled_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<TDto>();
        _validation.ValidateCreate(item).Returns(new List<KeyValuePair<string, string>>());
        _dataAccessFacade.CreateAsync(Arg.Any<TModel>()).Returns(_mapper.Map<TModel>(item));

        var result = await _service.CreateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _dataAccessFacade.Received().CreateAsync(Arg.Any<TModel>());
    }

    protected virtual async Task Update_ReturnsFromService_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<TDto>();
        _validation.ValidateUpdate(item).Returns(new List<KeyValuePair<string, string>>());
        _dataAccessFacade.UpdateAsync(Arg.Any<TModel>()).Returns(_mapper.Map<TModel>(item));

        var result = await _service.UpdateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _dataAccessFacade.Received().UpdateAsync(Arg.Any<TModel>());
    }

    protected virtual async Task Update_ThrowsValidationException_WhenValidationErrorOccured()
    {
        _validation.ValidateUpdate(Arg.Any<TDto>()).Returns(new List<KeyValuePair<string, string>>
            { KeyValuePair.Create<string, string>("", "") });
        await new Func<Task>(() => _service.UpdateAsync(new TDto())).Should().ThrowAsync<ValidationException>();
    }

    protected virtual async Task Create_ThrowsValidationException_WhenValidationErrorOccured()
    {
        _validation.ValidateUpdate(Arg.Any<TDto>()).Returns(new List<KeyValuePair<string, string>>
            { KeyValuePair.Create<string, string>("", "") });
        await new Func<Task>(() => _service.UpdateAsync(new TDto())).Should().ThrowAsync<ValidationException>();
    }
    
    protected virtual async Task Delete_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.DeleteAsync(null)).Should().ThrowAsync<NullIdException>();

        await _dataAccessFacade.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }

    protected virtual async Task Delete_ShouldCallService_WhenIdIsNotNull()
    {
        await _service.DeleteAsync(10);

        await _dataAccessFacade.Received(1).DeleteAsync(10);
    }

    protected virtual async Task GetById_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.GetByIdAsync(null)).Should().ThrowAsync<NullIdException>();

        await _dataAccessFacade.DidNotReceive().GetByIdAsync(Arg.Any<int>());
    }

    protected virtual async Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull()
    {
        var item = _fixture.Create<TDto>();
        _dataAccessFacade.GetByIdAsync(10).Returns(_mapper.Map<TModel>(item));

        var result = await _service.GetByIdAsync(10);
        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _dataAccessFacade.Received(1).GetByIdAsync(10);
    }
}