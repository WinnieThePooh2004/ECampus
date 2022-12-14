using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Infrastructure.Repositories
{
    public class GroupRepository : IRepository<Group, GroupParameters>
    {

        private readonly ILogger<GroupRepository> _logger;
        private readonly ApplicationDbContext _context;

        public GroupRepository(ILogger<GroupRepository> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Group> CreateAsync(Group entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(new Group { Id = id });
            await _context.SaveChangesAsync();
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);
            if (group is null)
            {
                throw new Exception();
            }
            return group;
        }

        public async Task<ListWithPaginationData<Group>> GetByParameters(GroupParameters parameters)
        {
            var query = _context.Groups.Filter(parameters);

            var totalCount = await query.CountAsync();

            var pagedItems = await query
                .OrderBy(c => c.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            return new ListWithPaginationData<Group>(pagedItems, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Group> UpdateAsync(Group entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
