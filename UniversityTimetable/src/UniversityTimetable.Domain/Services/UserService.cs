using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Domain.Services;

public class UserService : IUserService
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly IRelationsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidationFacade<UserDto> _validationFacade;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IPasswordChange _passwordChange;

    public UserService(IBaseService<UserDto> baseService,
        IRelationsDataAccess<User, Auditory, UserAuditory> userAuditoryRelations,
        IRelationsDataAccess<User, Group, UserGroup> userGroupRelations,
        IRelationsDataAccess<User, Teacher, UserTeacher> userTeacherRelations,
        IAuthenticationService authenticationService, IUpdateValidator<PasswordChangeDto> passwordChangeValidator,
        IValidationFacade<UserDto> validationFacade, IPasswordChange passwordChange)
    {
        _baseService = baseService;
        _userAuditoryRelations = userAuditoryRelations;
        _userGroupRelations = userGroupRelations;
        _userTeacherRelations = userTeacherRelations;
        _authenticationService = authenticationService;
        _passwordChangeValidator = passwordChangeValidator;
        _validationFacade = validationFacade;
        _passwordChange = passwordChange;
    }

    public Task<UserDto> CreateAsync(UserDto entity)
        => _baseService.CreateAsync(entity);

    public Task DeleteAsync(int? id)
        => _baseService.DeleteAsync(id);

    public Task<UserDto> GetByIdAsync(int? id)
        => _baseService.GetByIdAsync(id);

    public Task<UserDto> UpdateAsync(UserDto entity)
        => _baseService.UpdateAsync(entity);

    public async Task<List<KeyValuePair<string, string>>> ValidateCreateAsync(UserDto user)
    {
        return await _validationFacade.ValidateCreate(user);
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateUpdateAsync(UserDto user)
    {
        return await _validationFacade.ValidateUpdate(user);
    }

    public async Task<PasswordChangeDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange);
        if (errors.Any())
        {
            throw new ValidationException(typeof(PasswordChangeDto), errors);
        }

        await _passwordChange.ChangePassword(passwordChange);
        return passwordChange;
    }
    
    public Task SaveAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _userAuditoryRelations.CreateRelation(userId, auditoryId);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _userAuditoryRelations.DeleteRelation(userId, auditoryId);
    }

    public Task SaveGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _userGroupRelations.CreateRelation(userId, groupId);
    }

    public Task RemoveSavedGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _userGroupRelations.DeleteRelation(userId, groupId);
    }

    public Task SaveTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _userTeacherRelations.CreateRelation(userId, teacherId);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _userTeacherRelations.DeleteRelation(userId, teacherId);
    }
    
}