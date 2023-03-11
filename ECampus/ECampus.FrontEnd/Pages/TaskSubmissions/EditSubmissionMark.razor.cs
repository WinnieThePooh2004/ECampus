using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.TaskSubmissions;

public partial class EditSubmissionMark
{
    [Parameter] public int Id { get; set; }
    [Inject] private ITaskSubmissionRequests TaskSubmissionRequests { get; set; } = default!;
    [Inject] private IBaseRequests<TaskSubmissionDto> BaseRequests { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Inject] private IValidator<TaskSubmissionDto> Validator { get; set; } = default!;

    private TaskSubmissionDto? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await BaseRequests.GetByIdAsync(Id);
    }

    private async Task Save()
    {
        await TaskSubmissionRequests.UpdateMarkAsync(Id, _model!.TotalPoints);
        NavigationManager.NavigateTo($"/submissions/{_model!.CourseTaskId}");
    }
}