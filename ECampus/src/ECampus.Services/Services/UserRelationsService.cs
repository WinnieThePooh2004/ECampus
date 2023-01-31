﻿using ECampus.Core.Metadata;
using ECampus.Shared.Interfaces.Auth;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Metadata;

namespace ECampus.Services.Services;

[Inject(typeof(IUserRelationsService))]
public class UserRelationsService : IUserRelationsService
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRelationsDataAccessFacade _userDataAccessFacade;

    public UserRelationsService(IAuthenticationService authenticationService, IUserRelationsDataAccessFacade userDataAccessFacade)
    {
        _authenticationService = authenticationService;
        _userDataAccessFacade = userDataAccessFacade;
    }

    public Task SaveAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.SaveAuditory(userId, auditoryId);
    }

    public Task RemoveSavedAuditory(int userId, int auditoryId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.RemoveSavedAuditory(userId, auditoryId);
    }

    public Task SaveGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.SaveGroup(userId, groupId);
    }

    public Task RemoveSavedGroup(int userId, int groupId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.RemoveSavedGroup(userId, groupId);
    }

    public Task SaveTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.SaveTeacher(userId, teacherId);
    }

    public Task RemoveSavedTeacher(int userId, int teacherId)
    {
        _authenticationService.VerifyUser(userId);
        return _userDataAccessFacade.RemoveSavedTeacher(userId, teacherId);
    }
}