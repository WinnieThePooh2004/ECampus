using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;

namespace ECampus.Services.Services;

public class UserRolesService : IBaseService<UserDto>
{
    private readonly IMapper _mapper;
    private readonly IDataAccessManagerFactory _dataAccessFactory;

    public UserRolesService(IMapper mapper, IDataAccessManagerFactory dataAccessFactory)
    {
        _mapper = mapper;
        _dataAccessFactory = dataAccessFactory;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _dataAccessFactory.Complex.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters
            { UserId = id });
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        return _mapper.Map<UserDto>(await UpdateAsync(_mapper.Map<User>(user)));
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        return _mapper.Map<UserDto>(await CreateAsync(_mapper.Map<User>(user)));
    }

    public async Task<UserDto> DeleteAsync(int id)
    {
        return _mapper.Map<UserDto>(await _dataAccessFactory.Complex.DeleteAsync<User>(id));
    }

    private async Task<User> CreateAsync(User user)
    {
        var teacher = user.Teacher;
        var student = user.Student;
        user.Teacher = null;
        user.Student = null;
        await _dataAccessFactory.Primitive.CreateAsync(user);
        var result = user.Role switch
        {
            UserRole.Teacher when teacher is not null => await SetTeacherId(user, teacher),
            UserRole.Student when student is not null => await SetStudentId(user, student),
            _ => user
        };
        await _dataAccessFactory.Primitive.SaveChangesAsync();
        return result;
    }

    private async Task<User> UpdateAsync(User user)
    {
        await ChangeUserRelationships(user);
        await _dataAccessFactory.Primitive.SaveChangesAsync();
        return user;
    }

    private async Task<User> SetStudentId(User user, Student student)
    {
        student.UserEmail = user.Email;
        user.StudentId = student.Id;
        await _dataAccessFactory.Primitive.UpdateAsync(student);
        return user;
    }

    private async Task<User> SetTeacherId(User user, Teacher teacher)
    {
        teacher.UserEmail = user.Email;
        user.TeacherId = teacher.Id;
        await _dataAccessFactory.Primitive.UpdateAsync(teacher);
        return user;
    }

    private Task ChangeUserRelationships(User user) => user.Role switch
    {
        UserRole.Student => UpdateAsStudent(user),
        UserRole.Teacher => UpdateAsTeacher(user),
        _ => UpdateAsAdminOrGuest(user)
    };


    private async Task ClearTeacherData(User model)
    {
        model.TeacherId = null;
        if (model.Teacher is null)
        {
            return;
        }

        model.Teacher.UserEmail = null;
        await _dataAccessFactory.Primitive.UpdateAsync(model.Teacher);
        model.Teacher = null;
    }

    private async Task ClearStudentData(User model)
    {
        model.StudentId = null;
        if (model.Student is null)
        {
            return;
        }

        model.Student.UserEmail = null;
        await _dataAccessFactory.Primitive.UpdateAsync(model.Student);
        model.Student = null;
    }

    private async Task UpdateAsStudent(User model)
    {
        await ClearTeacherData(model);
        if (model.Student is null)
        {
            return;
        }

        model.Student.UserEmail = model.Email;
        model.StudentId = model.Student.Id;
        await _dataAccessFactory.Primitive.UpdateAsync(model);
    }

    private async Task UpdateAsTeacher(User model)
    {
        await ClearStudentData(model);
        if (model.Teacher is null)
        {
            return;
        }

        model.Teacher.UserEmail = model.Email;
        model.TeacherId = model.Teacher.Id;
        await _dataAccessFactory.Primitive.UpdateAsync(model);
    }

    private async Task UpdateAsAdminOrGuest(User model)
    {
        await ClearStudentData(model);
        await ClearTeacherData(model);
        await _dataAccessFactory.Primitive.UpdateAsync(model);
    }
}