using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.BaseServiceTests;

public abstract class BaseServiceTests<TDto, TModel>
    where TDto: class, IDataTransferObject, new()
    where TModel: class, IModel, new()
{
    private readonly BaseService<TDto, TModel> _service;
    private readonly IBaseRepository<TModel> _repository;
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;
    
    protected BaseServiceTests()
    {
        _repository = Substitute.For<IBaseRepository<TModel>>();
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
        _service = new BaseService<TDto, TModel>(_repository, Substitute.For<ILogger<BaseService<TDto, TModel>>>(), _mapper);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    protected virtual async Task Create_ReturnsFromService_ServiceCalled()
    {
        var item = _fixture.Create<TDto>();
        _repository.CreateAsync(Arg.Any<TModel>()).Returns(_mapper.Map<TModel>(item));

        var result = await _service.CreateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _repository.Received().CreateAsync(Arg.Any<TModel>());
    }

    protected virtual async Task Update_ReturnsFromService()
    {
        var item = _fixture.Create<TDto>();
        _repository.UpdateAsync(Arg.Any<TModel>()).Returns(_mapper.Map<TModel>(item));

        var result = await _service.UpdateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _repository.Received().UpdateAsync(Arg.Any<TModel>());
    }

    protected virtual async Task Delete_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.DeleteAsync(null)).Should().ThrowAsync<NullIdException>();
        
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }

    protected virtual async Task Delete_ShouldCallService_WhenIdIsNotNull()
    {
        await _service.DeleteAsync(10);

        await _repository.Received(1).DeleteAsync(10);
    }

    protected virtual async Task GetById_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.GetByIdAsync(null)).Should().ThrowAsync<NullIdException>();

        await _repository.DidNotReceive().GetByIdAsync(Arg.Any<int>());
    }

    protected virtual async Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull()
    {
        var item = _fixture.Create<TDto>();
        _repository.GetByIdAsync(10).Returns(_mapper.Map<TModel>(item));

        var result = await _service.GetByIdAsync(10);
        result.Should().BeEquivalentTo(_mapper.Map<TDto>(_mapper.Map<TModel>(item)),
            opt => opt.ComparingByMembers<TDto>());
        await _repository.Received(1).GetByIdAsync(10);
    }
}