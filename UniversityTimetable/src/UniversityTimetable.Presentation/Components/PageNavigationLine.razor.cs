using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Components
{
    public partial class PageNavigationLine
    {
        private static int[] _possiblePageSizes = { 5, 10, 20, 50, 100 };
        [Parameter] public QueryParameters Parameters { get; set; } = default!;
        [Parameter] public string ControllerName { get; set; } = string.Empty;
        [Parameter] public int MaxPageNumber { get; set; }
    }
}
