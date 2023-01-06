using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.Mocks.HttpRequests;

namespace UniversityTimetable.Tests.Integration.AuthHelpers;

public static class HttpClientExtensions
{
    public static async Task Login(this HttpClient client, User user)
    {
        var login = new LoginDto { Email = user.Email, Password = user.Password };
        var response = await client.PostAsJsonAsync("api/Auth/login", login);
        var token = await JsonSerializer.DeserializeAsync<LoginResult>(await response.Content.ReadAsStreamAsync(),
            HttpClientFactory.Options);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token?.Token);
    }
}