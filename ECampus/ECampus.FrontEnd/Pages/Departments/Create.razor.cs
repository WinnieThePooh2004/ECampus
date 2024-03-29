﻿using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.Departments;

public partial class Create
{
    [Parameter] public int FacultyId { get; set; }
    protected override string PageAfterSave => $"/departments/{FacultyId}";
}