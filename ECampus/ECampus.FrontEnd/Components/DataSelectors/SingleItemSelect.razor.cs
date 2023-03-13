using ECampus.Domain.Comparing;
using ECampus.Domain.Data;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.DataSelectors;

public sealed partial class SingleItemSelect<TData, TParameters>
    where TData : class, IMultipleItemsResponse, new()
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public EventCallback<int> SelectedIdChanged { get; set; }
    [Parameter] public EventCallback<TData> SelectChanged { get; set; }
    [Parameter] public int? SelectedId { get; set; }
    
    private int TotalColumns => TableHeaders.Count;

    private Dictionary<TData, bool> Select { get; } = new(new DataTransferObjectComparer<TData>());

    private bool this[TData item]
    {
        get
        {
            Select.TryAdd(item, false);
            if (item.Id == SelectedId)
            {
                Select[item] = true;
            }
            return Select[item];
        }
        set
        {
            if (!value)
            {
                return;
            }
            var selectedValue = new TData { Id = SelectedId ?? -1 };
            if (Select.ContainsKey(selectedValue))
            {
                Select[selectedValue] = false;
            }
            SelectedId = item.Id;
            SelectedIdChanged.InvokeAsync(item.Id);
            SelectChanged.InvokeAsync(item);
            Select[item] = true;
            StateHasChanged();
        }
    }
}