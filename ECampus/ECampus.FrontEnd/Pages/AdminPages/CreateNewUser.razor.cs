using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Validation.Interfaces;
using ECampus.Domain.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.AdminPages;

public partial class CreateNewUser
{
    [Inject] private IBaseRequests<UserDto> UserRolesRequests { get; set; } = default!;
    [Inject] private IUserValidatorFactory UserValidatorFactory { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    
    private readonly UserDto _model = new();
    private IValidator<UserDto> _validator = default!;

    protected override void OnInitialized()
    {
        _validator = UserValidatorFactory.CreateValidator();
    }
    
    private async Task Save()
    {
        await UserRolesRequests.CreateAsync(_model);
        NavigationManager.NavigateTo("/users");
    }
}