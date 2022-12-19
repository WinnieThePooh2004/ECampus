using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Authorization;

namespace UniversityTimetable.Domain.Authorization
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        public AuthService(IAuthRepository repository, ILogger<AuthService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDTO> Login(LoginDTO login, HttpContext context)
        {
            var user = await _repository.GetByEmailAsync(login.Email);
            if (user.Password != login.Password)
            {
                _logger.LogAndThrowException(new DomainException(HttpStatusCode.BadRequest, "Wrong password or email"));
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, login.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());

            return _mapper.Map<UserDTO>(user);
        }

        public async Task Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
