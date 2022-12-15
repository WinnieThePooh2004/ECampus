using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.EditForms.Classes
{
    public partial class ClassEditForm
    {
        [Parameter] public TimetableMode TimetableMode { get; set; }
    }
}
