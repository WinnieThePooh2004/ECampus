using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.PageModels;

public partial class DataTable<TData, TParameters>
    where TData : class, IDataTransferObject
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string? CreateLink { get; set; }
    [Parameter] public bool ShowDeleteButton { get; set; } = true;
    [Parameter] public bool ShowEditButton { get; set; } = true;

    [Parameter]
    public List<(string placeHolder, Expression<Func<TParameters, string?>>)> SearchTerms { get; set; } = new();

    /// <summary>
    /// provide it without id
    /// </summary>
    [Parameter]
    public string EditLink { get; set; } = default!;

    [Parameter] public List<(string header, string propertyName)> TableHeaders { get; set; } = new();
    [Parameter] public List<Func<TData, string>> TableData { get; set; } = new();

    [Parameter] public List<(string LinkName, Func<TData, string> LinkSource)> ActionLinks { get; set; } = new();

    [Parameter] public Action<TParameters> ParameterOptions { get; set; } = _ => { };

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    private int TotalLinks => ActionLinks.Count + (ShowDeleteButton ? 1 : 0) + (ShowEditButton ? 1 : 0);
    private bool _isAdmin;

    protected override async Task OnInitializedAsync()
    {
        _isAdmin = HttpContextAccessor.HttpContext?.User.IsInRole(nameof(UserRole.Admin)) ?? false;
        ParameterOptions(Parameters);
        await base.OnInitializedAsync();
    }

    private Task OnPropertyNameClicked(string propertyName)
    {
        if (Parameters.OrderBy == propertyName)
        {
            Parameters.SortOrder = (SortOrder)((int)(Parameters.SortOrder + 1) % 2);
            return RefreshData();
        }

        Parameters.OrderBy = propertyName;
        return RefreshData();
    }
}