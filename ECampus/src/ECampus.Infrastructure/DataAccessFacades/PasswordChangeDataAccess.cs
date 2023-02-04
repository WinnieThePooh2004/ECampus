using ECampus.Contracts.DataAccess;
using ECampus.Core.Metadata;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Infrastructure.DataAccessFacades;

[Inject(typeof(IPasswordChangeDataAccess))]
public class PasswordChangeDataAccess : IPasswordChangeDataAccess
{
    private readonly DbContext _context;

    public PasswordChangeDataAccess(DbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserAsync(int userId)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(user => user.Id == userId)
               ?? throw new ObjectNotFoundByIdException(typeof(User), userId);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}