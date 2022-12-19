using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Authorization;

namespace UniversityTimetable.Domain.Authorization
{
    public class AuthService : IAuthService
    {
        public Task<Token> Login(LoginDTO login, HttpContext context)
        {
            return Task.FromResult(new Token { AccessToken = "abc", Email = login.Email });
        }

        public Task Logout(string username)
        {
            return Task.CompletedTask;
        }
    }
}
