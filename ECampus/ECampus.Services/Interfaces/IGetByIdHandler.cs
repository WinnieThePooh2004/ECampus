using ECampus.Domain.Responses;

namespace ECampus.Services.Interfaces;

public interface IGetByIdHandler<TResponse>
    where TResponse : ISingleItemResponse
{
    Task<TResponse> GetByIdAsync(int id, CancellationToken token);
}