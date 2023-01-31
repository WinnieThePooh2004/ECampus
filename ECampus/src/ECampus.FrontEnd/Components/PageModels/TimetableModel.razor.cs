using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataContainers;
using ECampus.Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageModels;

public partial class TimetableModel
{
    [Parameter] public List<Func<ClassDto, string>> DescriptionList { get; set; } = new();
        
    [Parameter] public string CreateLink { get; set; } = string.Empty;
        
    [Parameter] public string EditLink { get; set; }  = string.Empty;

    [Parameter] public Func<IClassRequests, Task<Timetable>> RefreshData { get; set; } = default!;

    [Parameter] public Func<Timetable, string?>? Title { get; set; }

    [Inject] private IClassRequests Requests { get; set; } = default!;
    [Inject] private IBaseRequests<ClassDto> BaseRequests { get; set; } = default!;
    private Timetable? _timetable;

    protected override Task OnInitializedAsync()
    {
        return Refresh();
    }

    private async Task Delete(int id)
    {
        await BaseRequests.DeleteAsync(id);
        await Refresh();
    }

    private async Task Refresh()
    {
        _timetable = await RefreshData(Requests);
        StateHasChanged();
    }
}