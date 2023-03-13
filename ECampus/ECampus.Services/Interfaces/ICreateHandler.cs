using ECampus.Domain.Commands;
using ECampus.Domain.Responses;

namespace ECampus.Services.Interfaces;

public interface ICreateHandler<in TCreateCommand>
    where TCreateCommand : class, ICreateCommand
{
    Task<CreatedResponse> CreateAsync(TCreateCommand createCommand, CancellationToken token);
}