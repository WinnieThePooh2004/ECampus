using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageModels
{
    public partial class TimetableModel
    {
        [Parameter] public List<Func<ClassDto, string>> DescriptionList { get; set; } = new();

        /// <summary>
        /// takes day of week and number of class as parameters
        /// </summary>
        [Parameter] public string CreateLink { get; set; } = string.Empty;
        /// <summary>
        /// provide it without id
        /// </summary>
        [Parameter] public string EditLink { get; set; }  = string.Empty;

        [Parameter] public Func<IClassRequests, Task<Timetable>> RefreshData { get; set; } = default!;

        [Inject] private IClassRequests Requests { get; set; } = default!;
        [Inject] private IBaseRequests<ClassDto> BaseRequests { get; set; } = default!;
        private Timetable? _timetable;

        protected override Task OnInitializedAsync()
        {
            return Refresh();
        }

        private async Task Delete(int? id)
        {
            if(id is null)
            {
                return;
            }
            await BaseRequests.DeleteAsync((int)id);
            await Refresh();
        }

        private async Task Refresh()
        {
            _timetable = await RefreshData(Requests);
            StateHasChanged();
        }
    }
}
