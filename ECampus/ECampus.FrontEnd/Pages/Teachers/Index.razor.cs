using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Teachers;

public partial class Index
{
    [Parameter] public int DepartmentId { get; set; }
}