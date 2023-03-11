using ECampus.Domain.DataTransferObjects;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Mapping.Messages;
using ECampus.Services.Messaging;

namespace ECampus.Services.Services.Messaging;

public class UserMessagingService : IBaseService<UserDto>
{
    private readonly IBaseService<UserDto> _baseService;
    private readonly ISnsMessenger _snsMessenger;

    public UserMessagingService(IBaseService<UserDto> baseService, ISnsMessenger snsMessenger)
    {
        _baseService = baseService;
        _snsMessenger = snsMessenger;
    }

    public Task<UserDto> GetByIdAsync(int id, CancellationToken token = default) => _baseService.GetByIdAsync(id, token);

    public async Task<UserDto> CreateAsync(UserDto entity, CancellationToken token = default)
    {
        var createdUser = await _baseService.CreateAsync(entity, token);
        await _snsMessenger.PublishMessageAsync(createdUser.ToCreatedUserMessage());
        return createdUser;
    }

    public async Task<UserDto> UpdateAsync(UserDto entity, CancellationToken token = default)
    {
        var createdUser = await _baseService.UpdateAsync(entity, token);
        await _snsMessenger.PublishMessageAsync(createdUser.ToUserUpdatedMessage());
        return createdUser;
    }

    public async Task<UserDto> DeleteAsync(int id, CancellationToken token = default)
    {
        var deletedUser = await _baseService.DeleteAsync(id, token);
        await _snsMessenger.PublishMessageAsync(deletedUser.ToUserDeletedMessage());
        return deletedUser;
    }
}