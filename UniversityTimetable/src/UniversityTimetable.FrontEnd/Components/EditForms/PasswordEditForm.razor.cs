using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Extensions;

namespace UniversityTimetable.FrontEnd.Components.EditForms;

public partial class PasswordEditForm
{
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    protected override void OnInitialized()
    {
        Model.UserId = HttpContextAccessor.HttpContext?.User.GetId() ?? throw new UnauthorizedAccessException();
    }
}