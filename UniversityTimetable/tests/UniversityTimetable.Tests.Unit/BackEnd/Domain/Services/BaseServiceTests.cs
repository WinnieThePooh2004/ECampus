using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public sealed class BaseServiceTests
{
    private readonly BaseService<AuditoryDto, Auditory> _service;
    private readonly IBaseDataAccessFacade<Auditory> _dataAccessFacade;
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;
    private readonly IValidationFacade<AuditoryDto> _validation;

    public BaseServiceTests()
    {
        _dataAccessFacade = Substitute.For<IBaseDataAccessFacade<Auditory>>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfiles(new List<Profile>
        {
            new AuditoryProfile(),
        })).CreateMapper();
        _validation = Substitute.For<IValidationFacade<AuditoryDto>>();
        _service = new BaseService<AuditoryDto, Auditory>(_dataAccessFacade,
            Substitute.For<ILogger<BaseService<AuditoryDto, Auditory>>>(),
            _mapper, _validation);
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    private async Task Create_ReturnsFromService_ServiceCalled_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<AuditoryDto>();
        Auditory? createdAuditory = null;
        _validation.ValidateCreate(item).Returns(new List<KeyValuePair<string, string>>());
        _dataAccessFacade.CreateAsync(Arg.Do<Auditory>(a => createdAuditory = a)).Returns(_mapper.Map<Auditory>(item));

        var result = await _service.CreateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<AuditoryDto>(_mapper.Map<Auditory>(item)),
            opt => opt.ComparingByMembers<AuditoryDto>());
        createdAuditory.Should().BeEquivalentTo(_mapper.Map<Auditory>(item),
            opt => opt.ComparingByMembers<Auditory>());
        await _dataAccessFacade.Received(1).CreateAsync(Arg.Any<Auditory>());
    }

    [Fact]
    private async Task Update_ReturnsFromService_WhenNoValidationExceptions()
    {
        var item = _fixture.Create<AuditoryDto>();
        Auditory? updatedAuditory = null;
        _validation.ValidateUpdate(item).Returns(new List<KeyValuePair<string, string>>());
        _dataAccessFacade.UpdateAsync(Arg.Do<Auditory>(a => updatedAuditory = a)).Returns(_mapper.Map<Auditory>(item));

        var result = await _service.UpdateAsync(item);

        result.Should().BeEquivalentTo(_mapper.Map<AuditoryDto>(_mapper.Map<Auditory>(item)),
            opt => opt.ComparingByMembers<AuditoryDto>());
        updatedAuditory.Should().BeEquivalentTo(_mapper.Map<Auditory>(item),
            opt => opt.ComparingByMembers<Auditory>());
        await _dataAccessFacade.Received(1).UpdateAsync(Arg.Any<Auditory>());
    }

    [Fact]
    private async Task Update_ThrowsValidationException_WhenValidationErrorOccured()
    {
        var errors = new List<KeyValuePair<string, string>> { KeyValuePair.Create<string, string>("", "") };
        _validation.ValidateUpdate(Arg.Any<AuditoryDto>()).Returns(errors);

        await new Func<Task>(() => _service.UpdateAsync(new AuditoryDto())).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(AuditoryDto), errors).Message);

        await _dataAccessFacade.DidNotReceive().UpdateAsync(Arg.Any<Auditory>());
    }

    [Fact]
    private async Task Create_ThrowsValidationException_WhenValidationErrorOccured()
    {
        var errors = new List<KeyValuePair<string, string>> { KeyValuePair.Create<string, string>("", "") };
        _validation.ValidateUpdate(Arg.Any<AuditoryDto>()).Returns(errors);

        await new Func<Task>(() => _service.UpdateAsync(new AuditoryDto())).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(AuditoryDto), errors).Message);

        await _dataAccessFacade.DidNotReceive().UpdateAsync(Arg.Any<Auditory>());
    }

    [Fact]
    private async Task Delete_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.DeleteAsync(null)).Should().ThrowAsync<NullIdException>();

        await _dataAccessFacade.DidNotReceive().DeleteAsync(Arg.Any<int>());
    }

    [Fact]
    private async Task Delete_ShouldCallService_WhenIdIsNotNull()
    {
        await _service.DeleteAsync(10);

        await _dataAccessFacade.Received(1).DeleteAsync(10);
    }

    [Fact]
    private async Task GetById_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _service.GetByIdAsync(null)).Should().ThrowAsync<NullIdException>();

        await _dataAccessFacade.DidNotReceive().GetByIdAsync(Arg.Any<int>());
    }

    [Fact]
    private async Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull()
    {
        var item = _fixture.Create<AuditoryDto>();
        _dataAccessFacade.GetByIdAsync(10).Returns(_mapper.Map<Auditory>(item));

        var result = await _service.GetByIdAsync(10);
        result.Should().BeEquivalentTo(_mapper.Map<AuditoryDto>(_mapper.Map<Auditory>(item)),
            opt => opt.ComparingByMembers<AuditoryDto>());
        await _dataAccessFacade.Received(1).GetByIdAsync(10);
    }
}