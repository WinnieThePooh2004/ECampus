using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Comparing;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.DataSelectors;

public sealed partial class MultipleItemsSelect<TData, TParameters>
    where TData : class, IDataTransferObject, new()
    where TParameters : class, IQueryParameters, new()
{
    [Parameter] public string Title { get; set; }
    [Parameter] public List<string> PropertyNames { get; set; }
    [Parameter] public Action OnChanged { get; set; } = () => { };
    [Parameter] public List<Func<TData, object>> PropertiesToShow { get; set; }
    [Parameter] public List<TData> SelectTo { get; set; }
    private Dictionary<TData, bool> Select { get; set; } = null;

    private DataTransferObjectComparer<TData> _comparer = new();

    private int TotalColumns => PropertiesToShow.Count + 1;
    protected override void OnInitialized()
    {
        Select = new Dictionary<TData, bool>(_comparer);
    }

    private void ValueChecked(bool isChecked, TData item)
    {
        var selectInSourceList = SelectTo.FirstOrDefault(i => i.Id == item.Id);
        if(!isChecked && selectInSourceList is not null)
        {
            SelectTo.Remove(selectInSourceList);
            this[item] = false;
            return;
        }
        SelectTo.Add(item);
        this[item] = true;
    }

    private bool this[TData item]
    {
        get
        {
            if (!Select.ContainsKey(item))
            {
                Select.Add(item, SelectTo.Any(i => _comparer.Equals(i, item)));
            }
            return Select[item];
        }
        set
        {
            Select[item] = value;
            OnChanged();
        }
    }
}