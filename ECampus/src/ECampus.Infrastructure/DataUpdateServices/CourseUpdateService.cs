using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataUpdateServices;

public class CourseUpdateService : IDataUpdateService<Course>
{
    private readonly IDataUpdateService<Course> _baseUpdate;

    public CourseUpdateService(IDataUpdateService<Course> baseUpdate)
    {
        _baseUpdate = baseUpdate;
    }

    public async Task<Course> UpdateAsync(Course model, DbContext context)
    {
        var submissionsToDelete = await SubmissionsToDelete(model, context);
        var submissionsToCreate = await SubmissionsToCreate(model, context);
        context.RemoveRange(submissionsToDelete);
        context.AddRange(submissionsToCreate);
        return await _baseUpdate.UpdateAsync(model, context);
    }

    private static async Task<List<TaskSubmission>> SubmissionsToDelete(Course model, DbContext context)
    {
        if (model.Groups is null)
        {
            return new List<TaskSubmission>();
        }
        
        return await context.Set<TaskSubmission>()
            .FromSqlRaw($"""
                        SELECT TS.Id, TS.StudentId, TS.CourseTaskId, TS.TotalPoints, TS.IsMarked, TS.SubmissionContent
                        FROM Courses AS C
                        LEFT JOIN CourseTasks AS CT ON CT.CourseId = C.Id
                        LEFT JOIN CourseGroups AS CG ON CG.CourseId = C.Id
                        LEFT JOIN Groups AS G ON G.Id = CG.GroupId
                        LEFT JOIN Students AS S ON S.GroupId = G.Id
                        LEFT JOIN TaskSubmissions AS TS ON TS.CourseTaskId = CT.Id AND TS.StudentId = S.Id
                        WHERE G.Id NOT IN ({CurrentGroupIds(model)}) AND C.Id = {model.Id}
                        """).ToListAsync();
    }

    private static async Task<List<TaskSubmission>> SubmissionsToCreate(Course model, DbContext context)
    {
        if (model.Groups is null)
        {
            return new List<TaskSubmission>();
        }

        var addedStudents = await context.Set<Student>()
            .FromSqlRaw($"""
                        SELECT S.Id, S.FirstName, S.LastName, S.IsDeleted, S.GroupId, S.UserEmail
                        FROM Groups AS G
                        INNER JOIN Students AS S ON S.GroupId = G.Id
                        WHERE G.Id IN ({CurrentGroupIds(model)}) AND G.Id NOT IN(
                            SELECT CG.GroupId FROM Courses AS C
                            LEFT JOIN CourseGroups AS CG ON CG.CourseId = C.Id
                            WHERE C.Id = {model.Id}
                        )
                        """).ToListAsync();
        var allCourseTasks = await context.Set<CourseTask>().Where(task => task.CourseId == model.Id).ToListAsync();
        var result = new List<TaskSubmission>(addedStudents.Count * allCourseTasks.Count);

        foreach (var task in allCourseTasks)
        {
            result.AddRange(addedStudents.Select(student => new TaskSubmission{StudentId = student.Id, CourseTaskId = task.Id}));
        }

        return result;
    }
    
    private static string CurrentGroupIds(Course model)
    {
        var ids = model.Groups!.Select(g => g.Id.ToString()).ToList();
        if (ids.Count == 0)
        {
            ids.Add("0");
        }
        var currentGroupIds = string.Join(",", ids);
        if (currentGroupIds.Length == 0)
        {
            
        }
        return currentGroupIds;
    }
}