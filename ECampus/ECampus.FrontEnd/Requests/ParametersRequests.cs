using System.Diagnostics;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.FrontEnd.Requests.Options;
using ECampus.Shared.DataContainers;
using ECampus.Shared.Extensions;
using ECampus.Shared.QueryParameters;
using Newtonsoft.Json;

namespace ECampus.FrontEnd.Requests;

public class ParametersRequests<TData, TParameters> : IParametersRequests<TData, TParameters>
    where TData : class
    where TParameters : IQueryParameters
{
    private readonly string _controllerName;
    private readonly IHttpClientFactory _client;

    public ParametersRequests(IHttpClientFactory clientFactory, IRequestOptions options)
    {
        _client = clientFactory;
        _controllerName = options.GetControllerName(typeof(TData));
    }

    public async Task<ListWithPaginationData<TData>> GetByParametersAsync(TParameters parameters)
    {
        var response = await _client.CreateClient(RequestOptions.ClientName)
            .GetAsync($"/api/{_controllerName}?{parameters.ToQueryString()}");
        response.EnsureSuccessStatusCode();
        return JsonConvert.DeserializeObject<ListWithPaginationData<TData>>(await response.Content.ReadAsStringAsync())
               ?? throw new UnreachableException($"cannot get list with objects of type {typeof(TData)}");
    }
}