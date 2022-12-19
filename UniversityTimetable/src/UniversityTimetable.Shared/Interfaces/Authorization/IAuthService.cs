using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Authorization
{
    public interface IAuthService
    {
        public Task<Token> Login(LoginDTO login, Microsoft.AspNetCore.Http.HttpContext context);
        public Task Logout(string username);
    }
}