using ECampus.Domain.Commands;

namespace ECampus.Services.Interfaces;

public interface IUpdateHandler<in TUpdateCommand>
    where TUpdateCommand : IUpdateCommand
{
    Task UpdateAsync(TUpdateCommand updateCommand, CancellationToken token);
}