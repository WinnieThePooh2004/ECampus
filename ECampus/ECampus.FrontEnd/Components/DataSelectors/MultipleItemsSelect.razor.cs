using ECampus.Domain.Comparing;
using ECampus.Domain.Data;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.DataSelectors;

public sealed partial class MultipleItemsSelect<TData, TParameters>
    where TData : class, IMultipleItemsResponse, new()
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Parameter] public string Title { get; set; } = string.Empty;
    
    [Parameter] public EventCallback OnChanged { get; set; }

    [Parameter] public List<TData> SelectTo { get; set; } = new();

    [Parameter] public List<int> SelectIdsTo { get; set; } = new();

    private Dictionary<TData, bool> Select { get; } =
        new(new Dictionary<TData, bool>(new DataTransferObjectComparer<TData>()));

    protected override async Task RefreshData()
    {
        Select.Clear();
        await base.RefreshData();
        Select.Clear();
    }

    private bool DataRefreshed()
    {
        return Data?.Data.Count == Select.Count;
    }
    
    private int TotalColumns => TableHeaders.Count;

    private void ValueChecked(bool isChecked, TData item)
    {
        var selectInSourceList = SelectTo.FirstOrDefault(i => i.Id == item.Id);
        if (!isChecked && selectInSourceList is not null)
        {
            if (DataRefreshed())
            {
                SelectTo.Remove(selectInSourceList);
                SelectIdsTo.Remove(item.Id);
            }
            this[item] = false;
            return;
        }

        if (DataRefreshed())
        {
            SelectTo.Add(item);
            SelectIdsTo.Add(item.Id);
        }
        this[item] = true;
    }

    private bool this[TData item]
    {
        get
        {
            if (!Select.ContainsKey(item))
            {
                Select.Add(item, SelectTo.Any(i => i.Id == item.Id));
            }

            return Select[item];
        }
        set
        {
            Select[item] = value;
            OnChanged.InvokeAsync();
        }
    }
}