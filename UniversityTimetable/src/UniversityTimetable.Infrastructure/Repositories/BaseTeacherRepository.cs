using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class BaseTeacherRepository : IBaseRepository<Teacher>
    {
        private readonly ILogger<BaseTeacherRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRelationshipsRepository<Teacher, Subject, SubjectTeacher> _relationships;
        private readonly IBaseRepository<Teacher> _baseRepository;

        public BaseTeacherRepository(ILogger<BaseTeacherRepository> logger, ApplicationDbContext context,
            IRelationshipsRepository<Teacher, Subject, SubjectTeacher> relationships, IBaseRepository<Teacher> baseRepository)
        {
            _logger = logger;
            _context = context;
            _relationships = relationships;
            _baseRepository = baseRepository;
        }

        public async Task<Teacher> CreateAsync(Teacher entity)
        {
            _relationships.CreateRelationModels(entity);
            return await _baseRepository.CreateAsync(entity);
        }

        public Task DeleteAsync(int id)
            => _baseRepository.DeleteAsync(id);

        public async Task<Teacher> GetByIdAsync(int id)
        {
            var teacher = await _context
                .Teachers
                .Include(t => t.SubjectIds)
                .ThenInclude(st => st.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (teacher is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Teacher), id));
            }
            teacher.Subjects = teacher.SubjectIds.Select(t => t.Subject).ToList();
            return teacher;
        }

        public async Task<Teacher> UpdateAsync(Teacher entity)
        {
            await _relationships.UpdateRelations(entity);
            return await _baseRepository.UpdateAsync(entity);
        }
    }
}
