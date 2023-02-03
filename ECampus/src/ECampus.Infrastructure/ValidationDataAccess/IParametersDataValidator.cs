using System.Security.Claims;
using ECampus.Contracts.DataValidation;
using ECampus.Core.Metadata;
using ECampus.Shared.Auth;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.ValidationDataAccess;

[Inject(typeof(IParametersDataValidator<TaskSubmissionParameters>))]
public class TaskSubmissionParametersDataValidator : IParametersDataValidator<TaskSubmissionParameters>
{
    private readonly ApplicationDbContext _context;
    private readonly ClaimsPrincipal _user;

    public TaskSubmissionParametersDataValidator(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _user = httpContextAccessor.HttpContext.User;
    }

    public async Task<ValidationResult> ValidateAsync(TaskSubmissionParameters parameters)
    {
        var teacherIdClaim = _user.FindFirst(CustomClaimTypes.TeacherId);
        var teacherId = int.Parse(teacherIdClaim!.Value);
        var teacherTeachesCourse = await _context.Teachers
            .Include(teacher => teacher.Courses)!
            .ThenInclude(course => course.Tasks!.Where(task => task.Id == parameters.CourseTaskId))
            .AnyAsync(teacher => teacher.Id == teacherId && teacher.Courses!.Any(course => course.Tasks!.Any()));
        
        if (!teacherTeachesCourse)
        {
            return new ValidationResult(new ValidationError(nameof(parameters.CourseTaskId),
                "You are not teaching this course, so you can`t view its task"));
        }

        return new ValidationResult();
    }
}