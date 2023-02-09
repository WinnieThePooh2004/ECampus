using ECampus.DataAccess.Interfaces;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.DataAccess.DataUpdateServices;

public class CourseUpdateService : IDataUpdateService<Course>
{
    private readonly IDataUpdateService<Course> _baseUpdate;

    public CourseUpdateService(IDataUpdateService<Course> baseUpdate)
    {
        _baseUpdate = baseUpdate;
    }

    public async Task<Course> UpdateAsync(Course model, ApplicationDbContext context, CancellationToken token = default)
    {
        if (model.Groups is null)
        {
            return await _baseUpdate.UpdateAsync(model, context, token);
        }

        var submissionsToDelete = await SubmissionsToDelete(model, context, token);
        var submissionsToCreate = await SubmissionsToCreate(model, context, token);
        context.RemoveRange(submissionsToDelete);
        context.AddRange(submissionsToCreate);
        return await _baseUpdate.UpdateAsync(model, context, token);
    }

    private static async Task<List<TaskSubmission>> SubmissionsToDelete(Course model, ApplicationDbContext context,
        CancellationToken token = default)
    {
        var deleteQuery = DeleteQuery(model);
        return await context.TaskSubmissions
            .FromSqlRaw(deleteQuery).ToListAsync(token);
    }

    private static string DeleteQuery(Course model) =>
        $"""
        SELECT TS.Id, TS.StudentId, TS.CourseTaskId, TS.TotalPoints, TS.IsMarked, TS.SubmissionContent
        FROM Courses AS C
        LEFT JOIN CourseTasks AS CT ON CT.CourseId = C.Id
        LEFT JOIN CourseGroups AS CG ON CG.CourseId = C.Id
        LEFT JOIN Groups AS G ON G.Id = CG.GroupId
        LEFT JOIN Students AS S ON S.GroupId = G.Id
        LEFT JOIN TaskSubmissions AS TS ON TS.CourseTaskId = CT.Id AND TS.StudentId = S.Id
        WHERE G.Id NOT IN ({CurrentGroupIds(model)}) AND C.Id = {model.Id}
        """;

    private static async Task<List<TaskSubmission>> SubmissionsToCreate(Course model, ApplicationDbContext context,
        CancellationToken token = default)
    {
        var addedStudents = await context.Set<Student>()
            .FromSqlRaw(CreateQuery(model)).ToListAsync(token);
        var allCourseTasks = await context.CourseTasks.Where(task => task.CourseId == model.Id).ToListAsync(token);
        var result = new List<TaskSubmission>(addedStudents.Count * allCourseTasks.Count);

        foreach (var task in allCourseTasks)
        {
            result.AddRange(addedStudents.Select(student => new TaskSubmission
                { StudentId = student.Id, CourseTaskId = task.Id }));
        }

        return result;
    }

    private static string CreateQuery(Course model) =>
        $"""
        SELECT S.Id, S.FirstName, S.LastName, S.IsDeleted, S.GroupId, S.UserEmail
        FROM Groups AS G
        INNER JOIN Students AS S ON S.GroupId = G.Id
        WHERE G.Id IN ({CurrentGroupIds(model)}) AND G.Id NOT IN(
            SELECT CG.GroupId FROM Courses AS C
            LEFT JOIN CourseGroups AS CG ON CG.CourseId = C.Id
            WHERE C.Id = {model.Id}
        )
        """;

    private static string CurrentGroupIds(Course model)
    {
        var ids = model.Groups!.Select(g => g.Id.ToString()).ToList();
        if (ids.Count == 0)
        {
            ids.Add("0");
        }

        var currentGroupIds = string.Join(",", ids);
        return currentGroupIds;
    }
}