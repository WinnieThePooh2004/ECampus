﻿using ECampus.Domain.Data;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Components.PageModels;

public partial class IndexPageModel<TData, TParameters>
    where TData : class, IMultipleItemsResponse
    where TParameters : class, IQueryParameters<TData>, new()
{
    [Parameter] public string? CreateLink { get; set; }
    
    [Parameter] public string EditLink { get; set; } = default!;

    [Parameter] public List<(string LinkName, Func<TData, string> LinkSource)> ActionLinks { get; set; } = new();
    
    [Parameter] public bool? EditEnabled { get; set; }
    [Parameter] public bool ShowEditButton { get; set; } = true;
    [Parameter] public bool ShowDeleteButton { get; set; } = true;

    [Inject] private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (EditEnabled is not null)
        {
            return;
        }
        EditEnabled = HttpContextAccessor.HttpContext?.User.IsInRole(nameof(UserRole.Admin)) ?? false;
    }
}