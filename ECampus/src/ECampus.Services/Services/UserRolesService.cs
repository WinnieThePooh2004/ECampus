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
    private readonly IDataAccessManager _dataAccess;

    public UserRolesService(IMapper mapper, IDataAccessManager dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<UserDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        var user = await _dataAccess.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters(id),
            token);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(UserDto entity, CancellationToken token = default)
    {
        var user = _mapper.Map<User>(entity);
        return user.Role switch
        {
            UserRole.Admin or UserRole.Guest => await CreateAsAdminOrGuest(user, token),
            UserRole.Student => await CreateAsStudent(user, token),
            UserRole.Teacher => await CreateAsTeacher(user, token),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    public async Task<UserDto> DeleteAsync(int id, CancellationToken token = default)
    {
        var user = await _dataAccess.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters(id),
            token);
        var result = _dataAccess.Delete(user);
        await _dataAccess.SaveChangesAsync(token);
        return await EndUpdateAsync(result, token);
    }

    public async Task<UserDto> UpdateAsync(UserDto entity, CancellationToken token = default)
    {
        var user = _mapper.Map<User>(entity);
        var userFromDb =
            await _dataAccess.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters(user.Id), token);
        userFromDb.Username = user.Username;
        if (userFromDb.Role == user.Role)
        {
            return await EndUpdateAsync(userFromDb, token);
        }

        return user.Role switch
        {
            UserRole.Admin or UserRole.Guest => await UpdateAsAdminOrGuest(userFromDb, user, token),
            UserRole.Student => await UpdateAsStudent(userFromDb, user, entity, token),
            UserRole.Teacher => await UpdateAsTeacher(entity, userFromDb, user, token),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        };
    }

    private async Task<UserDto> UpdateAsTeacher(UserDto entity, User userFromDb, User user, CancellationToken token)
    {
        if (userFromDb.Student is not null)
        {
            userFromDb.Student.UserEmail = null;
            userFromDb.StudentId = null;
            userFromDb.Student = null;
        }

        userFromDb.TeacherId = entity.TeacherId;
        var selectedTeacher = await _dataAccess.GetPureByIdAsync<Teacher>((int)user.TeacherId!, token);
        selectedTeacher.UserEmail = user.Email;
        return await EndUpdateAsync(userFromDb, token);
    }

    private async Task<UserDto> UpdateAsStudent(User userFromDb, User user, UserDto entity, CancellationToken token)
    {
        if (userFromDb.Teacher is not null)
        {
            userFromDb.Teacher.UserEmail = null;
            userFromDb.TeacherId = null;
            userFromDb.Teacher = null;
        }

        userFromDb.StudentId = entity.StudentId;
        var selectedStudent = await _dataAccess.GetPureByIdAsync<Student>((int)user.StudentId!, token);
        selectedStudent.UserEmail = user.Email;
        return await EndUpdateAsync(userFromDb, token);
    }

    private async Task<UserDto> UpdateAsAdminOrGuest(User userFromDb, User user, CancellationToken token)
    {
        if (userFromDb.Student is not null)
        {
            userFromDb.Student.UserEmail = null;
            userFromDb.StudentId = null;
            userFromDb.Student = null;
        }

        if (userFromDb.Teacher is null)
        {
            return await EndUpdateAsync(user, token);
        }

        userFromDb.Teacher.UserEmail = null;
        userFromDb.TeacherId = null;
        userFromDb.Teacher = null;
        return await EndUpdateAsync(user, token);
    }

    private async Task<UserDto> EndUpdateAsync(User user, CancellationToken token)
    {
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(user);
    }

    private async Task<UserDto> CreateAsTeacher(User user, CancellationToken token)
    {
        user.StudentId = null;
        user.Student = null;
        var teacherFromDb = await _dataAccess.GetPureByIdAsync<Teacher>((int)user.TeacherId!, token);
        teacherFromDb.UserEmail = user.Email;
        return await EndCreateAsync(user, token);
    }

    private async Task<UserDto> CreateAsStudent(User user, CancellationToken token)
    {
        user.Teacher = null;
        user.TeacherId = null;
        var studentFromDb = await _dataAccess.GetPureByIdAsync<Student>((int)user.StudentId!, token);
        studentFromDb.UserEmail = user.Email;
        return await EndCreateAsync(user, token);
    }

    private async Task<UserDto> EndCreateAsync(User user, CancellationToken token)
    {
        await _dataAccess.CreateAsync(user, token);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(user);
    }

    private Task<UserDto> CreateAsAdminOrGuest(User user, CancellationToken token)
    {
        user.Student = null;
        user.StudentId = null;
        user.Teacher = null;
        user.TeacherId = null;
        return EndCreateAsync(user, token);
    }
}