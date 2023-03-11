using ECampus.Shared.DataTransferObjects;

namespace ECampus.Services.Contracts.Services;

public interface IUserRolesService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(UserDto user);
    Task<UserDto> CreateAsync(UserDto user);
    Task<UserDto> DeleteAsync(int id);
}