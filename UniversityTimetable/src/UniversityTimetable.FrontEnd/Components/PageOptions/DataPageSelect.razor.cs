using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.PageOptions
{
    public partial class DataPageSelect
    {
        [Parameter] required public int MaxPageNumber { get; set; }
        [Parameter] required public EventCallback<int> OnPageNumberChanged { get; set; }
        private int _pageNumber = 1;

        private Task PageNumberChanged(int pageNumber)
        {
            return OnPageNumberChanged.InvokeAsync(pageNumber);
        }
    }
}
