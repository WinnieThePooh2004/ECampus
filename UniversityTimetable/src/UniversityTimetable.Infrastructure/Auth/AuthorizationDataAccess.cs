using System.Net;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.Auth;

public class AuthorizationDataAccess : IAuthorizationDataAccess
{
    private readonly ApplicationDbContext _context;

    public AuthorizationDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email)
                   ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                       $"User with email {email} does not exist");
        return user;
    }
}