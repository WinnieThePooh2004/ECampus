using Microsoft.AspNetCore.Components;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.FrontEnd.Requests.Interfaces;

namespace UniversityTimetable.FrontEnd.Components.PageModels;

public partial class DetailsPageModel<TData>
{
    [Parameter] public TData Model { get; set; } = default!;
    [Inject] private IPropertySelector<TData> PropertySelector { get; set; } = default!;
}