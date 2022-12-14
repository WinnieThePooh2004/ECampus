using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.PageOptions
{
    public partial class DataPageSizeSelect
    {
        private static readonly int[] _pageSizes = { 5, 10, 20, 50, 100 };

        [Parameter] required public EventCallback<int> OnPageSizeChanged { get; set; }
        [Parameter] public int PageSize { get; set; } = 5;
        private Task PageSizeChanged(int pageSize)
        {
            return OnPageSizeChanged.InvokeAsync(pageSize);
        }
    }
}
