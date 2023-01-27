﻿using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.DataTransferObjects;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

public class TaskSubmissionRequests : ITaskSubmissionRequests
{
    private readonly string _controllerName;
    private readonly IHttpClientFactory _client;

    public TaskSubmissionRequests(IHttpClientFactory client, IRequestOptions options)
    {
        _client = client;
        _controllerName = options.GetControllerName(typeof(TaskSubmissionDto));
    }

    public async Task UpdateMarkAsync(int taskSubmissionId, int mark)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PutAsJsonAsync($"/api/{_controllerName}/mark/{taskSubmissionId}", mark);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateContextAsync(int taskSubmissionId, string content)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .PutAsJsonAsync($"/api/{_controllerName}/content/{taskSubmissionId}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TaskSubmissionDto> GetByCourseTaskAsync(int courseTaskId)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .GetAsync($"api/TaskSubmissions/byCourseTask/{courseTaskId}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<TaskSubmissionDto>(await response.Content.ReadAsStringAsync())!;
    }
}