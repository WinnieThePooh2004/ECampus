using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UniversityTimetable.Shared.Interfaces.Data;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.FrontEnd.Components.PageModels
{
    public partial class DataTable<TData, TParameters>
        where TData : class, IDataTransferObject
        where TParameters : class, IQueryParameters, new()
    {
        [Parameter] public string CreateLink { get; set; }
        [Parameter] public bool ShowDeleteButton { get; set; } = true;
        [Parameter] public bool ShowEditButton { get; set; } = true;
        /// <summary>
        /// provide it without id
        /// </summary>
        [Parameter] public string EditLink { get; set; }
        [Parameter] public List<string> TableHeaders { get; set; } = new();
        [Parameter] public List<Func<TData, string>> TableData { get; set; } = new();
        [Parameter] public List<(string, Func<TData, string>)> ActionLinks { get; set; } = new();
        [Parameter] public Action<TParameters> ParameterOptions { get; set; } = _ => { };
        
        [Inject] private AuthenticationStateProvider AuthProvider { get; set; }
        private int TotalLinks => ActionLinks.Count + (ShowDeleteButton ? 1 : 0) + (ShowEditButton ? 1 : 0);
        private bool _isAdmin;
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            _isAdmin = authState.User.IsInRole(nameof(UserRole.Admin));
            ParameterOptions(Parameters);
            await base.OnInitializedAsync();
        }
    }
}
