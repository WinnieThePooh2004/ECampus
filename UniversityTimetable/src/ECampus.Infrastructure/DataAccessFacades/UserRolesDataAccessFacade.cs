using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(IUserRolesDataAccessFacade))]
public class UserRolesDataAccessFacade : IUserRolesDataAccessFacade
{
    private readonly ApplicationDbContext _context;

    public UserRolesDataAccessFacade(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users
                   .AsNoTracking()
                   .Include(user => user.Student)
                   .Include(user => user.Teacher)
                   .FirstOrDefaultAsync(user => user.Id == id)
               ?? throw new ObjectNotFoundByIdException(typeof(User), id);
    }

    public async Task<User> CreateAsync(User user)
    {
        var teacher = user.Teacher;
        var student = user.Student;
        user.Teacher = null;
        user.Student = null;
        _context.Add(user);
        await _context.SaveChangesAsync();
        return user.Role switch
        {
            UserRole.Teacher when teacher is not null => await SetTeacherId(user, teacher),
            UserRole.Student when student is not null => await SetStudentId(user, student),
            _ => user
        };
    }

    private async Task<User> SetStudentId(User user, Student student)
    {
        student.UserId = user.Id;
        _context.Update(student);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<User> SetTeacherId(User user, Teacher teacher)
    {
        teacher.UserId = user.Id;
        _context.Update(teacher);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        ChangeUserRelationships(user)();
        await _context.SaveChangesAsync();
        return user;
    }

    private Action ChangeUserRelationships(User user) => user.Role switch
    {
        UserRole.Student => () => UpdateAsStudent(user),
        UserRole.Teacher => () => UpdateAsTeacher(user),
        _ => () => UpdateAsAdminOrGuest(user)
    };


    private void ClearTeacherData(User model)
    {
        model.TeacherId = null;
        if (model.Teacher is null)
        {
            return;
        }

        model.Teacher.UserId = null;
        _context.Update(model.Teacher);
        model.Teacher = null;
    }

    private void ClearStudentData(User model)
    {
        model.StudentId = null;
        if (model.Student is null)
        {
            return;
        }

        model.Student.UserId = null;
        _context.Update(model.Student);
        model.Student = null;
    }

    private void UpdateAsStudent(User model)
    {
        ClearTeacherData(model);
        if (model.Student is null)
        {
            return;
        }

        model.Student.UserId = model.Id;
        model.StudentId = model.Student.Id;
        _context.Update(model);
    }

    private void UpdateAsTeacher(User model)
    {
        ClearStudentData(model);
        if (model.Teacher is null)
        {
            return;
        }

        model.Teacher.UserId = model.Id;
        model.TeacherId = model.Teacher.Id;
        _context.Update(model);
    }

    private void UpdateAsAdminOrGuest(User model)
    {
        ClearStudentData(model);
        ClearTeacherData(model);
        _context.Update(model);
    }
}