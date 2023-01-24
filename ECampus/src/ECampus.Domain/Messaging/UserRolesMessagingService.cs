using ECampus.Domain.Mapping.Messages;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Messaging;

namespace ECampus.Domain.Messaging;

public class UserRolesMessagingService : IUserRolesService
{
    private readonly IUserRolesService _baseUserRolesService;
    private readonly ISqsMessenger _sqsMessenger;

    public UserRolesMessagingService(IUserRolesService baseUserRolesService, ISqsMessenger sqsMessenger)
    {
        _baseUserRolesService = baseUserRolesService;
        _sqsMessenger = sqsMessenger;
    }

    public Task<UserDto> GetByIdAsync(int id) => _baseUserRolesService.GetByIdAsync(id);

    public async Task<UserDto> UpdateAsync(UserDto user)
    {
        var updatedUser = await _baseUserRolesService.UpdateAsync(user);
        await _sqsMessenger.SendMessageAsync(updatedUser.ToUserUpdatedMessage());
        return updatedUser;
    }

    public async Task<UserDto> CreateAsync(UserDto user)
    {
        var createdUser = await _baseUserRolesService.CreateAsync(user);
        await _sqsMessenger.SendMessageAsync(createdUser.ToCreatedUserMessage());
        return createdUser;
    }
}