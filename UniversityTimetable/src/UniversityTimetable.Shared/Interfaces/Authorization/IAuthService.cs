using Microsoft.AspNetCore.Http;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Shared.Interfaces.Authorization
{
    public interface IAuthService
    {
        public Task<UserDTO> Login(LoginDTO login, HttpContext context);
        public Task Logout(HttpContext context);
    }
}