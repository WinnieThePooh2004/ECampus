using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.Input;

public partial class Checkbox
{
#pragma warning disable
    [Parameter]
    public bool Value
    {
        get => _isChecked;
        set
        {
            if (_isChecked == value)
            {
                return;
            }

            _isChecked = value;
            ValueChanged.InvokeAsync(value);
        }
    }
#pragma warning restore

    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
    private bool _isChecked;
}