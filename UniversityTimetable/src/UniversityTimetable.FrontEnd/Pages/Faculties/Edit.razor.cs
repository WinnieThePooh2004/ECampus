using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Faculties
{
    public partial class Edit
    {
        protected override string PageAfterSave => "/faculties";
    }
}
