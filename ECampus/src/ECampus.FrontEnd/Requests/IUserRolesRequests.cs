using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.Requests;

public interface IUserRolesRequests
{
    Task<UserDto> CreateAsync(UserDto user);
    Task<UserDto> GetByIdAsync(int id);
    Task<UserDto> UpdateAsync(UserDto user);
}