using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Pages.Timetable
{
    public partial class Group
    {
        [Parameter] public int GroupId { get; set; }
    }
}
