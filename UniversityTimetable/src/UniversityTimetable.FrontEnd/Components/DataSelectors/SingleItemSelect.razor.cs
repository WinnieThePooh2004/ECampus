using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Comparing;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.DataSelectors;

public sealed partial class SingleItemSelect<TData, TParameters>
    where TData : class, IDataTransferObject, new()
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string Title { get; set; } = "";
    [Parameter] public EventCallback<int> SelectedIdChanged { get; set; }
    [Parameter] public int SelectedId { get; set; }
    
    private int TotalColumns => TableHeaders.Count;

    private Dictionary<TData, bool> Select { get; set; } = new(new DataTransferObjectComparer<TData>());

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
            var selectedValue = new TData { Id = SelectedId };
            if (Select.ContainsKey(selectedValue))
            {
                Select[selectedValue] = false;
            }
            SelectedId = item.Id;
            SelectedIdChanged.InvokeAsync(item.Id);
            Select[item] = true;
            StateHasChanged();
        }
    }
}