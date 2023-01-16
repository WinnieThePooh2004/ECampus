using Microsoft.AspNetCore.Components;

namespace UniversityTimetable.FrontEnd.Components.Input;

public partial class Checkbox
{
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

    [Parameter] public EventCallback<bool> ValueChanged { get; set; }
    private bool _isChecked;
}