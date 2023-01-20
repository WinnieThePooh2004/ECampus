using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.PageModels;

public partial class IndexPageModel<TData, TParameters>
    where TData : class, IDataTransferObject
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string? CreateLink { get; set; }
    
    [Parameter]
    public string EditLink { get; set; } = default!;

    [Parameter] public List<(string LinkName, Func<TData, string> LinkSource)> ActionLinks { get; set; } = new();
    
    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    private bool _isAdmin;

    protected override async Task OnInitializedAsync()
    {
        _isAdmin = HttpContextAccessor.HttpContext?.User.IsInRole(nameof(UserRole.Admin)) ?? false;
        await base.OnInitializedAsync();
    }
}