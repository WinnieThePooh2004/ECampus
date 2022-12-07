using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Pagination;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Domain.Services
{
    public class DepartmentService : IService<DepartmentDTO, DepartmentParameters>
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Department, DepartmentParameters> _repository;

        public DepartmentService(IRepository<Department, DepartmentParameters> repository, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DepartmentDTO> CreateAsync(DepartmentDTO entity)
        {
            var department = _mapper.Map<Department>(entity);
            return _mapper.Map<DepartmentDTO>(await _repository.CreateAsync(department));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null)
            {
                throw new Exception();
            }
            await _repository.DeleteAsync((int)id);
        }

        public async Task<DepartmentDTO> GetByIdAsync(int? id)
        {
            if (id is null)
            {
                throw new Exception();
            }
            return _mapper.Map<DepartmentDTO>(await _repository.GetByIdAsync((int)id));
        }

        public async Task<ListWithPaginationData<DepartmentDTO>> GetByParameters(DepartmentParameters parameters)
        {
            _logger.LogInformation("Getting facultacies by parameters:{params}", parameters);
            var facultacies = await _repository.GetByParameters(parameters);
            _logger.LogInformation("By provided parameters found {count} facultacies", facultacies.Metadata.TotalCount);
            return _mapper.Map<ListWithPaginationData<DepartmentDTO>>(facultacies);
        }

        public async Task<DepartmentDTO> UpdateAsync(DepartmentDTO entity)
        {
            var department = _mapper.Map<Department>(entity);
            return _mapper.Map<DepartmentDTO>(await _repository.UpdateAsync(department));
        }
    }
}
