using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Models;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace UniversityTimetable.Domain.Validation.UniversalValidators;

public class ClassDtoUniversalValidator : IUpdateValidator<ClassDto>, ICreateValidator<ClassDto>
{
    private readonly IMapper _mapper;
    private readonly IValidationDataAccess<Class> _dataAccess;
    private readonly IUpdateValidator<ClassDto> _baseUpdateValidator;
    private readonly ICreateValidator<ClassDto> _baseCreateValidator;

    public ClassDtoUniversalValidator(IMapper mapper, IValidationDataAccess<Class> dataAccess,
        IUpdateValidator<ClassDto> baseUpdateValidator, ICreateValidator<ClassDto> baseCreateValidator)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _baseUpdateValidator = baseUpdateValidator;
        _baseCreateValidator = baseCreateValidator;
    }
    
    async Task<List<KeyValuePair<string, string>>> IUpdateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _baseUpdateValidator.ValidateAsync(dataTransferObject);
        if (baseErrors.Any())
        {
            return baseErrors;
        }

        return await ValidateAsync(dataTransferObject);
    }

    async Task<List<KeyValuePair<string, string>>> ICreateValidator<ClassDto>.ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _baseCreateValidator.ValidateAsync(dataTransferObject);
        if (baseErrors.Any())
        {
            return baseErrors;
        }

        return await ValidateAsync(dataTransferObject);    }

    private async Task<List<KeyValuePair<string, string>>> ValidateAsync(ClassDto dataTransferObject)
    {
        var model = await _dataAccess.LoadRequiredDataForCreateAsync(_mapper.Map<Class>(dataTransferObject));
        var errors = ValidateReferencedValues(model);
        if (errors.Any())
        {
            return errors;
        }

        errors.AddRange(ValidateSubject(model));
        errors.AddRange(ValidateTeacher(model));
        errors.AddRange(ValidateAuditory(model));
        errors.AddRange(ValidateGroup(model));
        return errors;
    }

    private static IEnumerable<KeyValuePair<string, string>> ValidateTeacher(Class @class)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (@class.Teacher.Classes
            .Any(c => c.Id != @class.Id &&
                      c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency != WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(KeyValuePair.Create(nameof(@class.TeacherId),
                $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }

    private static IEnumerable<KeyValuePair<string, string>> ValidateSubject(Class @class)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (@class.Teacher.SubjectIds.All(s => s.SubjectId != @class.SubjectId))
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(@class.SubjectId),
                $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                $"does not teach subject {@class.Subject.Name}"));
        }

        return errors;
    }

    private static IEnumerable<KeyValuePair<string, string>> ValidateAuditory(Class @class)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (@class.Auditory.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(@class.AuditoryId),
                $"Auditory {@class.Auditory.Name} in building {@class.Auditory.Building} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }

    private static List<KeyValuePair<string, string>> ValidateReferencedValues(Class @class)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (@class.Group is null)
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(@class.GroupId), "Group does not exist"));
        }

        if (@class.Auditory is null)
        {
            errors.Add(KeyValuePair.Create(nameof(@class.AuditoryId), "Auditory does not exist"));
        }

        if (@class.Subject is null)
        {
            errors.Add(KeyValuePair.Create(nameof(@class.SubjectId), "Subject does not exist"));
        }

        if (@class.Teacher is null)
        {
            errors.Add(KeyValuePair.Create(nameof(@class.TeacherId), "Teacher does not exist"));
        }

        return errors;
    }

    private static IEnumerable<KeyValuePair<string, string>> ValidateGroup(Class @class)
    {
        var errors = new List<KeyValuePair<string, string>>();
        if (@class.Group.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None
                       || @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(KeyValuePair.Create<string, string>(nameof(@class.GroupId),
                $"Group {@class.Group.Name} already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }
}