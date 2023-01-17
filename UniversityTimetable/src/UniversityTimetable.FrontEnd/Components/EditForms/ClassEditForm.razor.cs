using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace UniversityTimetable.FrontEnd.Components.EditForms;

public partial class ClassEditForm
{
    [Parameter] public TimetableMode TimetableMode { get; set; }
}