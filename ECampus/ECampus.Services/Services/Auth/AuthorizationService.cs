using System.IdentityModel.Tokens.Jwt;
using System.Net;
using ECampus.Core.Installers;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ECampus.Services.Services.Auth;

[Inject(typeof(IAuthorizationService))]
public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtAuthOptions _authOptions;
    private readonly IDataAccessFacade _parametersDataAccess;
    private readonly IUserProfileService _userProfileService;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor, JwtAuthOptions authOptions,
        IDataAccessFacade parametersDataAccess, IUserProfileService userProfileService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authOptions = authOptions;
        _parametersDataAccess = parametersDataAccess;
        _userProfileService = userProfileService;
    }

    public async Task<LoginResult> Login(LoginDto login, CancellationToken token = default)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new HttpContextNotFoundExceptions();
        }

        var user = await _parametersDataAccess.GetSingleAsync<User, UserEmailParameters>(
            new UserEmailParameters(login.Email), token);
        if (user.Password != login.Password)
        {
            throw new DomainException(HttpStatusCode.BadRequest, "Wrong password or email");
        }

        var result = new LoginResult
        {
            Email = user.Email,
            Role = user.Role.ToString(),
            Username = user.Username,
            UserId = user.Id,
            StudentId = user.StudentId,
            TeacherId = user.TeacherId,
            GroupId = user.Student?.GroupId
        };

        return CreateJwt(result);
    }

    public async Task<LoginResult> SignUp(RegistrationDto registrationDto, CancellationToken token = default)
    {
        var userToCreate = new User
        {
            Email = registrationDto.Email, Username = registrationDto.Username, Password = registrationDto.Password
        };

        var user = _parametersDataAccess.Create(userToCreate);
        await _parametersDataAccess.SaveChangesAsync(token);
        
        var result = new LoginResult
        {
            Email = user.Email,
            Role = user.Role.ToString(),
            Username = user.Username,
            UserId = user.Id
        };

        return CreateJwt(result);
    }

    public Task<ValidationResult> ValidateSignUp(RegistrationDto registrationDto, CancellationToken token = default) =>
        _userProfileService.ValidateCreateAsync(
            new UserDto
            {
                Email = registrationDto.Email, Username = registrationDto.Username
            }, token);

    public async Task<ValidationResult> ValidateLogin(LoginDto login, CancellationToken token = default)
    {
        var userWithSameEmail = await _parametersDataAccess
            .SingleOrDefaultAsync<User, UserEmailParameters>(new UserEmailParameters(login.Email), token);

        if (userWithSameEmail is null)
        {
            return new ValidationResult(nameof(login.Email), "User with this id does not exist");
        }

        return userWithSameEmail.Password != login.Password
            ? new ValidationResult(nameof(login.Password), "Passwords dont match")
            : new ValidationResult();
    }
    
    private LoginResult CreateJwt(LoginResult result)
    {
        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: result.CreateClaims(),
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(2)),
            signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        result.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return result;
    }
}