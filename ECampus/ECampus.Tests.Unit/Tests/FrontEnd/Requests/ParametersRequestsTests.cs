﻿using System.Net;
using System.Text.Json;
using ECampus.Domain.Extensions;
using ECampus.Domain.Requests.Auditory;
using ECampus.Domain.Responses;
using ECampus.Domain.Responses.Auditory;
using ECampus.FrontEnd.Requests;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Tests.Shared.Mocks.HttpRequests;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Requests;

public class ParametersRequestsTests : IDisposable
{
    private readonly ParametersRequests<MultipleAuditoryResponse, AuditoryParameters> _sut;
    private readonly Fixture _fixture = new();
    private readonly HttpClientFactory _clientFactory = new();

    public ParametersRequestsTests()
    {
        var requestsOptions = Substitute.For<IRequestOptions>();
        requestsOptions.GetControllerName(Arg.Any<Type>()).Returns("Auditories");
        _sut = new ParametersRequests<MultipleAuditoryResponse, AuditoryParameters>(_clientFactory, requestsOptions);
    }

    public void Dispose()
    {
        _clientFactory.MessageHandler.Responses.Clear();
    }

    [Fact]
    public async Task GetByParameters_ShouldReturnList_WhenStatusCodeIsSuccessful()
    {
        var list = new ListWithPaginationData<MultipleAuditoryResponse>
        {
            Metadata = _fixture.Create<PaginationData>(),
            Data = _fixture.CreateMany<MultipleAuditoryResponse>(10).ToList()
        };
        var parameters = _fixture.Create<AuditoryParameters>();
        var response = new HttpResponseMessage();
        response.Content = new StringContent(JsonSerializer.Serialize(list));
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/Auditories?{parameters.ToQueryString()}"),
            response);

        var result = await _sut.GetByParametersAsync(parameters);

        result.Should().BeEquivalentTo(list,
            opt => 
                opt.ComparingByMembers<ListWithPaginationData<MultipleAuditoryResponse>>());
    }

    [Fact]
    public async Task GetById_ShouldReturnData_WhenStatusCodeIsSuccessful()
    {
        var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadGateway };
        var parameters = _fixture.Create<AuditoryParameters>();
        _clientFactory.MessageHandler.Responses.Add(
            new HttpRequestMessage(HttpMethod.Get, $"https://google.com/api/Auditories?{parameters.ToQueryString()}"),
            response);

        await new Func<Task>(() => _sut.GetByParametersAsync(parameters)).Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Response status code does not indicate success: 502 (Bad Gateway).");
    }
}