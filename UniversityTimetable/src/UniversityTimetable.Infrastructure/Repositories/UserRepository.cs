using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository<User> _baseService;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, IBaseRepository<User> baseService, ILogger<UserRepository> logger)
        {
            _context = context;
            _baseService = baseService;
            _logger = logger;
        }

        public Task<User> CreateAsync(User entity) 
            => _baseService.CreateAsync(entity);

        public Task DeleteAsync(int id) 
            => _baseService.DeleteAsync(id);

        public Task<User> GetByIdAsync(int id) => 
            _baseService.GetByIdAsync(id);

        public Task<User> UpdateAsync(User entity) 
            => _baseService.UpdateAsync(entity);

        public async Task<Dictionary<string, string>> ValidateAsync(User user)
        {
            var errors = new Dictionary<string, string>();
            if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != user.Id))
            {
                errors.Add(nameof(user.Email), "This email is already user");
            }
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                errors.Add(nameof(user.Email), "This username is already user");
            }
            return errors;
        }
        
        
    }
}
