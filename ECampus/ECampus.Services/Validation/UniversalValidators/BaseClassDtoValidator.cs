using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;
using ECampus.Domain.Validation;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Services.Validation.UniversalValidators;

public class BaseClassDtoValidator
{
    private readonly IDataAccessFacade _parametersDataAccess;

    protected BaseClassDtoValidator(IDataAccessFacade parametersDataAccess)
    {
        _parametersDataAccess = parametersDataAccess;
    }

    protected async Task<ValidationResult> ValidateAsync(ClassDto @class)
    {
        var errors = new ValidationResult();
        var classesOnSameTime = _parametersDataAccess.GetByParameters<Class, ClassValidationDataParameters>(
            new ClassValidationDataParameters
            {
                Number = @class.Number, WeekDependency = @class.WeekDependency,
                DayOfWeek = @class.DayOfWeek
            });

        await ValidateTeacherId(@class, classesOnSameTime, errors);
        await ValidateAuditoryId(@class, classesOnSameTime, errors);
        await ValidateGroupId(@class, classesOnSameTime, errors);
        return errors;
    }

    private static async Task ValidateGroupId(ClassDto @class, IQueryable<Class> classesOnSameTime, ValidationResult errors)
    {
        if (await classesOnSameTime.AnyAsync(c => c.GroupId == @class.GroupId))
        {
            errors.AddError(new ValidationError(nameof(@class.GroupId),
                $"Group with id {@class.GroupId}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }
    }

    private static async Task ValidateAuditoryId(ClassDto @class, IQueryable<Class> classesOnSameTime, ValidationResult errors)
    {
        if (await classesOnSameTime.AnyAsync(c => c.AuditoryId == @class.AuditoryId))
        {
            errors.AddError(new ValidationError(nameof(@class.AuditoryId),
                $"Auditory with id = {@class.AuditoryId} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }
    }

    private static async Task ValidateTeacherId(ClassDto @class, IQueryable<Class> classesOnSameTime, ValidationResult errors)
    {
        if (await classesOnSameTime.AnyAsync(c => c.TeacherId == @class.TeacherId))
        {
            errors.AddError(new ValidationError(nameof(@class.TeacherId),
                $"Teacher with id = {@class.TeacherId}" +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}"));
        }
    }
}