using AutoMapper;
using FluentValidation;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.CreateUpdateValidators;

public class ClassDtoCreateUpdateValidator : ICreateValidator<ClassDto>, IUpdateValidator<ClassDto>
{
    private readonly IMapper _mapper;
    private readonly IValidationRepository<Class> _repository;
    private readonly IValidator<ClassDto> _baseValidator;
    public ClassDtoCreateUpdateValidator(IMapper mapper, IValidationRepository<Class> repository, IValidator<ClassDto> baseValidator)
    {
        _mapper = mapper;
        _repository = repository;
        _baseValidator = baseValidator;
    }

    public async Task<Dictionary<string, string>> ValidateAsync(ClassDto dataTransferObject)
    {
        var baseErrors = await _baseValidator.ValidateAsync(dataTransferObject);
        if (baseErrors.Errors.Any())
        {
            return new Dictionary<string, string>(baseErrors.Errors.Select(error =>
                new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage)));
        }
        var model = await _repository.LoadRequiredDataForCreate(_mapper.Map<Class>(dataTransferObject));
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
    
    private Dictionary<string, string> ValidateTeacher(Class @class)
    {
        var errors = new Dictionary<string, string>();
        if (@class.Teacher.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(nameof(@class.AuditoryId), $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                                                  $"already has class number {@class.Number}" +
                                                  $" on {(DayOfWeek)@class.DayOfWeek}s " +
                                                  $"with week dependency {@class.WeekDependency}");
        }

        return errors;
    }

    private Dictionary<string, string> ValidateSubject(Class @class)
    {
        var errors = new Dictionary<string, string>();
        if (@class.Teacher.SubjectIds.All(s => s.SubjectId != @class.SubjectId))
        {
            errors.Add(nameof(@class.TeacherId), $"Teacher {@class.Teacher.FirstName} {@class.Teacher.LastName} " +
                                                 $"does not teach subject {@class.Subject.Name}");
        }

        return errors;
    }

    private Dictionary<string, string> ValidateAuditory(Class @class)
    {
        var errors = new Dictionary<string, string>();
        if (@class.Auditory.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None ||
                       @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(nameof(@class.AuditoryId),
                $"Auditory {@class.Auditory.Name} in building {@class.Auditory.Building} " +
                $"already has class number {@class.Number}" +
                $" on {(DayOfWeek)@class.DayOfWeek}s " +
                $"with week dependency {@class.WeekDependency}");
        }

        return errors;
    }

    private Dictionary<string, string> ValidateReferencedValues(Class @class)
    {
        var errors = new Dictionary<string, string>();
        if (@class.Group is null)
        {
            errors.Add(nameof(@class.GroupId), "Subject does not exist");
        }

        if (@class.Auditory is null)
        {
            errors.Add(nameof(@class.AuditoryId), "Subject does not exist");
        }

        if (@class.Subject is null)
        {
            errors.Add(nameof(@class.SubjectId), "Subject does not exist");
        }

        if (@class.Teacher is null)
        {
            errors.Add(nameof(@class.TeacherId), "Teacher does not exist");
        }

        return errors;
    }

    private Dictionary<string, string> ValidateGroup(Class @class)
    {
        var errors = new Dictionary<string, string>();
        if (@class.Group.Classes
            .Any(c => c.Number == @class.Number &&
                      c.DayOfWeek == @class.DayOfWeek &&
                      c.Id != @class.Id &&
                      (c.WeekDependency == WeekDependency.None
                       || @class.WeekDependency == WeekDependency.None ||
                       c.WeekDependency == @class.WeekDependency)))
        {
            errors.Add(nameof(@class.GroupId), $"Group {@class.Group.Name} already has class number {@class.Number}" +
                                               $" on {(DayOfWeek)@class.DayOfWeek}s " +
                                               $"with week dependency {@class.WeekDependency}");
        }

        return errors;
    }
}