using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable
{
    public partial class Create
    {
        [Parameter] public int Id { get; set; }
        [Parameter] public int Mode { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "DayOfWeek")]
        public int DayOfWeek { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "Number")]
        public int Number { get; set; }

        protected override string PageAfterSave => $"/Timetable/{(TimetableMode)Mode}/{Id}";

        private ClassDto CreateModel()
            => (TimetableMode)Mode switch
            {
                TimetableMode.Teacher => new() { Number = Number, DayOfWeek = DayOfWeek, TeacherId = Id },
                TimetableMode.Group => new() { Number = Number, DayOfWeek = DayOfWeek, GroupId = Id },
                TimetableMode.Auditory => new() { Number = Number, DayOfWeek = DayOfWeek, AuditoryId = Id },
                _ => throw new ArgumentOutOfRangeException(nameof(Mode), "Enum is out of range"),
            };

    }
}
