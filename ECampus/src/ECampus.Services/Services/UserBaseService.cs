using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Services;

public class UserBaseService : IBaseService<UserDto>
{
    private readonly IBaseService<UserDto> _realService;

    public UserBaseService(IBaseService<UserDto> baseService,
        Lazy<UserRolesService> userRolesService,
        IHttpContextAccessor httpContextAccessor)
    {
        var currentRequestQueryString = httpContextAccessor.HttpContext!.Request.Path;
        if (currentRequestQueryString.ToString().ToLower().Contains("userrole"))
        {
            _realService = userRolesService.Value;
            return;
        }
        _realService = baseService;
    }

    public Task<UserDto> GetByIdAsync(int id, CancellationToken token = default)
    {
        return _realService.GetByIdAsync(id, token);
    }

    public Task<UserDto> CreateAsync(UserDto entity, CancellationToken token = default)
    {
        return _realService.CreateAsync(entity, token);
    }

    public Task<UserDto> UpdateAsync(UserDto entity, CancellationToken token = default)
    {
        return _realService.UpdateAsync(entity, token);
    }

    public Task<UserDto> DeleteAsync(int id, CancellationToken token = default)
    {
        return _realService.DeleteAsync(id, token);
    }
}