using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Extentions;
using System.Net;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository<User> _baseService;
        private readonly ILogger<UserRepository> _logger;
        private readonly IRelationshipsRepository<User, Auditory, UserAuditory> _userAuditoryRelations;
        private readonly IRelationshipsRepository<User, Group, UserGroup> _userGroupRelations;
        private readonly IRelationshipsRepository<User, Teacher, UserTeacher> _userTeacherRelations;

        public UserRepository(ApplicationDbContext context, IBaseRepository<User> baseService, ILogger<UserRepository> logger,
            IRelationshipsRepository<User, Auditory, UserAuditory> userAuditoryRelations,
            IRelationshipsRepository<User, Group, UserGroup> userGroupRelations,
            IRelationshipsRepository<User, Teacher, UserTeacher> userTeacherRelations)
        {
            _context = context;
            _baseService = baseService;
            _logger = logger;
            _userAuditoryRelations = userAuditoryRelations;
            _userGroupRelations = userGroupRelations;
            _userTeacherRelations = userTeacherRelations;
        }

        public async Task<User> CreateAsync(User entity)
        {
            return await _baseService.CreateAsync(entity);
        }

        public Task DeleteAsync(int id)
        {
            return _baseService.DeleteAsync(id);
        }

        public async Task<User> SignIn(User user)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if(userFromDb is null)
            {
                _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.NotFound, "No user with this email found", user));
                return null;
            }
            if(userFromDb.Password != user.Password)
            {
                _logger.LogAndThrowException(new InfrastructureExceptions(HttpStatusCode.BadRequest, "Invalid password of email"));
            }
            return userFromDb;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.SavedGroupsIds)
                .ThenInclude(g => g.Group)
                .Include(u => u.SavedAuditoriesIds)
                .ThenInclude(a => a.Auditory)
                .Include(u => u.SavedTeachersIds)
                .ThenInclude(t => t.Teacher)
                .FirstOrDefaultAsync(u => u.Id == id);
            if(user is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(User), id));
                return null;
            }
            user.SavedGroups = user.SavedGroupsIds.Select(g => g.Group).ToList();
            user.SavedTeachers = user.SavedTeachersIds.Select(t => t.Teacher).ToList();
            user.SavedAuditories = user.SavedAuditoriesIds.Select(a => a.Auditory).ToList();

            return user;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            await _userAuditoryRelations.UpdateRelations(entity);
            await _userGroupRelations.UpdateRelations(entity);
            await _userTeacherRelations.UpdateRelations(entity);

            return await _baseService.UpdateAsync(entity);
        }

        public Task<Dictionary<string, string>> ValidateAsync(User user)
        {
            var errors = new Dictionary<string, string>();
            if(user.Role == UserRole.Admin)
            {
                errors.Add(nameof(user.Role), "Cannot registed new admin");
            }

            return Task.FromResult(errors);
        }
    }
}
