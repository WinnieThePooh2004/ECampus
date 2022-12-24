using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Domain.Services;

public class UserService : IUserService
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly IRelationshipsRepository<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsRepository<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsRepository<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUpdateValidator<UserDto> _updateValidator;
    private readonly ICreateValidator<UserDto> _createValidator;

    public UserService(IBaseService<UserDto> baseService,
        IRelationshipsRepository<User, Auditory, UserAuditory> userAuditoryRelations,
        IRelationshipsRepository<User, Group, UserGroup> userGroupRelations,
        IRelationshipsRepository<User, Teacher, UserTeacher> userTeacherRelations,
        IAuthenticationService authenticationService, ICreateValidator<UserDto> createValidator,
        IUpdateValidator<UserDto> updateValidator)
    {
        _baseService = baseService;
        _userAuditoryRelations = userAuditoryRelations;
        _userGroupRelations = userGroupRelations;
        _userTeacherRelations = userTeacherRelations;
        _authenticationService = authenticationService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public Task<UserDto> CreateAsync(UserDto entity)
        => _baseService.CreateAsync(entity);

    public Task DeleteAsync(int? id)
        => _baseService.DeleteAsync(id);

    public Task<UserDto> GetByIdAsync(int? id)
        => _baseService.GetByIdAsync(id);

    public Task<UserDto> UpdateAsync(UserDto entity)
        => _baseService.UpdateAsync(entity);

    public async Task<Dictionary<string, string>> ValidateCreateAsync(UserDto user, HttpContext context)
    {
        if (user.Role == UserRole.Admin && !context.User.IsInRole(nameof(UserRole.Admin)))
        {
            return new Dictionary<string, string>{ [nameof(user.Role)] = "Cannot register new admin unless register account is not admin" };
        }
        return await _createValidator.ValidateAsync(user);
    }

    public async Task<Dictionary<string, string>> ValidateUpdateAsync(UserDto user)
    {
        return await _updateValidator.ValidateAsync(user);
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