using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Domain.DataTransferObjects;
using FluentValidation;
using Microsoft.AspNetCore.Components;

namespace ECampus.FrontEnd.Pages.TaskSubmissions;

public partial class EditSubmissionContent
{
    [Parameter] public int CourseTaskId { get; set; }
    [Inject] private ITaskSubmissionRequests TaskSubmissionRequests { get; set; } = default!;
    [Inject] private IValidator<TaskSubmissionDto> Validator { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    private TaskSubmissionDto? _model;

    protected override async Task OnInitializedAsync()
    {
        _model = await TaskSubmissionRequests.GetByCourseTaskAsync(CourseTaskId);
    }

    private async Task Submit()
    {
        await TaskSubmissionRequests.UpdateContextAsync(_model!.Id, _model.SubmissionContent);
        NavigationManager.NavigateTo($"/tasks/{_model.CourseTaskId}");
    }
}