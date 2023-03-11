using System.Linq.Expressions;
using ECampus.FrontEnd.PropertySelectors;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.Data;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Enums;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageBases;

public class DataTableBase<TData, TParameters> : ComponentBase
    where TData : class, IDataTransferObject
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Parameter] public Action<TParameters> ParameterOptions { get; set; } = _ => { };

    [Inject] private IParametersRequests<TData, TParameters> DataRequests { get; set; } = default!;
    
    [Inject] private IPropertySelector<TData> PropertySelector { get; set; } = default!;

    [Inject] private ISearchTermsSelector<TParameters> SearchTermsSelector { get; set; } = default!;
    
    protected ListWithPaginationData<TData>? Data { get; private set; }
    
    protected List<(string header, string propertyName)> TableHeaders { get; private set; } = new();
    protected List<(Expression<Func<string?>>, string placeHolder)> SearchTerms { get; private set; } = new();

    protected TParameters Parameters { get; } = new();

    protected virtual async Task RefreshData()
    {
        Data = await DataRequests.GetByParametersAsync(Parameters);
        StateHasChanged();
    }

    protected List<string> GetAllPropertiesValues(TData item)
    {
        return PropertySelector.GetAllPropertiesValues(item);
    }
    
    protected Task OnTableHeaderClicked(string propertyName)
    {
        if (Parameters.OrderBy == propertyName)
        {
            Parameters.SortOrder = (SortOrder)((int)(Parameters.SortOrder + 1) % 2);
            return RefreshData();
        }

        Parameters.OrderBy = propertyName;
        return RefreshData();
    }

    protected override Task OnInitializedAsync()
    {
        ParameterOptions(Parameters);
        TableHeaders = PropertySelector.GetAllPropertiesNames();
        SearchTerms = SearchTermsSelector.PropertiesExpressions(Parameters);
        return RefreshData();
    }

    protected Task PageNumberChanged(int pageNumber)
    {
        Parameters.PageNumber = pageNumber;
        return RefreshData();
    }

    protected Task PageSizeChanged(int pageSize)
    {
        Parameters.PageSize = pageSize;
        return RefreshData();
    }
}