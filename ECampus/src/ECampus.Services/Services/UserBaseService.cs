using ECampus.Contracts.Services;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Services;

public class UserBaseService : IBaseService<UserDto>
{
    private readonly IBaseService<UserDto> _realService;

    public UserBaseService(IBaseService<UserDto> baseService,
        UserRolesService userRolesService,
        IHttpContextAccessor httpContextAccessor)
    {
        var currentRequestQueryString = httpContextAccessor.HttpContext!.Request.Path;
        if (currentRequestQueryString.ToString().ToLower().Contains("userrole"))
        {
            _realService = userRolesService;
            return;
        }
        _realService = baseService;
    }

    public Task<UserDto> GetByIdAsync(int id)
    {
        return _realService.GetByIdAsync(id);
    }

    public Task<UserDto> CreateAsync(UserDto entity)
    {
        return _realService.CreateAsync(entity);
    }

    public Task<UserDto> UpdateAsync(UserDto entity)
    {
        return _realService.UpdateAsync(entity);
    }

    public Task<UserDto> DeleteAsync(int id)
    {
        return _realService.DeleteAsync(id);
    }
}