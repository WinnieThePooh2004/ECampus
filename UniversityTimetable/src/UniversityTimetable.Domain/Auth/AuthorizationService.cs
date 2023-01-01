﻿using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;

namespace UniversityTimetable.Domain.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly IAuthorizationRepository _repository;
    private readonly ILogger<AuthorizationService> _logger;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IAuthorizationRepository repository, ILogger<AuthorizationService> logger,
        IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserDto> Login(LoginDto login)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        var user = _mapper.Map<UserDto>(await _repository.GetByEmailAsync(login.Email));
        if (user.Password != login.Password)
        {
            _logger.LogAndThrowException(new DomainException(HttpStatusCode.BadRequest, "Wrong password or email"));
        }
        
        await _httpContextAccessor.HttpContext.SignInAsync(user);
        return user;
    }

    public async Task Logout()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}