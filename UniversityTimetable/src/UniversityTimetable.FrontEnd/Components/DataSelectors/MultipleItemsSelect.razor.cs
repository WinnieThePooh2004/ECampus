using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Comparing;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.FrontEnd.Components.DataSelectors
{
    public sealed partial class MultipleItemsSelect<TData, TParameters>
        where TData : class, IDataTransferObject, new()
        where TParameters : class, IQueryParameters, new()
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public EventCallback<TData> OnAdd { get; set; }
        [Parameter] public EventCallback<TData> OnDelete { get; set; }
        [Parameter] public EventCallback<TData> SelectedValueChanged { get; set; }
        [Parameter] public List<string> PropertyNames { get; set; }
        [Parameter] public List<Func<TData, object>> PropertiesToShow { get; set; }
        [Parameter] public Func<TData, bool> ShowOnly { get; set; } = item => true;
        [Parameter] public List<TData> SelectTo { get; set; }
        private Dictionary<TData, bool> Select { get; set; } = null;

        private DataTransferObjectComparer<TData> _comparer = new();

        protected override void OnInitialized()
        {
            Select = new Dictionary<TData, bool>(_comparer);
        }

        private void ValueChecked(bool isChecked, TData item)
        {
            if(!isChecked && SelectTo.Any(i => _comparer.Equals(i, item)))
            {
                OnDelete.InvokeAsync(item);
                Select[item] = false;
                return;
            }
            OnAdd.InvokeAsync(item);
            Select[item] = true;
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
            set => Select[item] = value;
        }
    }
}
