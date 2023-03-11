using ECampus.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.EditForms;

public partial class PasswordEditForm
{
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    protected override void OnInitialized()
    {
        Model.UserId = HttpContextAccessor.HttpContext?.User.GetId() ?? throw new UnauthorizedAccessException();
    }
}