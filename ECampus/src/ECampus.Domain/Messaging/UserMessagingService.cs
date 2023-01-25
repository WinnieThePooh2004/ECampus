using ECampus.Domain.Mapping.Messages;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Messaging;

namespace ECampus.Domain.Messaging;

public class UserMessagingService : IBaseService<UserDto>
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public UserMessagingService(IBaseService<UserDto> baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public Task<UserDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public async Task<UserDto> CreateAsync(UserDto entity)
    {
        var createdUser = await _baseService.CreateAsync(entity);
        await _snsMessenger.PublishMessageAsync(createdUser.ToCreatedUserMessage());
        return createdUser;
    }

    public async Task<UserDto> UpdateAsync(UserDto entity)
    {
        var createdUser = await _baseService.UpdateAsync(entity);
        await _snsMessenger.PublishMessageAsync(createdUser.ToUserUpdatedMessage());
        return createdUser;
    }

    public async Task<UserDto> DeleteAsync(int? id)
    {
        var deletedUser = await _baseService.DeleteAsync(id);
        await _snsMessenger.PublishMessageAsync(deletedUser.ToUserDeletedMessage());
        return deletedUser;
    }
}