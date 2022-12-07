using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class GroupService : IService<GroupDTO, GroupParameters>
    {
        private readonly ILogger<GroupService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Group, GroupParameters> _repository;

        public GroupService(ILogger<GroupService> logger, IMapper mapper, IRepository<Group, GroupParameters> repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<GroupDTO> CreateAsync(GroupDTO entity)
        {
            var group = _mapper.Map<Group>(entity);
            return _mapper.Map<GroupDTO>(await _repository.CreateAsync(group));
        }

        public Task DeleteAsync(int? id)
        {
            if(id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _repository.DeleteAsync((int)id);
        }

        public async Task<GroupDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _mapper.Map<GroupDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<GroupDTO>> GetByParametersAsync(GroupParameters parameters)
        {
            return _mapper.Map<ListWithPaginationData<GroupDTO>>(await _repository.GetByParameters(parameters));
        }

        public async Task<GroupDTO> UpdateAsync(GroupDTO entity)
        {
            var group = _mapper.Map<Group>(entity);
            return _mapper.Map<GroupDTO>(await _repository.UpdateAsync(group));
        }
    }
}
