﻿using System.Net;
using FluentAssertions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Integration.AppFactories;
using UniversityTimetable.Tests.Integration.AuthHelpers;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Integration.Tests.InstantFailuresTests;

public class VerifyUserFailuresTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;

    public VerifyUserFailuresTests(ApplicationWithoutDatabase factory)
    {
        _client = factory.CreateClient();
        var user = DefaultUsers.Guest;
        factory.Context.Users = new DbSetMock<User>(user);
        _client.Login(user);
    }

    [Fact]
    public async Task UserAuditoryPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Users/auditory?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserAuditoryDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response =
            await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/Users/auditory?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserGroupPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Users/group?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserGroupDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/Users/group?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserTeacherPost_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Users/teacher?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserTeacherDelete_ShouldReturn400_WhenUserIsNotAdminAndIdsNotSame()
    {
        var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "api/Users/teacher?userId=1"));
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}