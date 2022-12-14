using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Departments
{
    public partial class Create
    {
        [Parameter] public int FacultyId { get; set; }
        protected override string PageAfterSave => $"/departments/{FacultyId}";
    }
}
