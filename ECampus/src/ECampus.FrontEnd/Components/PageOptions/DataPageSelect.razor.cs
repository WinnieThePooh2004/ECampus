using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageOptions;

public partial class DataPageSelect
{
    private const int PagesInLine = 5;
    [Parameter] public int TotalPages { get; set; }
    [Parameter] public EventCallback<int> OnPageNumberChanged { get; set; }
    private int _pageNumber = 1;

    private Task PageNumberChanged(int pageNumber)
    {
        _pageNumber = pageNumber;
        StateHasChanged();
        return OnPageNumberChanged.InvokeAsync(pageNumber);
    }
}