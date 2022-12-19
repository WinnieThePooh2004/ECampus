using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Authorization;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.Auth;

public class AuthRepository : IAuthRepository
{
    private readonly ILogger<AuthRepository> _logger;
    private readonly ApplicationDbContext _context;

    public AuthRepository(ILogger<AuthRepository> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.BadRequest, $"User with email {email} does not exist"));
        }

        return user;
    }
}