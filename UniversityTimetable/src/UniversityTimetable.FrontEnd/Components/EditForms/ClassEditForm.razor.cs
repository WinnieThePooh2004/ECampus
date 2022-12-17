using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms
{
    public partial class ClassEditForm
    {
        [Parameter] public TimetableMode TimetableMode { get; set; }
    }
}
