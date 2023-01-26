using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Shared.Interfaces.Domain.Validation;

public interface ITaskSubmissionDataValidator
{
    /// <summary>
    /// this method verify if current users teacher id is id of teacher that teach course to owner of submission
    /// </summary>
    /// <param name="teacherId">pass here value of claim with name TeacherId</param>
    /// <param name="taskSubmissionId"></param>
    /// <returns></returns>
    Task<ValidationResult> ValidateTeacherId(int teacherId, int taskSubmissionId);

    Task<TaskSubmission> LoadSubmissionData(int taskSubmissionId);
}