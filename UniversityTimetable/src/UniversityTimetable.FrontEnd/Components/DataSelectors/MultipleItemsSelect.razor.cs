using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Comparing;

namespace UniversityTimetable.FrontEnd.Components.DataSelectors
{
    public partial class MultipleItemsSelect<TData, TParameters>
        where TData : class
        where TParameters : IQueryParameters, new()
    {
        [Parameter] public EventCallback<TData> OnAdd { get; set; }
        [Parameter] public EventCallback<TData> OnDelete { get; set; }
        [Parameter] public EventCallback<TData> SelectedValueChanged { get; set; }
        [Parameter] public List<string> PropertyNames { get; set; }
        [Parameter] public List<Func<TData, object>> PropertiesToShow { get; set; }
        [Parameter] public Func<TData, bool> ShowOnly { get; set; } = item => true;
        [Parameter] public List<TData> SelectTo { get; set; }
        protected Dictionary<TData, bool> Select { get; set; } = null;

        private IdComparer<TData> _comparer = new();

        protected override void OnInitialized()
        {
            Select = new(_comparer);
        }

        protected virtual void ValueChecked(bool isChecked, TData item)
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
            set
            {
                Select[item] = value;
            }
        }
    }
}
