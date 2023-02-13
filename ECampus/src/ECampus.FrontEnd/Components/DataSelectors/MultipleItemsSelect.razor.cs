using ECampus.Shared.Comparing;
using ECampus.Shared.Data;
using ECampus.Shared.QueryParameters;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.DataSelectors;

public sealed partial class MultipleItemsSelect<TData, TParameters>
    where TData : class, IDataTransferObject, new()
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public EventCallback OnChanged { get; set; }
    [Parameter] public List<TData> SelectTo { get; set; } = new();

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
            }
            this[item] = false;
            return;
        }

        if (DataRefreshed())
        {
            SelectTo.Add(item);
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