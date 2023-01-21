using ECampus.Shared.Enums;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.EditForms;

public partial class ClassEditForm
{
    [Parameter] public TimetableMode TimetableMode { get; set; }
}