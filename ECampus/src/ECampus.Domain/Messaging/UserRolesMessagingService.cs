using ECampus.Contracts.Services;
using ECampus.Domain.Interfaces;
using ECampus.Domain.Mapping.Messages;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Domain.Messaging;

public class UserRolesMessagingService : IUserRolesService
{
    private readonly IUserRolesService _baseUserRolesService;
    private readonly ISnsMessenger _snsMessenger;

    public UserRolesMessagingService(IUserRolesService baseUserRolesService, ISnsMessenger snsMessenger)
    {
        _baseUserRolesService = baseUserRolesService;
        _snsMessenger = snsMessenger;
    }

    public Task<UserDto> GetByIdAsync(int id) => _baseUserRolesService.GetByIdAsync(id);

    public Task<UserDto> DeleteAsync(int id) => _baseUserRolesService.DeleteAsync(id);

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        var updatedUser = await _baseUserRolesService.UpdateAsync(user);
        await _snsMessenger.PublishMessageAsync(updatedUser.ToUserUpdatedMessage());
        return updatedUser;
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        var createdUser = await _baseUserRolesService.CreateAsync(user);
        await _snsMessenger.PublishMessageAsync(createdUser.ToCreatedUserMessage());
        return createdUser;
    }
}