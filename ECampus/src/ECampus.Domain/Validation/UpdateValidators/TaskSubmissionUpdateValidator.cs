using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation.UpdateValidators;

public class TaskSubmissionUpdateValidator : IUpdateValidator<TaskSubmissionDto>
{
    private readonly IUpdateValidator<TaskSubmissionDto> _updateValidator;
    private readonly IValidationDataAccess<TaskSubmission> _validationDataAccess;

    public TaskSubmissionUpdateValidator(IUpdateValidator<TaskSubmissionDto> updateValidator,
        IValidationDataAccess<TaskSubmission> validationDataAccess)
    {
        _updateValidator = updateValidator;
        _validationDataAccess = validationDataAccess;
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionDto dataTransferObject)
    {
        var errors = await _updateValidator.ValidateAsync(dataTransferObject);
        if (!errors.IsValid)
        {
            return errors;
        }

        var modelInDb = await _validationDataAccess.LoadRequiredDataForUpdateAsync(
            new TaskSubmission { Id = dataTransferObject.Id });

        if (modelInDb.StudentId != dataTransferObject.StudentId)
        {
            errors.AddError(new ValidationError(nameof(TaskSubmissionDto.StudentId), "Student id cannot be changed"));
        }
        
        if (modelInDb.CourseTaskId != dataTransferObject.CourseTaskId)
        {
            errors.AddError(new ValidationError(nameof(TaskSubmissionDto.CourseTaskId), "Course task id cannot be changed"));
        }

        return errors;
    }
}