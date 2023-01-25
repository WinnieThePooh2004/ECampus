using ECampus.Domain.Mapping.Messages;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Messaging;

namespace ECampus.Domain.Messaging;

public class UserMessagingService : IBaseService<UserDto>
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly ISqsMessenger _sqsMessenger;

    public UserMessagingService(IBaseService<UserDto> baseService, ISqsMessenger sqsMessenger)
    {
        _baseService = baseService;
        _sqsMessenger = sqsMessenger;
    }

    public Task<UserDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public async Task<UserDto> CreateAsync(UserDto entity)
    {
        var createdUser = await _baseService.CreateAsync(entity);
        await _sqsMessenger.SendMessageAsync(createdUser.ToCreatedUserMessage());
        return createdUser;
    }

    public async Task<UserDto> UpdateAsync(UserDto entity)
    {
        var createdUser = await _baseService.UpdateAsync(entity);
        await _sqsMessenger.SendMessageAsync(createdUser.ToUserUpdatedMessage());
        return createdUser;
    }

    public async Task<UserDto> DeleteAsync(int? id)
    {
        var deletedUser = await _baseService.DeleteAsync(id);
        await _sqsMessenger.SendMessageAsync(deletedUser.ToUserDeletedMessage());
        return deletedUser;
    }
}