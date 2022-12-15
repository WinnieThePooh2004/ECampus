using Microsoft.AspNetCore.Components;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.FrontEnd.Components.PageModels
{
    public partial class IndexPageModel<TData, TParameters>
        where TData : class, IDataTransferObject
        where TParameters : IQueryParameters, new()
    {
        [Parameter] public string CreateLink { get; set; } = null;
        /// <summary>
        /// provide it without id
        /// </summary>
        [Parameter] public string EditLink { get; set; } = null;
        [Parameter] public List<string> TableHeaders { get; set; }
        [Parameter] public List<Func<TData, string>> TableData { get; set; }
        [Parameter] public List<(string, Func<TData, string>)> ActionLinks { get; set; }

        [Parameter] public Action<TParameters> ParameterOptions { get; set; } = opt => { };

        protected override Task OnInitializedAsync()
        {
            ParameterOptions(Parameters);
            return base.OnInitializedAsync();
        }
    }
}
