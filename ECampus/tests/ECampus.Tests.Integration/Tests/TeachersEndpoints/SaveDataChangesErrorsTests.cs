﻿using System.Net;
using System.Net.Http.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;
using Newtonsoft.Json;

namespace ECampus.Tests.Integration.Tests.TeachersEndpoints;

public class SaveDataChangesErrorsTests : IClassFixture<ApplicationFactory>
{
    private readonly HttpClient _client;

    public SaveDataChangesErrorsTests(ApplicationFactory app)
    {
        _client = app.CreateClient();
        _client.Login(UserRole.Admin);
    }

    [Fact]
    public async Task Post_ShouldReturn400_WhenFkViolates()
    {
        var teacher = new TeacherDto { FirstName = "fn", LastName = "ln" };

        var response = await _client.PostAsJsonAsync("/api/Teachers", teacher);
        var responseObject =
            JsonConvert.DeserializeObject<BadResponseObject>(await response.Content.ReadAsStringAsync())!;

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseObject.Message.Should().Be($"Error occured while saving entity of type{typeof(Teacher)} details\nError code: 400");
    }
}