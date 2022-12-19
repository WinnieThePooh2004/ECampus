using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class BaseSubjectRepository : IBaseRepository<Subject>
    {
        private readonly ILogger<BaseSubjectRepository> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IBaseRepository<Subject> _baseRepository;
        private readonly IRelationshipsRepository<Subject, Teacher, SubjectTeacher> _relations;

        public BaseSubjectRepository(ILogger<BaseSubjectRepository> logger, ApplicationDbContext context,
            IBaseRepository<Subject> baseRepository, IRelationshipsRepository<Subject, Teacher, SubjectTeacher> relations)
        {
            _logger = logger;
            _context = context;
            _baseRepository = baseRepository;
            _relations = relations;
        }

        public async Task<Subject> CreateAsync(Subject entity)
        {
            _relations.CreateRelationModels(entity);
            return await _baseRepository.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Subject { Id = id });
            _context.RemoveRange(await _context.SubjectTeachers.Where(st => st.SubjectId == id).ToListAsync());
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Subject), id));
            }
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(t => t.TeacherIds)
                .ThenInclude(st => st.Teacher)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (subject is null)
            {
                _logger.LogAndThrowException(new ObjectNotFoundByIdException(typeof(Subject), id));
            }
            subject.Teachers = subject.TeacherIds.Select(t => t.Teacher).ToList();
            return subject;
        }

        public async Task<Subject> UpdateAsync(Subject entity)
        {
            await _relations.UpdateRelations(entity);
            return await _baseRepository.UpdateAsync(entity);
        }
    }
}
