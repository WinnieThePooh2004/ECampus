using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.Domain.Validation.UniversalValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.UniversalValidators;

public class ClassUniversalValidatorTests
{
    private readonly ClassDtoUniversalValidator _sut;
    private readonly IValidationDataAccess<Class> _dataValidator;
    private readonly IValidator<ClassDto> _baseValidator;
    private readonly Fixture _fixture = new();

    public ClassUniversalValidatorTests()
    {
        _dataValidator = Substitute.For<IDataValidator<Class>>();
        _baseValidator = Substitute.For<IValidator<ClassDto>>();
        _sut = new ClassDtoUniversalValidator(MapperFactory.Mapper, _dataValidator, _baseValidator);
    }

    [Fact]
    public async Task Validate_ReturnsValidationFailures()
    {
        var @class = new ClassDto();
        var classFromDb = CreateTestModel();
        _baseValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreate(Arg.Any<Class>()).Returns(classFromDb);
        var expectedErrors = CreateExpectedErrors(classFromDb);

        var actual = await _sut.ValidateAsync(@class);

        actual.Should().ContainsKeysWithValues(expectedErrors);
    }

    [Fact]
    public async Task Validate_AddedMessages_WhenPropertiesIsnull()
    {
        var classFromDb = new Class();
        _baseValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreate(Arg.Any<Class>()).Returns(classFromDb);
        var expected = new List<KeyValuePair<string, string>>
        {
            KeyValuePair.Create("GroupId", "Group does not exist"),
            KeyValuePair.Create("AuditoryId", "Auditory does not exist"),
            KeyValuePair.Create("SubjectId", "Subject does not exist"),
            KeyValuePair.Create("TeacherId", "Teacher does not exist")
        };

        var actual = await _sut.ValidateAsync(new ClassDto());

        actual.Should().ContainsKeysWithValues(expected);
    }

    private Class CreateTestModel()
    {
        var teacher = new TeacherFactory().CreateModel(_fixture);
        var subject = new SubjectFactory().CreateModel(_fixture);
        var group = new GroupFactory().CreateModel(_fixture);
        var auditory = new AuditoryFactory().CreateModel(_fixture);

        var @class = _fixture.Build<Class>()
            .With(c => c.Auditory, auditory)
            .With(c => c.AuditoryId, auditory.Id)
            .With(c => c.Group, group)
            .With(c => c.GroupId, group.Id)
            .With(c => c.Subject, subject)
            .With(c => c.SubjectId, subject.Id)
            .With(c => c.Teacher, teacher)
            .With(c => c.TeacherId, teacher.Id)
            .Create();

        var existingClass = new Class
        {
            Id = -1, Number = @class.Number, DayOfWeek = @class.DayOfWeek, WeekDependency = @class.WeekDependency
        };
        teacher.Classes = new List<Class> { existingClass };
        teacher.SubjectIds = new List<SubjectTeacher>();
        group.Classes = new List<Class> { existingClass };
        auditory.Classes = new List<Class> { existingClass };

        return @class;
    }

    [Fact]
    public async Task Validate_ShouldNotAddMoreErrors_WhenBaseValidatorFoundErrors()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        var classFromDb = CreateTestModel();
        _baseValidator.ValidateAsync(Arg.Any<ClassDto>())
            .Returns(new ValidationResult(errors
                .Select(e => new ValidationFailure(e.Key, e.Value))));
        _dataValidator.LoadRequiredDataForCreate(Arg.Any<Class>()).Returns(classFromDb);

        var actualErrors = await _sut.ValidateAsync(new ClassDto());

        actualErrors.Should().ContainsKeysWithValues(errors);
        actualErrors.Count.Should().Be(10);
    }

    private static IEnumerable<KeyValuePair<string, string>> CreateExpectedErrors(Class @class)
        => new List<KeyValuePair<string, string>>
        {
            KeyValuePair.Create<string, string>("GroupId",
                $"Group {@class.Group?.Name} already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            KeyValuePair.Create<string, string>("AuditoryId",
                $"Auditory {@class.Auditory?.Name} in building {@class.Auditory?.Building} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            KeyValuePair.Create<string, string>("TeacherId",
                $"Teacher {@class.Teacher?.FirstName} {@class.Teacher?.LastName} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            KeyValuePair.Create<string, string>("SubjectId",
                $"Teacher {@class.Teacher?.FirstName} {@class.Teacher?.LastName} " +
                $"does not teach subject {@class.Subject?.Name}")
        };
}