﻿using System.Net;
using System.Net.Http.Json;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using FluentAssertions;

namespace ECampus.Tests.Integration.Tests.Endpoints.TeachersEndpoints;

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

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}