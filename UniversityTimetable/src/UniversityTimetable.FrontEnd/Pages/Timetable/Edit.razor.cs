using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Pages.Timetable
{
    public partial class Edit
    {
        [Parameter] public int Mode { get; set; }
        protected override string PageAfterSave => $"/Timetable/{(TimetableMode)Mode}/{GetBackLinkObjectId()}";

        private int? GetBackLinkObjectId()
            => (TimetableMode)Mode switch
            {
                TimetableMode.Teacher => Model?.TeacherId,
                TimetableMode.Group => Model?.GroupId,
                TimetableMode.Auditory => Model?.AuditoryId,
                _ => throw new ArgumentOutOfRangeException(nameof(Mode)),
            };
    }
}
