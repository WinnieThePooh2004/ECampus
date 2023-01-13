using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Comparing;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.DataSelectors;

public sealed partial class SingleItemSelect<TData, TParameters>
    where TData : class, IDataTransferObject, new()
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public EventCallback<TData> SelectedItemChanged { get; set; }
    [Parameter] public List<string> PropertyNames { get; set; } = new();
    [Parameter] public List<Func<TData, object>> PropertiesToShow { get; set; } = new();

    [Parameter] public TData? SelectedItem { get; set; }

    private Dictionary<TData, bool> Select { get; set; } = new(new DataTransferObjectComparer<TData>());

    private bool this[TData item]
    {
        get
        {
            Select.TryAdd(item, false);
            if (item.Id == SelectedItem?.Id)
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
            if (SelectedItem is not null && Select.ContainsKey(SelectedItem))
            {
                Select[SelectedItem] = false;
            }
            SelectedItem = item;
            SelectedItemChanged.InvokeAsync(item);
            Select[item] = true;
            StateHasChanged();
        }
    }
}