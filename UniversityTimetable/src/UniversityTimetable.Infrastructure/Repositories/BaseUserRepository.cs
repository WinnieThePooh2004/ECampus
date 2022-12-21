using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.Repositories;

public class BaseUserRepository : IBaseRepository<User>
{
    private readonly ApplicationDbContext _context;
    private readonly IBaseRepository<User> _baseService;
    private readonly ILogger<BaseUserRepository> _logger;
    private readonly IRelationshipsRepository<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationshipsRepository<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationshipsRepository<User, Teacher, UserTeacher> _userTeacherRelations;

    public BaseUserRepository(IBaseRepository<User> baseService, ApplicationDbContext context, ILogger<BaseUserRepository> logger,
        IRelationshipsRepository<User, Auditory, UserAuditory> userAuditoryRelations,
        IRelationshipsRepository<User, Group, UserGroup> userGroupRelations,
        IRelationshipsRepository<User, Teacher, UserTeacher> userTeacherRelations)
    {
        _baseService = baseService;
        _context = context;
        _logger = logger;
        _userAuditoryRelations = userAuditoryRelations;
        _userGroupRelations = userGroupRelations;
        _userTeacherRelations = userTeacherRelations;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await DbSetWithAllRelatedData()
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

    public Task<User> CreateAsync(User entity)
    {
        _userAuditoryRelations.CreateRelationModels(entity);
        _userGroupRelations.CreateRelationModels(entity);
        _userTeacherRelations.CreateRelationModels(entity);

        return _baseService.UpdateAsync(entity);
    }

    public async Task<User> UpdateAsync(User entity)
    {
        await _userAuditoryRelations.UpdateRelations(entity);
        await _userGroupRelations.UpdateRelations(entity);
        await _userTeacherRelations.UpdateRelations(entity);
        return await _baseService.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _baseService.DeleteAsync(id);

    private IQueryable<User> DbSetWithAllRelatedData()
        => _context.Users
            .Include(u => u.SavedGroupsIds)
            .ThenInclude(g => g.Group)
            .Include(u => u.SavedAuditoriesIds)
            .ThenInclude(a => a.Auditory)
            .Include(u => u.SavedTeachersIds)
            .ThenInclude(t => t.Teacher);
}