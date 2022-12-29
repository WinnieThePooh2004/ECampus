using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Domain.Services;

public class UserService : IUserService
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly IRelationshipsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidationFacade<UserDto> _validationFacade;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IPasswordChange _passwordChange;
    private readonly IMapper _mapper;

    public UserService(IBaseService<UserDto> baseService,
        IRelationshipsDataAccess<User, Auditory, UserAuditory> userAuditoryRelations,
        IRelationshipsDataAccess<User, Group, UserGroup> userGroupRelations,
        IRelationshipsDataAccess<User, Teacher, UserTeacher> userTeacherRelations,
        IAuthenticationService authenticationService, IUpdateValidator<PasswordChangeDto> passwordChangeValidator,
        IValidationFacade<UserDto> validationFacade, IPasswordChange passwordChange, IMapper mapper)
    {
        _baseService = baseService;
        _userAuditoryRelations = userAuditoryRelations;
        _userGroupRelations = userGroupRelations;
        _userTeacherRelations = userTeacherRelations;
        _authenticationService = authenticationService;
        _passwordChangeValidator = passwordChangeValidator;
        _validationFacade = validationFacade;
        _passwordChange = passwordChange;
        _mapper = mapper;
    }

    public Task<UserDto> CreateAsync(UserDto entity)
        => _baseService.CreateAsync(entity);

    public Task DeleteAsync(int? id)
        => _baseService.DeleteAsync(id);

    public Task<UserDto> GetByIdAsync(int? id)
        => _baseService.GetByIdAsync(id);

    public Task<UserDto> UpdateAsync(UserDto entity)
        => _baseService.UpdateAsync(entity);

    public async Task<List<KeyValuePair<string, string>>> ValidateCreateAsync(UserDto user, HttpContext context)
    {
        return await _validationFacade.ValidateCreate(user);
    }

    public async Task<List<KeyValuePair<string, string>>> ValidateUpdateAsync(UserDto user)
    {
        return await _validationFacade.ValidateCreate(user);
    }

    public async Task<UserDto> ChangePassword(PasswordChangeDto passwordChange)
    {
        var errors = await _passwordChangeValidator.ValidateAsync(passwordChange);
        if (errors.Any())
        {
            throw new ValidationException(typeof(UserDto), errors);
        }

        return _mapper.Map<UserDto>(await _passwordChange.ChangePassword(passwordChange));
    }

    public async Task<List<KeyValuePair<string, string>>> ValidatePasswordChange(PasswordChangeDto passwordChange)
    {
        return await _passwordChangeValidator.ValidateAsync(passwordChange);
    }

    public Task SaveAuditory(ClaimsPrincipal user, int auditoryId)
    {
        _authenticationService.VerifyUser(user);
        return _userAuditoryRelations.CreateRelation(user.GetId(), auditoryId);
    }

    public Task RemoveSavedAuditory(ClaimsPrincipal user, int auditoryId)
    {
        _authenticationService.VerifyUser(user);
        return _userAuditoryRelations.DeleteRelation(user.GetId(), auditoryId);
    }

    public Task SaveGroup(ClaimsPrincipal user, int groupId)
    {
        _authenticationService.VerifyUser(user);
        return _userGroupRelations.CreateRelation(user.GetId(), groupId);
    }

    public Task RemoveSavedGroup(ClaimsPrincipal user, int groupId)
    {
        _authenticationService.VerifyUser(user);
        return _userGroupRelations.DeleteRelation(user.GetId(), groupId);
    }

    public Task SaveTeacher(ClaimsPrincipal user, int teacherId)
    {
        _authenticationService.VerifyUser(user);
        return _userTeacherRelations.CreateRelation(user.GetId(), teacherId);
    }

    public Task RemoveSavedTeacher(ClaimsPrincipal user, int teacherId)
    {
        _authenticationService.VerifyUser(user);
        return _userTeacherRelations.DeleteRelation(user.GetId(), teacherId);
    }
}