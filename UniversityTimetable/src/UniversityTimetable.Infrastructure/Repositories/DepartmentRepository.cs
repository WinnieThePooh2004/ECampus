using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class DepartmentRepository : IRepository<Department, DepartmentParameters>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public DepartmentRepository(ApplicationDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Department> CreateAsync(Department entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Department() { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(f => f.Id == id);
            if (department == null)
            {
                throw new Exception();
            }
            return department;
        }

        public async Task<ListWithPaginationData<Department>> GetByParameters(DepartmentParameters parameters)
        {
            var entities = _context.Departments
                .Where(d => (string.IsNullOrEmpty(parameters.DepartmentName) || d.Name == parameters.DepartmentName)
                        && d.FacultacyId == parameters.FacultacyId);
            int totalCount = await entities.CountAsync();

            var pagedItems = await entities
                .OrderBy(a => a.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new ListWithPaginationData<Department>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
