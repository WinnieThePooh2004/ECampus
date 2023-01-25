using ECampus.Messaging.Messages;
using MediatR;
using ILogger = Serilog.ILogger;

namespace ECampus.Messaging.MessageHandlers.Users;

public class UserDeletedHandler : IRequestHandler<UserDeleted>
{
    private readonly ILogger _logger;

    public UserDeletedHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task<Unit> Handle(UserDeleted request, CancellationToken cancellationToken)
    {
        _logger.Information("Deleted user {Username}", request.Username);
        return Unit.Task;
    }
}