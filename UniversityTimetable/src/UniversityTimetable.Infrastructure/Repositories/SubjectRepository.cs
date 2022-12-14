using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class SubjectRepository : IRepository<Subject, SubjectParameters>
    {
        private readonly ILogger<SubjectRepository> _logger;
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ILogger<SubjectRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Subject> CreateAsync(Subject entity)
        {
            entity.TeacherIds = entity.Teachers.Select(t => new SubjectTeacher { TeacherId = t.Id }).ToList();
            entity.Teachers = null;
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Subject { Id = id });
            _context.RemoveRange(await _context.SubjectTeachers.Where(st => st.SubjectId == id).ToListAsync());
            await _context.SaveChangesAsync();
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(t => t.TeacherIds)
                .ThenInclude(st => st.Teacher)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (subject is null)
            {
                throw new Exception();
            }
            subject.Teachers = subject.TeacherIds.Select(t => t.Teacher).ToList();
            return subject;
        }

        public async Task<ListWithPaginationData<Subject>> GetByParameters(SubjectParameters parameters)
        {
            var query = _context.Subjects
                .Include(s => s.TeacherIds)
                .Filter(parameters);
                
            var totalCount = await query.CountAsync();

            var pagedItems = await query
                .OrderBy(c => c.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            return new ListWithPaginationData<Subject>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Subject> UpdateAsync(Subject entity)
        {
            _context.RemoveRange(await _context.SubjectTeachers.Where(st => st.SubjectId == entity.Id).ToListAsync());
            entity.TeacherIds = entity.Teachers.Select(t => new SubjectTeacher { TeacherId = t.Id, SubjectId = entity.Id }).ToList();
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
