using AutoMapper;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Interfaces.Repositories;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IBaseService<UserDTO> _baseService;

        public UserService(IUserRepository repository, IMapper mapper, ILogger<UserService> logger, IBaseService<UserDTO> baseService)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _baseService = baseService;
        }

        public async Task<UserDTO> CreateAsync(UserDTO entity)
        {
            var errors = await ValidateAsync(entity);
            if(errors.Any())
            {
                _logger.LogAndThrowException(new ValidationException(typeof(UserDTO), errors));
            }
            return await _baseService.CreateAsync(entity);
        }

        public Task DeleteAsync(int? id)
            => _baseService.DeleteAsync(id);

        public Task<UserDTO> GetByIdAsync(int? id)
            => _baseService.GetByIdAsync(id);

        public async Task<UserDTO> UpdateAsync(UserDTO entity)
        {
            var errors = await ValidateAsync(entity);
            if (errors.Any())
            {
                _logger.LogAndThrowException(new ValidationException(typeof(UserDTO), errors));
            }
            return await _baseService.UpdateAsync(entity);
        }

        public Task<Dictionary<string, string>> ValidateAsync(UserDTO userDTO)
        {
            return _repository.ValidateAsync(_mapper.Map<User>(userDTO));
        }
    }
}
