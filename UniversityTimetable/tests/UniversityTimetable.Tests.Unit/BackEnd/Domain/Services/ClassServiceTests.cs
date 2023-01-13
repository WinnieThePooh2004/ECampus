using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public class ClassServiceTests
{
    private readonly ClassService _sut;
    private readonly ITimetableDataAccessFacade _dataAccess = Substitute.For<ITimetableDataAccessFacade>();
    private readonly IUpdateValidator<ClassDto> _validator = Substitute.For<IUpdateValidator<ClassDto>>();
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper = MapperFactory.Mapper;

    public ClassServiceTests()
    {
        var logger = Substitute.For<ILogger<ClassService>>();
        _sut = new ClassService(logger, _dataAccess, _mapper, _validator);
    }

    [Fact]
    public async Task Validate_ReturnsFromValidationFacade()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        var @class = CreateClass();
        _validator.ValidateAsync(@class).Returns(errors);

        var result = await _sut.ValidateAsync(@class);

        result.Should().BeEquivalentTo(errors);
        await _validator.Received(1).ValidateAsync(@class);
    }

    [Fact]
    private async Task GetTimetableForAuditory_ReturnsMappedFromRepository()
    {
        var data = new TimetableData
        {
            Classes = new ClassFactory().CreateMany(_fixture, 10),
            Auditory = new Auditory(),
            Group = new Group(),
            Teacher = new Teacher()
        };
        _dataAccess.GetTimetableForAuditoryAsync(10).Returns(data);
        var expected = new Timetable(_mapper.Map<List<ClassDto>>(data.Classes))
        {
            Auditory = new AuditoryDto()
        };

        var result = await _sut.GetTimetableForAuditoryAsync(10);

        result.Should().BeEquivalentTo(expected, opt => opt.ComparingByMembers<Timetable>());
    }

    [Fact]
    private async Task GetTimetableForGroup_ReturnsMappedFromRepository()
    {
        var data = new TimetableData
        {
            Classes = new ClassFactory().CreateMany(_fixture, 10),
            Auditory = new Auditory(),
            Group = new Group(),
            Teacher = new Teacher()
        };
        _dataAccess.GetTimetableForGroupAsync(10).Returns(data);
        var expected = new Timetable(_mapper.Map<List<ClassDto>>(data.Classes))
        {
            Group = new GroupDto()
        };

        var result = await _sut.GetTimetableForGroupAsync(10);

        result.Should().BeEquivalentTo(expected, opt => opt.ComparingByMembers<Timetable>());
    }

    [Fact]
    private async Task GetTimetableForTeacher_ReturnsMappedFromRepository()
    {
        var data = new TimetableData
        {
            Classes = new ClassFactory().CreateMany(_fixture, 10),
            Auditory = new Auditory(),
            Group = new Group(),
            Teacher = new TeacherFactory().CreateModel(_fixture)
        };
        _dataAccess.GetTimetableForTeacherAsync(10).Returns(data);
        var expected = new Timetable(_mapper.Map<List<ClassDto>>(data.Classes))
        {
            Teacher = _mapper.Map<TeacherDto>(data.Teacher)
        };

        var result = await _sut.GetTimetableForTeacherAsync(10);

        result.Should().BeEquivalentTo(expected, opt => opt.ComparingByMembers<Timetable>());
    }

    [Fact]
    public async Task GetTimetableForAuditory_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _sut.GetTimetableForAuditoryAsync(null)).Should()
            .ThrowAsync<NullIdException>()
            .WithMessage(new NullIdException().Message);
    }
    
    [Fact]
    public async Task GetTimetableForGroup_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _sut.GetTimetableForGroupAsync(null)).Should()
            .ThrowAsync<NullIdException>()
            .WithMessage(new NullIdException().Message);
    }
    
    [Fact]
    public async Task GetTimetableForTeacher_ShouldThrowException_WhenIdIsNull()
    {
        await new Func<Task>(() => _sut.GetTimetableForTeacherAsync(null)).Should()
            .ThrowAsync<NullIdException>()
            .WithMessage(new NullIdException().Message);
    }

    private ClassDto CreateClass()
    {
        var rand = new Random();
        return _fixture.Build<ClassDto>()
            .Without(c => c.Auditory)
            .Without(c => c.Group)
            .Without(c => c.Subject)
            .Without(c => c.Teacher)
            .With(c => c.Number, rand.Next(0, 5))
            .With(c => c.DayOfWeek, rand.Next(0, 6))
            .Create();
    }
}