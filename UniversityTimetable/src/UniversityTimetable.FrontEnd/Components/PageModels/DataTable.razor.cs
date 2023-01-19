using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UniversityTimetable.FrontEnd.PropertySelectors;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.PageModels;

public partial class DataTable<TData, TParameters>
    where TData : class, IDataTransferObject
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string? CreateLink { get; set; }

    [Parameter]
    public List<(Expression<Func<string?>>, string placeHolder)> SearchTerms { get; set; } = new();

    /// <summary>
    /// provide it without id
    /// </summary>
    [Parameter]
    public string EditLink { get; set; } = default!;

    [Parameter] public List<(string LinkName, Func<TData, string> LinkSource)> ActionLinks { get; set; } = new();

    [Parameter] public Action<TParameters> ParameterOptions { get; set; } = _ => { };

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    [Inject] private IPropertySelector<TData> PropertySelector { get; set; } = default!;

    [Inject] private ISearchTermsSelector<TParameters> SearchTermsSelector { get; set; } = default!;

    private List<(string header, string propertyName)> _tableHeaders = new();
    private bool _isAdmin;

    protected override async Task OnInitializedAsync()
    {
        _tableHeaders = PropertySelector.GetAllPropertiesNames();
        SearchTerms = SearchTermsSelector.PropertiesExpressions(Parameters);
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