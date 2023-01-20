using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.PageOptions;

public partial class SearchTermsGroup
{
    [Parameter] public List<(Expression<Func<string?>>, string placeHolder)> SearchTerms { get; set; } = new();
    
    [Parameter] public EventCallback OnBlur { get; set; }
}