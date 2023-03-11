using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.TaskSubmissions;

public partial class TeacherView
{
    [Parameter] public int CourseTaskId { get; set; }
}