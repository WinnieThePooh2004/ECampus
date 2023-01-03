using AutoMapper;
using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;
#pragma warning disable CS8602
#pragma warning disable CS8604

namespace UniversityTimetable.Domain.Validation.UniversalValidators;

public class ClassDtoUniversalValidator : ICreateValidator<ClassDto>, IUpdateValidator<ClassDto>
{
    private readonly IMapper _mapper;
    private readonly IValidationDataAccess<Class> _dataAccess;
    private readonly IValidator<ClassDto> _baseValidator;

    public ClassDtoUniversalValidator(IMapper mapper, IValidationDataAccess<Class> dataAccess,
        IValidator<ClassDto> baseValidator)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
        _baseValidator = baseValidator;
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _baseValidator.ValidateAsync(dataTransferObject);
        if (baseErrors.Errors.Any())
        {
            return new List<KeyValuePair<string, string>>(baseErrors.Errors.Select(error =>
                new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage)));
        }

        var model = await _dataAccess.LoadRequiredDataForCreate(_mapper.Map<Class>(dataTransferObject));
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