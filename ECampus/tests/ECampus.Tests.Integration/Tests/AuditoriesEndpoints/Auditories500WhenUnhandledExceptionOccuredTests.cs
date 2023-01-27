using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ECampus.Api.MiddlewareFilters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Tests.Integration.AppFactories;
using ECampus.Tests.Integration.AuthHelpers;
using ECampus.Tests.Shared.Mocks.HttpRequests;
using FluentAssertions;
using NSubstitute;

namespace ECampus.Tests.Integration.Tests.AuditoriesEndpoints;

public class Auditories500WhenUnhandledExceptionOccuredTests : IClassFixture<ApplicationWithoutDatabase>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions = HttpClientFactory.Options;

    public Auditories500WhenUnhandledExceptionOccuredTests(ApplicationWithoutDatabase app)
    {
        _client = app.CreateClient();
        app.Context.SaveChangesAsync().Returns(0).AndDoes(_ => throw new Exception());
        _client.Login(DefaultUsers.GetUserByRole(UserRole.Admin));
    }

    [Fact]
    public async Task Delete_ShouldReturn500_WhenExceptionOccuredWhileDbWasSavingChanges()
    {
        var auditory = new AuditoryDto { Id = 10, Name = "n1", Building = "b1"};
        var response = await _client.PutAsJsonAsync("/api/Auditories", auditory);
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var result = JsonSerializer
            .Deserialize<BadResponseObject>(await response.Content.ReadAsStringAsync(), _serializerOptions);
        result.Should().NotBeNull();
        result?.Message.Should().Be("Unhandled exception occured on data access level, view inner exception to see details\nError code: 500");
    }
}