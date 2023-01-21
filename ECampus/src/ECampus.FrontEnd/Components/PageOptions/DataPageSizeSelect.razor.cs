using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageOptions;

public partial class DataPageSizeSelect
{
    private static readonly int[] PageSizes = { 5, 10, 20, 50, 100 };
    [Parameter] public EventCallback<int> OnPageSizeChanged { get; set; }
    [Parameter] public int PageSize { get; set; } = 5;
        
    private Task PageSizeChanged(int pageSize)
    {
        PageSize = pageSize;
        return OnPageSizeChanged.InvokeAsync(pageSize);
    }
}