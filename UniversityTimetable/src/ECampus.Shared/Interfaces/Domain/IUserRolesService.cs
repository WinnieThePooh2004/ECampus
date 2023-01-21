using ECampus.Shared.DataTransferObjects;

namespace ECampus.Shared.Interfaces.Domain;

public interface IUserRolesService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(UserDto user);
    Task<UserDto> CreateAsync(UserDto user);
}