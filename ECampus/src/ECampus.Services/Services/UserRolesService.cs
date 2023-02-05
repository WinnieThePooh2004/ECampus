using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Services;

public class UserRolesService : IBaseService<UserDto>
{
    private readonly IUserRolesRepository _dataAccess;
    private readonly IMapper _mapper;

    public UserRolesService(IUserRolesRepository dataAccess, IMapper mapper)
    {
        _dataAccess = dataAccess;
        _mapper = mapper;
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        return _mapper.Map<UserDto>(await _dataAccess.GetByIdAsync(id));
    }

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        return _mapper.Map<UserDto>(await _dataAccess.UpdateAsync(_mapper.Map<User>(user)));
    }
    
    public async Task<UserDto> CreateAsync(UserDto user)
    {
        return _mapper.Map<UserDto>(await _dataAccess.CreateAsync(_mapper.Map<User>(user)));
    }

    public async Task<UserDto> DeleteAsync(int id)
    {
        return _mapper.Map<UserDto>(await _dataAccess.DeleteAsync(id));
    }
}