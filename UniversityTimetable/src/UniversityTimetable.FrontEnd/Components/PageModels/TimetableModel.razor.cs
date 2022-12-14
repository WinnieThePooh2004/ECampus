using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageModels
{
    public partial class TimetableModel
    {
        [Parameter] public List<Func<ClassDTO, string>> DescriptionList { get; set; } = new();
        /// <summary>
        /// takes day of week and number of class as parameters
        /// </summary>
        [Parameter] public Func<int, int, string> CreateLink { get; set; }
        /// <summary>
        /// provide it without id
        /// </summary>
        [Parameter] public string EditLink { get; set; }
        [Parameter] public Func<IClassRequests, Task<UniversityTimetable.Shared.DataContainers.Timetable>> RefreshData { get; set; }

        [Inject] private IClassRequests Requests { get; set; }

        private UniversityTimetable.Shared.DataContainers.Timetable _timetable;

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
            await Requests.DeleteAsync((int)id);
            await Refresh();
        }

        private async Task Refresh()
        {
            _timetable = await RefreshData(Requests);
            StateHasChanged();
        }
    }
}
