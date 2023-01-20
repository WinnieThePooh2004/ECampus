using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.DataUpdateServices;

public class UserUpdateService : IDataUpdateService<User>
{
    private readonly IDataUpdateService<User> _baseUpdate;

    public UserUpdateService(IDataUpdateService<User> baseUpdate)
    {
        _baseUpdate = baseUpdate;
    }

    public Task<User> UpdateAsync(User model, DbContext context) =>
        model.Role switch
        {
            UserRole.Student => UpdateAsStudent(model, context),
            UserRole.Teacher => UpdateAsTeacher(model, context),
            _ => UpdateAsAdminOrGuest(model, context)
        };

    private static void ClearTeacherData(User model, DbContext context)
    {
        model.TeacherId = null;
        if (model.Teacher is null)
        {
            return;
        }

        model.Teacher.UserId = null;
        context.Update(model.Teacher);
        model.Teacher = null;
    }

    private static void ClearStudentData(User model, DbContext context)
    {
        model.StudentId = null;
        if (model.Student is null)
        {
            return;
        }

        model.Student.UserId = null;
        context.Update(model.Student);
        model.Student = null;
    }

    private Task<User> UpdateAsStudent(User model, DbContext context)
    {
        ClearTeacherData(model, context);
        if (model.Student is null)
        {
            return _baseUpdate.UpdateAsync(model, context);
        }
        model.Student.UserId = model.Id;
        model.StudentId = model.Student.Id;
        return _baseUpdate.UpdateAsync(model, context);
    }
    
    private Task<User> UpdateAsTeacher(User model, DbContext context)
    {
        ClearStudentData(model, context);
        if (model.Teacher is null)
        {
            return _baseUpdate.UpdateAsync(model, context);
        }
        model.Teacher.UserId = model.Id;
        model.TeacherId = model.Teacher.Id;
        return _baseUpdate.UpdateAsync(model, context);
    }
    
    private Task<User> UpdateAsAdminOrGuest(User model, DbContext context)
    {
        ClearStudentData(model, context);
        ClearTeacherData(model, context);
        return _baseUpdate.UpdateAsync(model, context);
    }
}