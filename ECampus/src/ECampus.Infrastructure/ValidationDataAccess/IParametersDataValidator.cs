using System.Security.Claims;
using ECampus.Core.Metadata;
using ECampus.Shared.Auth;
using ECampus.Shared.Interfaces.DataAccess.Validation;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(IParametersDataValidator<TaskSubmissionParameters>))]
public class TaskSubmissionParametersDataValidator : IParametersDataValidator<TaskSubmissionParameters>
{
    private readonly DbContext _context;
    private readonly ClaimsPrincipal _user;

    public TaskSubmissionParametersDataValidator(DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _user = httpContextAccessor.HttpContext.User;
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionParameters parameters)
    {
        var teacherIdClaim = _user.FindFirst(CustomClaimTypes.TeacherId);
        var teacherId = int.Parse(teacherIdClaim!.Value);
        if (!await _context.Set<Teacher>().Include(t => t.CourseTeachers).AnyAsync(t =>
                t.Id == teacherId && t.CourseTeachers!.Any(ct => ct.CourseId == parameters.CourseTaskId)))
        {
            return new ValidationResult(new ValidationError(nameof(parameters.CourseTaskId),
                "You are not teaching this course, so you can`t view its task"));
        }

        return new ValidationResult();
    }
}