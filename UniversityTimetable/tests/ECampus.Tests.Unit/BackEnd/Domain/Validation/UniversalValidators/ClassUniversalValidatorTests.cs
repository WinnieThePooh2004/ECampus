using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UniversalValidators;

public class ClassUniversalValidatorTests
{
    private readonly ClassDtoUniversalValidator _sut;
    private readonly IValidationDataAccess<Class> _dataValidator;
    private readonly IUpdateValidator<ClassDto> _baseUpdateValidator = Substitute.For<IUpdateValidator<ClassDto>>();
    private readonly Fixture _fixture = new();

    public ClassUniversalValidatorTests()
    {
        _dataValidator = Substitute.For<IValidationDataAccess<Class>>();
        _sut = new ClassDtoUniversalValidator(MapperFactory.Mapper, _dataValidator, _baseUpdateValidator);
    }

    [Fact]
    public async Task ValidateAsCreateValidator_ReturnsValidationFailures()
    {
        var @class = new ClassDto();
        var classFromDb = CreateTestModel();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);
        var expectedErrors = CreateExpectedErrors(classFromDb);

        var actual = await ((ICreateValidator<ClassDto>)_sut).ValidateAsync(@class);

        actual.GetAllErrors().Should().Contain(expectedErrors.GetAllErrors());
    }

    [Fact]
    public async Task ValidateAsCreateValidator_AddedMessages_WhenPropertiesIsnull()
    {
        var classFromDb = new Class();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);
        var expected = new ValidationResult
        (
            new ValidationError("GroupId", "Group does not exist"),
            new ValidationError("AuditoryId", "Auditory does not exist"),
            new ValidationError("SubjectId", "Subject does not exist"),
            new ValidationError("TeacherId", "Teacher does not exist")
        );

        var actual = await ((ICreateValidator<ClassDto>)_sut).ValidateAsync(new ClassDto());

        actual.GetAllErrors().Should().Contain(expected.GetAllErrors());
    }

    [Fact]
    public async Task ValidateAsCreateValidator_ShouldNotAddMoreErrors_WhenBaseValidatorFoundErrors()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        var classFromDb = CreateTestModel();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(errors);
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);

        var actualErrors = await ((ICreateValidator<ClassDto>)_sut).ValidateAsync(new ClassDto());

        actualErrors.GetAllErrors().Should().Contain(errors.GetAllErrors());
        actualErrors.GetAllErrors().Count().Should().Be(10);
    }

    [Fact]
    public async Task ValidateAsUpdateValidator_ReturnsValidationFailures()
    {
        var @class = new ClassDto();
        var classFromDb = CreateTestModel();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);
        var expectedErrors = CreateExpectedErrors(classFromDb);

        var actual = await ((IUpdateValidator<ClassDto>)_sut).ValidateAsync(@class);

        actual.GetAllErrors().Should().Contain(expectedErrors.GetAllErrors());
    }

    [Fact]
    public async Task ValidateAsUpdateValidator_AddedMessages_WhenPropertiesIsnull()
    {
        var classFromDb = new Class();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(new ValidationResult());
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);
        var expected = new List<ValidationError>
        {
            new("GroupId", "Group does not exist"),
            new("AuditoryId", "Auditory does not exist"),
            new("SubjectId", "Subject does not exist"),
            new("TeacherId", "Teacher does not exist")
        };

        var actual = await ((IUpdateValidator<ClassDto>)_sut).ValidateAsync(new ClassDto());

        actual.GetAllErrors().Should().Contain(expected);
    }

    [Fact]
    public async Task ValidateAsUpdateValidator_ShouldNotAddMoreErrors_WhenBaseValidatorFoundErrors()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        var classFromDb = CreateTestModel();
        _baseUpdateValidator.ValidateAsync(Arg.Any<ClassDto>()).Returns(errors);
        _dataValidator.LoadRequiredDataForCreateAsync(Arg.Any<Class>()).Returns(classFromDb);

        var actualErrors = await ((IUpdateValidator<ClassDto>)_sut).ValidateAsync(new ClassDto());

        actualErrors.GetAllErrors().Should().Contain(errors.GetAllErrors());
        actualErrors.GetAllErrors().Count().Should().Be(10);
    }

    private static ValidationResult CreateExpectedErrors(Class @class)
        => new(
            new ValidationError("GroupId",
                $"Group {@class.Group?.Name} already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            new ValidationError("AuditoryId",
                $"Auditory {@class.Auditory?.Name} in building {@class.Auditory?.Building} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            new ValidationError("TeacherId",
                $"Teacher {@class.Teacher?.FirstName} {@class.Teacher?.LastName} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"),
            new ValidationError("SubjectId",
                $"Teacher {@class.Teacher?.FirstName} {@class.Teacher?.LastName} " +
                $"does not teach subject {@class.Subject?.Name}")
        );

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
}