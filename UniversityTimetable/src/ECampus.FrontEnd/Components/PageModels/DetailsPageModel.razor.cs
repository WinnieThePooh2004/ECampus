using ECampus.FrontEnd.PropertySelectors;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageModels;

public partial class DetailsPageModel<TData>
{
    [Parameter] public TData Model { get; set; } = default!;
    [Inject] private IPropertySelector<TData> PropertySelector { get; set; } = default!;
}