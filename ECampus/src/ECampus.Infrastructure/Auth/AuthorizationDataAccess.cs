using System.Net;
using ECampus.Core.Metadata;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Interfaces.Auth;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.Auth;

[Inject(typeof(IAuthorizationDataAccess))]
public class AuthorizationDataAccess : IAuthorizationDataAccess
{
    private readonly ApplicationDbContext _context;

    public AuthorizationDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.Include(u => u.Student).SingleOrDefaultAsync(u => u.Email == email)
                   ?? throw new InfrastructureExceptions(HttpStatusCode.NotFound,
                       $"User with email {email} does not exist");
        return user;
    }
}