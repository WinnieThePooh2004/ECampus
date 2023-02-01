using AutoMapper;
using ECampus.Contracts.DataValidation;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace ECampus.Domain.Validation.UniversalValidators;

public class BaseClassDtoValidator
{
    private readonly IMapper _mapper;
    private readonly IValidationDataAccess<Class> _dataAccess;

    protected BaseClassDtoValidator(IMapper mapper, IValidationDataAccess<Class> dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    protected async Task<ValidationResult> ValidateAsync(ClassDto dataTransferObject)
    {
        var model = await _dataAccess.LoadRequiredDataForCreateAsync(_mapper.Map<Class>(dataTransferObject));
        var errors = ValidateReferencedValues(model);
        if (!errors.IsValid)
        {
            return errors;
        }

        errors.MergeResults(ValidateSubject(model));
        errors.MergeResults(ValidateTeacher(model));
        errors.MergeResults(ValidateAuditory(model));
        errors.MergeResults(ValidateGroup(model));
        return errors;
    }

    private static ValidationResult ValidateTeacher(Class @class)
    {
        var errors = new ValidationResult();
        if (@class.Teacher.Classes
            .Any(c => c.Id != @class.Id &&
                      c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency != WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.AddError(new ValidationError(nameof(@class.TeacherId),
                $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }

    private static ValidationResult ValidateSubject(Class @class)
    {
        var errors = new ValidationResult();
        if (@class.Teacher.SubjectIds.All(s => s.SubjectId != @class.SubjectId))
        {
            errors.AddError(new ValidationError(nameof(@class.SubjectId),
                $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                $"does not teach subject {@class.Subject.Name}"));
        }

        return errors;
    }

    private static ValidationResult ValidateAuditory(Class @class)
    {
        var errors = new ValidationResult();
        if (@class.Auditory.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.AddError(new ValidationError(nameof(@class.AuditoryId),
                $"Auditory {@class.Auditory.Name} in building {@class.Auditory.Building} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }

    private static ValidationResult ValidateReferencedValues(Class @class)
    {
        var errors = new ValidationResult();
        if (@class.Group is null)
        {
            errors.AddError(new ValidationError(nameof(@class.GroupId), "Group does not exist"));
        }

        if (@class.Auditory is null)
        {
            errors.AddError(new ValidationError(nameof(@class.AuditoryId), "Auditory does not exist"));
        }

        if (@class.Subject is null)
        {
            errors.AddError(new ValidationError(nameof(@class.SubjectId), "Subject does not exist"));
        }

        if (@class.Teacher is null)
        {
            errors.AddError(new ValidationError(nameof(@class.TeacherId), "Teacher does not exist"));
        }

        return errors;
    }

    private static ValidationResult ValidateGroup(Class @class)
    {
        var errors = new ValidationResult();
        if (@class.Group.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None
                       || @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.AddError(new ValidationError(nameof(@class.GroupId),
                $"Group {@class.Group.Name} already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }

        return errors;
    }
}