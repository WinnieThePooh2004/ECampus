using System.Diagnostics;
using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Services.Contracts.Services;

namespace ECampus.Services.Services;

public class UserService : IBaseService<UserDto>
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public UserService(IMapper mapper, IDataAccessFacade dataAccess)
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

    public async Task<UserDto> CreateAsync(UserDto dto, CancellationToken token = default)
    {
        var user = _mapper.Map<User>(dto);
        return user.Role switch
        {
            UserRole.Admin or UserRole.Guest => await CreateAsAdminOrGuest(user, token),
            UserRole.Student => await CreateAsStudent(user, token),
            UserRole.Teacher => await CreateAsTeacher(user, token),
            _ => throw new UnreachableException("", new ArgumentOutOfRangeException(nameof(dto)))
        };
    }

    public async Task<UserDto> DeleteAsync(int id, CancellationToken token = default)
    {
        var user = await _dataAccess.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters(id),
            token);
        var result = _dataAccess.Delete(user);
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(result);
    }

    public async Task<UserDto> UpdateAsync(UserDto dto, CancellationToken token = default)
    {
        var user = _mapper.Map<User>(dto);
        var userFromDb =
            await _dataAccess.GetSingleAsync<User, UserRolesParameters>(new UserRolesParameters(user.Id), token);
        userFromDb.Username = user.Username;
        if (userFromDb.Role == user.Role)
        {
            return await UpdateWhenRoleNotChanged(userFromDb, user, token);
        }

        return user.Role switch
        {
            UserRole.Admin or UserRole.Guest => await UpdateAsAdminOrGuest(userFromDb, user, token),
            UserRole.Student => await UpdateAsStudent(userFromDb, user, dto, token),
            UserRole.Teacher => await UpdateAsTeacher(userFromDb, user, token),
            _ => throw new UnreachableException("", new ArgumentOutOfRangeException(nameof(dto)))
        };
    }

    private async Task<UserDto> UpdateWhenRoleNotChanged(User userFromDb, User user, CancellationToken token)
    {
        return userFromDb.Role switch
        {
            UserRole.Admin or UserRole.Guest => await EndUpdateAsync(userFromDb, user.Role, token),
            UserRole.Student => await UpdateWhenRoleStudentNotChanged(userFromDb, user, token),
            UserRole.Teacher => await UpdateWhenRoleTeacherNotChanged(userFromDb, user, token),
            _ => throw new UnreachableException("", new ArgumentOutOfRangeException(nameof(user)))
        };
    }

    private async Task<UserDto> UpdateWhenRoleTeacherNotChanged(User userFromDb, User user, CancellationToken token)
    {
        if (userFromDb.TeacherId == user.TeacherId)
        {
            return await EndUpdateAsync(userFromDb, user.Role, token);
        }
        
        var newTeacher = await _dataAccess.PureByIdAsync<Teacher>((int)user.TeacherId!, token);
        newTeacher.UserEmail = userFromDb.Email;
        userFromDb.Teacher!.UserEmail = null;
        userFromDb.TeacherId = newTeacher.Id;
        return await EndUpdateAsync(userFromDb, user.Role, token);
    }

    private async Task<UserDto> UpdateWhenRoleStudentNotChanged(User userFromDb, User user, CancellationToken token)
    {
        if (userFromDb.StudentId == user.StudentId)
        {
            return await EndUpdateAsync(userFromDb, user.Role, token);
        }

        var newStudent = await _dataAccess.PureByIdAsync<Student>((int)user.StudentId!, token);
        newStudent.UserEmail = userFromDb.Email;
        userFromDb.Student!.UserEmail = null;
        userFromDb.StudentId = newStudent.Id;
        userFromDb.Student = null;
        return await EndUpdateAsync(userFromDb, user.Role, token);
    }

    private async Task<UserDto> UpdateAsTeacher(User userFromDb, User user, CancellationToken token)
    {
        ClearStudentData(userFromDb);

        userFromDb.TeacherId = user.TeacherId;
        var selectedTeacher = await _dataAccess.PureByIdAsync<Teacher>((int)user.TeacherId!, token);
        selectedTeacher.UserEmail = userFromDb.Email;
        return await EndUpdateAsync(userFromDb, user.Role, token);
    }

    private static void ClearStudentData(User userFromDb)
    {
        if (userFromDb.Student is null)
        {
            return;
        }
        userFromDb.Student.UserEmail = null;
        userFromDb.StudentId = null;
        userFromDb.Student = null;
    }

    private async Task<UserDto> UpdateAsStudent(User userFromDb, User user, UserDto entity, CancellationToken token)
    {
        CreateTeacherData(userFromDb);

        userFromDb.StudentId = entity.StudentId;
        var selectedStudent = await _dataAccess.PureByIdAsync<Student>((int)user.StudentId!, token);
        selectedStudent.UserEmail = userFromDb.Email;
        return await EndUpdateAsync(userFromDb, user.Role, token);
    }

    private static void CreateTeacherData(User userFromDb)
    {
        if (userFromDb.Teacher is null)
        {
            return;
        }
        userFromDb.Teacher.UserEmail = null;
        userFromDb.TeacherId = null;
        userFromDb.Teacher = null;
    }

    private async Task<UserDto> UpdateAsAdminOrGuest(User userFromDb, User user, CancellationToken token)
    {
        ClearStudentData(userFromDb);
        CreateTeacherData(userFromDb);
        return await EndUpdateAsync(user, user.Role, token);
    }

    private async Task<UserDto> EndUpdateAsync(User user, UserRole newRole, CancellationToken token)
    {
        user.Role = newRole;
        await _dataAccess.SaveChangesAsync(token);
        return _mapper.Map<UserDto>(user);
    }

    private async Task<UserDto> CreateAsTeacher(User user, CancellationToken token)
    {
        user.StudentId = null;
        user.Student = null;
        user.Teacher = await _dataAccess.PureByIdAsync<Teacher>((int)user.TeacherId!, token);
        return await EndCreateAsync(user, token);
    }

    private async Task<UserDto> CreateAsStudent(User user, CancellationToken token)
    {
        user.Teacher = null;
        user.TeacherId = null;
        user.Student = await _dataAccess.PureByIdAsync<Student>((int)user.StudentId!, token);
        return await EndCreateAsync(user, token);
    }

    private async Task<UserDto> EndCreateAsync(User user, CancellationToken token)
    {
        _dataAccess.Create(user);
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