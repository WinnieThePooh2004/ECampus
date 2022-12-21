using System.Net;
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

    public AuthorizationService(IAuthorizationRepository repository, ILogger<AuthorizationService> logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserDto> Login(LoginDto login, HttpContext context)
    {
        _logger.LogInformation("Start login with email {Email}, password {Password}", login.Email, login.Password);
        var user = _mapper.Map<UserDto>(await _repository.GetByEmailAsync(login.Email));
        if (user.Password != login.Password)
        {
            _logger.LogAndThrowException(new DomainException(HttpStatusCode.BadRequest, "Wrong password or email"));
        }

        await context.SignInAsync(user);
        return user;
    }

    public async Task Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}